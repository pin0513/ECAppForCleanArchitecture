using System.Text;
using Dapper;
using ECApp.Domain.Entities;
using ECApp.Infrastructure.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ECApp.Infrastructure.Helpers
{
    public class UpdateSchemaHelper
    {
        public static void SyncSchema(ECDBContext dbContext, string dbConnectionString,
            string updateDbFolderName)
        {
            var dbName = new SqlConnectionStringBuilder(dbConnectionString).InitialCatalog;

            if (dbName is null or "master")
            {
                throw new Exception("should specific a db in connection string");
            }

            var migrationFolderInfos =
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.Parent?.Parent?.Parent?.GetDirectories(
                    "Migration");

            if (migrationFolderInfos != null && migrationFolderInfos.Any())
            {
                var migrateFolder = migrationFolderInfos.FirstOrDefault();

                CheckSchemaVersion(dbConnectionString, migrateFolder);

                var versionData = dbContext.SchemaVersions.Where(a => a.DeletedTime.HasValue == false)
                    .ToList();

                List<System.Version> versionSort = new List<System.Version>();

                var versionFolders = migrateFolder.GetDirectories("DBSchema").FirstOrDefault().GetDirectories("V*.*.*");

                foreach (var folder in versionFolders)
                {
                    var valid = System.Version.TryParse(folder.Name.TrimStart('V'), out var versionObj);

                    if (valid)
                        versionSort.Add(versionObj);
                }

                versionSort = versionSort.OrderBy(a => a).ToList();

                foreach (var version in versionSort)
                {
                    StringBuilder sb = new StringBuilder();
                    var isVersionOk = true;
                    var sortedVersionFolder = versionFolders.FirstOrDefault(a => a.Name == "V" + version.ToString());

                    if (sortedVersionFolder != null)
                    {
                        var subDBFolders = sortedVersionFolder.GetDirectories(updateDbFolderName);

                        if (subDBFolders.Any())
                        {
                            var subDBFolder = subDBFolders.FirstOrDefault();

                            var addOrExistVersionData = versionData.FirstOrDefault(a =>
                                a.PartitionKey == updateDbFolderName + "_" + sortedVersionFolder.Name + "_" + dbName);

                            if (addOrExistVersionData != null && addOrExistVersionData.RawKey == "VersionUpgradeOk")
                            {
                                addOrExistVersionData.LatestUpdatedTime = DateTimeOffset.UtcNow;

                                continue;
                            }

                            var allScripts = subDBFolder.GetFiles("*.sql");

                            if (allScripts.Any())
                            {
                                allScripts = allScripts.OrderBy(f => f.Name).ToArray();

                                foreach (var scrip in allScripts)
                                {
                                    using var conn = new SqlConnection(dbConnectionString);

                                    try
                                    {
                                        using var file = File.OpenText(scrip.FullName);
                                        conn.Execute(file.ReadToEnd());
                                    }
                                    catch (Exception e)
                                    {
                                        sb.AppendLine($"file:{scrip.Name}, errorMsg:{e.Message}");
                                        isVersionOk = false;
                                    }
                                }
                            }

                            if (addOrExistVersionData != null)
                            {
                                if (isVersionOk)
                                {
                                    addOrExistVersionData.RawKey = "VersionUpgradeOk";

                                    addOrExistVersionData.Data =
                                        "execute ok, past error log \n " + addOrExistVersionData.Data;
                                }
                                else
                                {
                                    addOrExistVersionData.RawKey = "VersionUpgradeFailed";

                                    addOrExistVersionData.Data =
                                        $"execute error , current error: {sb}, past error log \n " +
                                        addOrExistVersionData.Data;
                                }

                                dbContext.Update(addOrExistVersionData);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                if (isVersionOk)
                                {
                                    addOrExistVersionData = new SchemaVersion()
                                    {
                                        Id = dbContext.NewId().GetAwaiter().GetResult(),
                                        PartitionKey = updateDbFolderName + "_" + sortedVersionFolder.Name + "_" +
                                                       dbName,
                                        RawKey = "VersionUpgradeOk"
                                    };
                                }
                                else
                                {
                                    addOrExistVersionData = new SchemaVersion()
                                    {
                                        Id = dbContext.NewId().GetAwaiter().GetResult(),
                                        PartitionKey = updateDbFolderName + "_" + sortedVersionFolder.Name + "_" +
                                                       dbName,
                                        RawKey = "VersionUpgradeFailed", Data = sb.ToString()
                                    };
                                }

                                dbContext.SchemaVersions.Add(addOrExistVersionData);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

        private static void CheckSchemaVersion(string dbConnectionString, DirectoryInfo? migrateFolder)
        {
            var schemaTraceVersion =
                migrateFolder.GetDirectories("DBSchema").FirstOrDefault().GetDirectories("DBVersion");

            if (schemaTraceVersion.Any())
            {
                foreach (var fileScirpt in schemaTraceVersion.FirstOrDefault().GetFiles("*.sql"))
                {
                    try
                    {
                        using SqlConnection conn =
                            new SqlConnection(dbConnectionString);

                        using var file = File.OpenText(fileScirpt.FullName);
                        conn.Execute(file.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        throw new Exception(fileScirpt.FullName + " handling error" + e);
                    }
                }
            }
        }
    }
}