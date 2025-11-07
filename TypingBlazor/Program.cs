using System.Net.NetworkInformation;
using TypingBlazor.Components;
using TypingTrainer.Logic;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

AddLogicServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static void AddLogicServices(WebApplicationBuilder builder)
{

    builder.Services.AddScoped<ITextProvider, SimpleTextProvider>();
    builder.Services.AddScoped<IStatisticsInfo, SimpleStatisticsInfo>();
    builder.Services.AddScoped<ICorrectChecker, SimpleCorrectChecker>();
    builder.Services.AddScoped<IMistakeProcessor, SimpleMistakeProcessor>();
    builder.Services.AddScoped<ITimerProvider, SimpleTimer>();
    builder.Services.AddScoped<ITyping, Typing>();
}