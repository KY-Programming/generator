REM 
REM ==========================================
REM ||          ChangeReturnType            ||
REM ==========================================
REM 
cd ChangeReturnType
rmdir /S/Q bin
rmdir /S/Q \Generator\Output
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.AspDotNet --prerelease
cd ..\..
dotnet build ChangeReturnType.sln --no-incremental

REM 
REM ==========================================
REM ||              FromModel               ||
REM ==========================================
REM 
cd FromModel
rmdir /S/Q bin
rmdir /S/Q \Generator\Output
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
cd ..\..
dotnet build FromModel.sln --no-incremental

REM 
REM ==========================================
REM ||  GenerateInterfacesInsteadClasses    ||
REM ==========================================
REM 
cd GenerateInterfacesInsteadClasses
rmdir /S/Q bin
rmdir /S/Q \Generator\Output
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
cd ..\..
dotnet build GenerateInterfacesInsteadClasses.sln --no-incremental

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

REM 
REM ==========================================
REM ||        WithCustomHttpClient          ||
REM ==========================================
REM 
cd WithCustomHttpClient
rmdir /S/Q bin
rmdir /S/Q Service\ClientApp\src\app\models
rmdir /S/Q Service\ClientApp\src\app\services
cd Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.AspDotNet --prerelease
cd ..\..
dotnet build WithCustomHttpClient.sln --no-incremental
