#取得 .NET Core CLI 工具
https://learn.microsoft.com/zh-tw/ef/core/get-started/overview/install#get-the-entity-framework-core-tools

#install dotnet-ef
dotnet tool install --global dotnet-ef

nuget require
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.SqlServer

CD /Users/paul_huang/Dev/MAYO-Form/src/MAYO-Form/MAYO-Form.Infrastructure
#.net 6 up (要用單引號，不然 !需要escape)

//all sync
//share standard db
dotnet ef dbcontext scaffold 'Data Source=127.0.0.1;Initial Catalog=tstMAYOFormSharedDB_Test;User Id=sa;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=true;Connection Timeout=300;' Microsoft.EntityFrameworkCore.SqlServer --context "MAYOFormStandardDbContext" --context-dir "Persistence" --output-dir "../MAYO-Form.Domain/Standard/Entities" --namespace "MAYO_Form.Domain.Standard.Entities" --context-namespace MAYO_Form.Infrastructure.Persistence --force

//tenant db 
dotnet ef dbcontext scaffold 'Data Source=127.0.0.1;Initial Catalog=tstMAYOFormTenantDB_Test;User Id=sa;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=true;Connection Timeout=300;' Microsoft.EntityFrameworkCore.SqlServer --context "MAYOFormTenantManagerDbContext" --context-dir "Persistence" --output-dir "../MAYO-Form.Domain/TenantManager/Entities" --namespace "MAYO_Form.Domain.TenantManager.Entities" --context-namespace MAYO_Form.Infrastructure.Persistence --force


//指定Table範例

dotnet ef dbcontext scaffold 'Data Source=127.0.0.1;Initial Catalog=tstMAYOFormSharedDB_Test;User Id=sa;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=true;Connection Timeout=300;' Microsoft.EntityFrameworkCore.SqlServer --context "MAYOFormDBContext" --context-dir "Persistence" --output-dir "../MAYO-Form.Domain/Standard/Entities" --namespace "MAYO_Form.Domain.Standard.Entities" --context-namespace MAYO_Form.Infrastructure.Persistence --force -t FormDesignObj

dotnet ef dbcontext scaffold 'Data Source=127.0.0.1;Initial Catalog=tstMAYOFormTenantDB_Test;User Id=sa;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=true;Connection Timeout=300;' Microsoft.EntityFrameworkCore.SqlServer --context "MAYOFormTenantManagerDbContext" --context-dir "Persistence" --output-dir "../MAYO-Form.Domain/TenantManager/Entities" --namespace "MAYO_Form.Domain.TenantManager.Entities" --context-namespace MAYO_Form.Infrastructure.Persistence --force -t Roles -t Functions -t FunctionPermissions -t RoleFunctions


//同步進Context後，要移除 以下override的function
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=tstMAYOFormSharedDB;User Id=sa;Password=1qaz@WSX3edc;Encrypt=True;TrustServerCertificate=true;Connection Timeout=300;");
