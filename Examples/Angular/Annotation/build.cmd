REM 
REM ==========================================
REM ||          ChangeReturnType            ||
REM ==========================================
REM 
cd ChangeReturnType
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
REM ||    ServiceFromAspNetCore    ||
REM ==========================================
REM 
cd ServiceFromAspNetCore
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
REM ||            Strict                    ||
REM ==========================================
REM 
cd Strict
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\models
rmdir /S/Q ClientApp\src\app\services
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..