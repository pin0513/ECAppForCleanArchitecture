using ECApp.Application;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace UnitTests;

[SetUpFixture]
public class UnitTesting
{
    private static IServiceScopeFactory _scopeFactory;
    private static IConfigurationRoot _configuration;

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    [OneTimeSetUp]
    public void RunBeforeAllTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
#if DEBUG
            .AddJsonFile("appsettings.local.json", true, true)
#else
            .AddJsonFile("appsettings.Release.json", true, true)
#endif
            .AddEnvironmentVariables();

        _configuration = builder.Build();
        ServiceCollection services = new ServiceCollection();
        services.AddApplication();

        var serviceProvider = services.BuildServiceProvider();
        ServiceActivator.Configure(serviceProvider);
        _scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();
    }

    private static Dictionary<string, Mock> _mockDict = new Dictionary<string, Mock>();

    protected static void ReplaceWithMock<T>(IServiceCollection services, Action<Mock<T>> setup = null,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) where T : class
    {
        var mock = new Mock<T>();
        if (setup != null)
        {
            setup(mock);
        }

        _mockDict[typeof(T).Name] = mock;
        services.Replace(new ServiceDescriptor(typeof(T), provider => mock.Object, lifetime));
    }
}