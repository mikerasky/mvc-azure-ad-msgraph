using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using dev365App.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var initialScopes = configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(configuration.GetSection("DownstreamApi"))
                .AddInMemoryTokenCaches();

builder.Services.AddTransient<IGraphService, GraphService>();

builder.Services.AddControllersWithViews(options => {
    var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
