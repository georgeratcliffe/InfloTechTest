using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManagement.Data.Entities;
using Westwind.AspNetCore.Markdown;

var builder = WebApplication.CreateBuilder(args);

var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).CreateLogger("Program");

// Add services to the container.
builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddDomainAuditServices()
    .AddMarkdown()
    .AddControllersWithViews();

builder.Services.AddKeyedScoped<List<AuditEntry>>("Audit", (_, _) => new());

var app = builder.Build();

app.Use(async (context, next) =>
{
    // print request information
    var requestBody = context.Request.Body;

    using (var bodyReader = new StreamReader(context.Request.Body))
    {
        string body = await bodyReader.ReadToEndAsync();
        string message = "Request recieved: " + context.Request.GetEncodedUrl() + " with method: " + context.Request.Method + " and payload: " + body + ".";
        logger.Log(LogLevel.Information, null, message);
        Debug.WriteLine(message);
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
        await next.Invoke();
        context.Request.Body = requestBody;
    }
});

app.UseMarkdown();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
