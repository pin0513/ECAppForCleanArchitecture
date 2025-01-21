using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Respawn;
using System.Linq.Expressions;
using Dapper;
using ECApp.Application;
using ECApp.Application.Interfaces;
using ECApp.Infrastructure;
using ECApp.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Respawn.Graph;

[SetUpFixture]
public class Testing
{
    private static IServiceScopeFactory _scopeFactory;
    private static IConfigurationRoot _configuration;
    
    private static string? TestingMasterDbConnectionString = string.Empty;
    private static string TestECDBConnectionString = string.Empty;
    
    private static Respawner _ecDbRespawner;

    public static string GetDBNameHelper(string conn)
    {
        return new SqlConnectionStringBuilder(conn).InitialCatalog;
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

                      // #endif
                      .AddEnvironmentVariables();

        _configuration = builder.Build();

        TestingMasterDbConnectionString = _configuration.GetConnectionString("MasterDBConn");

        TestECDBConnectionString = _configuration.GetConnectionString("ECDBConn");
        
        CreateIfNotExistTestingDatabase();
        Console.WriteLine("Waiting Database Created");

        //let the bullet flies a while
        //todo: important 這邊等5秒 是因為 資料庫建立需要時間，不然後續 SyncDatabaseSchema 太早執行會死掉
        Thread.Sleep(5000);

        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddLogging();
        services.AddApplication();
        services.AddInfrastructure(_configuration);

        ReplaceWithMock<ICurrentUserService>(services);
        SetupMock<ICurrentUserService>(a =>
        {
            a.Setup(b => b.UserId).Returns(() => Guid.NewGuid());
            a.Setup(b => b.UserAccount).Returns(() => "testUser001");
        });


        var serviceProvider = services.BuildServiceProvider();
        ServiceActivator.Configure(serviceProvider);
        _scopeFactory = serviceProvider.GetService<IServiceScopeFactory>();

        SyncDatabaseSchema();

        var resetDbActions = Respawner.CreateAsync(TestECDBConnectionString, new RespawnerOptions
        {
            TablesToIgnore = new Table[] { "sysdiagrams", "SchemaVersions" },
        });
        _ecDbRespawner = resetDbActions.GetAwaiter().GetResult();
    }

    public static async Task ResetState()
    {
        await _ecDbRespawner.ResetAsync(TestECDBConnectionString);
    }

    private static void CreateIfNotExistTestingDatabase()
    {
        var testExampleDb = "ECDB";

        try
        {
            using var conn = new SqlConnection(TestingMasterDbConnectionString);

            conn.Execute(@$"
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = '{testExampleDb}')
BEGIN
  CREATE DATABASE [{testExampleDb}]
END
");
        }
        catch (SqlException sqlEx)
        {
            Console.WriteLine($"Ensure DataBase {testExampleDb} Error, ErrorMessage: {sqlEx.Message}, StackTrace:{sqlEx.StackTrace}");
        }
    }

    private void SyncDatabaseSchema()
    {
        using var scope = _scopeFactory.CreateScope();
        var ECDBContext = scope.ServiceProvider.GetService<ECDBContext>();

        UpdateSchemaHelper.SyncSchema(ECDBContext, TestECDBConnectionString, GetDBNameHelper(TestECDBConnectionString));
    }

    private static Dictionary<string, Mock> _mockDict = new Dictionary<string, Mock>();

    public static void ReplaceWithMock<T>(IServiceCollection services, Action<Mock<T>> setup = null,
                                          ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where T : class
    {
        var mock = new Mock<T>();

        if (setup != null)
        {
            setup(mock);
        }

        _mockDict[typeof(T).Name] = mock;
        services.Replace(new ServiceDescriptor(typeof(T), provider => mock.Object, lifetime));
    }

    public static void SetupMock<T>(Action<Mock<T>> action)
        where T : class
    {
        if (_mockDict.TryGetValue(typeof(T).Name, out var mock) && mock != null)
        {
            action(mock as Mock<T>);
        }
        else
        {
            throw new Exception("should mock service first.");
        }
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetService<ISender>();

        return await mediator.Send(request);
    }

    #region EFCore Manipulation

    public static async Task<IList<TEntity>> QueryAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var type = predicate.Parameters.FirstOrDefault()?.Type;

        if (type == null) throw new("QueryAsync 參數型別有誤");

        var ecDBContext = scope.ServiceProvider.GetService<ECDBContext>();
        var dbEntityType = ecDBContext.Model.FindEntityType(type);

        if (dbEntityType != null)
        {
            return await ecDBContext.Set<TEntity>().Where(predicate).ToListAsync();
        }
        else
        {
            throw new Exception($"EntityTypeNotFound in DBContext, Name:{type.Name}");
        }
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var type = entity.GetType();

        var ecDBContext = scope.ServiceProvider.GetService<ECDBContext>();
        var dbEntityType = ecDBContext.Model.FindEntityType(type);

        if (dbEntityType != null)
        {
            await ecDBContext.AddAsync(entity);
            await ecDBContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"EntityTypeNotFound in DBContext, Name:{type.Name}");
        }
    }

    public static async Task UpdateAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var type = entity.GetType();

        if (type == null) throw new("QueryAsync 參數型別有誤");

        var ecDBContext = scope.ServiceProvider.GetService<ECDBContext>();
        var dbEntityType = ecDBContext.Model.FindEntityType(type);

        if (dbEntityType != null)
        {
            ecDBContext.Update(entity);
            await ecDBContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"EntityTypeNotFound in DBContext, Name:{type.Name}");
        }
    }

    public static async Task<bool> RemoveAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var type = entity.GetType();

        var ecDBContext = scope.ServiceProvider.GetService<ECDBContext>();
        var dbEntityType = ecDBContext.Model.FindEntityType(type);

        if (dbEntityType != null)
        {
            ecDBContext.Remove(entity);
            await ecDBContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"EntityTypeNotFound in DBContext, Name:{type.Name}");
        }

        return true;
    }

    public static async Task AddRangeAsync<T>(List<T> entities)
        where T : class
    {
        if (entities.Any() == false) return;

        using var scope = _scopeFactory.CreateScope();

        var entity = entities.FirstOrDefault();

        var ecDbContext = scope.ServiceProvider.GetService<ECDBContext>();
        var type = entity.GetType();

        if (entities.All(a => a.GetType().Name == entities.GetType().Name) == false)
            throw new ArgumentException("AddRange參數類別必須相同");

        var bridgeDBEntityType = ecDbContext.Model.FindEntityType(type);

        if (bridgeDBEntityType != null)
        {
            await ecDbContext.AddRangeAsync(entity);
            await ecDbContext.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"EntityTypeNotFound in DBContext, Name:{type.Name}");
        }
    }

    #endregion
}