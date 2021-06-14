REM ==========================================
REM ||           JsonWithWriter             ||
REM ==========================================
REM 
cd JsonWithWriter
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Json --prerelease
dotnet build --no-incremental
cd ..
