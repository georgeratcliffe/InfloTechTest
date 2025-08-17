using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using UserManagement.Data;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Services.Implementations;
using UserManagement.Services.Interfaces;
using UserManager.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

var logger = LoggerFactory.Create(config =>
{
    config.AddConsole();
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).CreateLogger("Program");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserAuditService, UserAuditService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "UserAPI");
    });
}

app.MapApiEndpoints();

app.Use(async (context, next) =>
{
    // print request information
    var requestBody = context.Request.Body;

    using (var bodyReader = new StreamReader(context.Request.Body))
    {
        string body = await bodyReader.ReadToEndAsync();
        string message = "Request received: " + context.Request.GetEncodedUrl() + " with method: " + context.Request.Method + " and payload: " + body + ".";
        logger.Log(LogLevel.Information, null, message);
        Debug.WriteLine(message);
        context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));
        await next.Invoke();
        context.Request.Body = requestBody;
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
