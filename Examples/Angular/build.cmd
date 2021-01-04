REM ==========================================
REM ||    ServiceFromAspNetCoreAnnotation    ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreAnnotation
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\models
rmdir /S/Q ClientApp\src\app\services
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM 
REM ==========================================
REM ||    ServiceFromAspNetCoreSignalRHub    ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreSignalRHub
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\models
rmdir /S/Q ClientApp\src\app\services
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM 
REM ==========================================
REM ||          ModelFromAssembly           ||
REM ==========================================
REM 
cd ModelFromAssembly
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM 
REM ==========================================
REM ||  ServiceFromAspNetCoreViaFluentApi   ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreViaFluentApi
rmdir /S/Q bin
rmdir /S/Q Service\ClientApp\src\app\models
rmdir /S/Q Service\ClientApp\src\app\services
cd Generator
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
cd ..\..
dotnet build ServiceFromAspNetCoreViaFluentApi.sln --no-incremental

REM 
REM ==========================================
REM ||    ServiceFromSignalRViaFluentApi    ||
REM ==========================================
REM 
cd ServiceFromSignalRViaFluentApi
rmdir /S/Q bin
rmdir /S/Q Service\ClientApp\src\app\models
rmdir /S/Q Service\ClientApp\src\app\services
cd Generator
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
cd ..\..
dotnet build ServiceFromSignalRViaFluentApi.sln --no-incremental
