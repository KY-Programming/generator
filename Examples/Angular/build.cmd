REM ==========================================
REM ||    ServiceFromAspNetCoreAnnotation    ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreAnnotation
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\models
rmdir /S/Q ClientApp\src\app\services
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
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
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
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
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..