using Microsoft.AspNetCore.Components;
using UserManagement.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpClient();

builder.Services.AddScoped(sp =>
{
    NavigationManager navigation = sp.GetRequiredService<NavigationManager>();
    //return new HttpClient { BaseAddress = new Uri(navigation.BaseUri) };
    return new HttpClient { BaseAddress = new Uri("https://localhost:7074/") };
});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowBlazorOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7074/");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowBlazorOrigin");

app.UseHttpsRedirection();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();


app.UseAntiforgery();

app.UseCors("AllowBlazorOrigin");

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(UserManagement.Blazor.Client._Imports).Assembly);

app.Run();
