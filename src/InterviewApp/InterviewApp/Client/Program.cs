using InterviewApp.Client;
using InterviewApp.Client.Services;
using InterviewApp.Client.Services.Interface;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IInterviewService, InterviewService>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();