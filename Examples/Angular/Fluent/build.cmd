REM 
REM ==========================================
REM ||        ServiceFromAspNetCore         ||
REM ==========================================
REM 
cd ServiceFromAspNetCore
rmdir /S/Q bin
rmdir /S/Q Service\ClientApp\src\app\models
rmdir /S/Q Service\ClientApp\src\app\services
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.AspDotNet --prerelease
cd ..\..
dotnet build ServiceFromAspNetCore.sln --no-incremental

REM 
REM ==========================================
REM ||         ServiceFromSignalR           ||
REM ==========================================
REM 
cd ServiceFromSignalR
rmdir /S/Q bin
rmdir /S/Q Service\ClientApp\src\app\models
rmdir /S/Q Service\ClientApp\src\app\services
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.AspDotNet --prerelease
cd ..\..
dotnet build ServiceFromSignalR.sln --no-incremental
