using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using TypingBlazor;
using TypingBlazor.Client.Pages;
using TypingBlazor.Components;
using TypingTrainer.Logic;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<TypingUser, IdentityRole>(options=>
{
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;        
    options.Password.RequiredLength = 0;        
    options.Password.RequireLowercase = false;       
    options.Password.RequireUppercase = false;       
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredUniqueChars = 0;       


})
.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();





builder.Services.AddAuthorization();

builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(
        options => options.SerializeAllClaims = true);
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddHttpClient();

builder.Services.AddLogicServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // создаёт базу и таблицы, если их нет
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(TypingBlazor.Client._Imports).Assembly);

app.Run();
