REM 
REM ==========================================
REM ||          FromDatabase           ||
REM ==========================================
REM 
cd FromDatabase
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Sqlite --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||    ToDatabase    ||
REM ==========================================
REM 
cd ToDatabase
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..