using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TypingTrainer.Logic;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddLogicServices();
await builder.Build().RunAsync();
