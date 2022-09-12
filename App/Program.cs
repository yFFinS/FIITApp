using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Configure Microsoft Dependency Injection
var services = new ServiceCollection();

// Create logger
services.AddLogging(configure => configure.AddConsole());

// Create service provider
using var serviceProvider = services.BuildServiceProvider();

// Test logger
var logger = serviceProvider.GetService<ILogger<Program>>()!;
logger.LogInformation("Hello World!");

