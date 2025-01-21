ECDB
IntegrationTest Require 
local DB

Example Docker Command: 

```dockerfile
docker run \
--name "/festive_kirch" \
--runtime "runc" \
--log-driver "json-file" \
--restart "no" \
--cap-add "SYS_PTRACE" \
--publish "0.0.0.0:1433:1433/tcp" \
--network "bridge" \
--hostname "ce6067cab6e1" \
--expose "1401/tcp" \
--expose "1433/tcp" \
--env "LD_LIBRARY_PATH=/opt/mssql/lib" \
--env "ACCEPT_EULA=1" \
--env "MSSQL_SA_PASSWORD=1qaz@WSX3edc" \
--env "PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin" \
--env "MSSQL_RPC_PORT=135" \
--env "CONFIG_EDGE_BUILD=1" \
--env "PAL_BOOT_WITH_MINIMAL_CONFIG=1" \
--env "PAL_ENABLE_PAGE_ALIGNED_PE_FILE_CREATION=1" \
--label "com.azure.dev.image.build.sourceversion"="ce89b6c967e193696164e69929c3341c38ba9c7e" \
--label "com.azure.dev.image.system.teamfoundationcollectionuri"="https://dev.azure.com/tigerdid/" \
--label "com.microsoft.product"="Microsoft SQL Server" \
--label "com.microsoft.version"="15.0.2000.155916" \
--label "org.opencontainers.image.ref.name"="ubuntu" \
--label "org.opencontainers.image.version"="18.04" \
--label "vendor"="Microsoft" \
--detach \
--entrypoint "/opt/mssql/bin/permissions_check.sh" \
"mcr.microsoft.com/azure-sql-edge" \
"/bin/sh" "-c" "/opt/mssql/bin/launchpadd -usens=false -enableOutboundAccess=true -usesameuser=true -sqlGroup root -- -reparentOrphanedDescendants=true -useDefaultLaunchers=false & /app/asdepackage/AsdePackage & /opt/mssql/bin/sqlservr" 
```