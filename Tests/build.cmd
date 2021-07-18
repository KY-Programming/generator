

REM ==========================================
REM ||                Sqlite                ||
REM ==========================================
REM 
cd Sqlite
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..\..

PAUSE