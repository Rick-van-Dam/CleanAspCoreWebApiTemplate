using System.Diagnostics.CodeAnalysis;
using CleanAspCore.Core.Data;
using CleanAspCore.Core.Data.Extensions;
using CleanAspCore.TestDataGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ExcludeFromCodeCoverage]

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<HrContext>();
builder.Services.AddHostedService<TestDataGeneratorService>();

IHost app = builder.Build();

app.EnsureHrContextDatabaseIsCreated();

await app.RunAsync();
