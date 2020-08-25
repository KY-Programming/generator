REM ==========================================
REM ||    ServiceFromAspNetCoreAnnotation    ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreAnnotation\ServiceFromAspNetCoreAnnotation
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..\..
REM 
REM ==========================================
REM ||    ServiceFromAspNetCoreSignalRHub    ||
REM ==========================================
REM 
cd ServiceFromAspNetCoreSignalRHub\ServiceFromAspNetCoreSignalRHub
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..\..