@title KY-Generator Tests

REM ==========================================
REM ||    AnnotationsWithMultipleOutputs    ||
REM ==========================================
REM 
cd AnnotationsWithMultipleOutputs
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||               Types                 ||
REM ==========================================
REM 
cd Types
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||           WebApiController           ||
REM ==========================================
REM 
cd WebApiController
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\fix-casing\models
rmdir /S/Q ClientApp\src\app\fix-casing\services
rmdir /S/Q ClientApp\src\app\keep-casing\models
rmdir /S/Q ClientApp\src\app\keep-casing\services
rmdir /S/Q ClientApp\src\app\produces\models
rmdir /S/Q ClientApp\src\app\produces\services
rmdir /S/Q ClientApp\src\app\routed\models
rmdir /S/Q ClientApp\src\app\routed\services
rmdir /S/Q ClientApp\src\app\versioned-api\models
rmdir /S/Q ClientApp\src\app\versioned-api\services
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||       AnnotationInNestedClass        ||
REM ==========================================
REM 
cd AnnotationInNestedClass
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||           AnnotationAsync           ||
REM ==========================================
REM 
cd AnnotationAsync
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||       AnnotationAsyncAssembly        ||
REM ==========================================
REM 
cd AnnotationAsyncAssembly
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||              Fluent                  ||
REM ==========================================
REM 
cd Fluent\Fluent.Generator
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
dotnet build --no-incremental
cd ..\..

REM ==========================================
REM ||  ReflectionLoadFromNugetPackageNet5  ||
REM ==========================================
REM 
cd ReflectionLoadFromNugetPackageNet5
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

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
cd ..

PAUSE