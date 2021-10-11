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
REM ||              Caseing                 ||
REM ==========================================
REM 
cd Caseing.Generator
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
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
rmdir /S/Q ClientApp\src\app\convert-to-interface\models
rmdir /S/Q ClientApp\src\app\convert-to-interface\services
rmdir /S/Q ClientApp\src\app\date\models
rmdir /S/Q ClientApp\src\app\date\services
rmdir /S/Q ClientApp\src\app\derived\models
rmdir /S/Q ClientApp\src\app\derived\services
rmdir /S/Q ClientApp\src\app\duplicate-name\models
rmdir /S/Q ClientApp\src\app\duplicate-name\services
rmdir /S/Q ClientApp\src\app\edge-cases\models
rmdir /S/Q ClientApp\src\app\edge-cases\services
rmdir /S/Q ClientApp\src\app\fix-casing\models
rmdir /S/Q ClientApp\src\app\fix-casing\services
rmdir /S/Q ClientApp\src\app\get-complex\models
rmdir /S/Q ClientApp\src\app\get-complex\services
rmdir /S/Q ClientApp\src\app\keep-casing\models
rmdir /S/Q ClientApp\src\app\keep-casing\services
rmdir /S/Q ClientApp\src\app\parameter-on-controller\models
rmdir /S/Q ClientApp\src\app\parameter-on-controller\services
rmdir /S/Q ClientApp\src\app\post\models
rmdir /S/Q ClientApp\src\app\post\services
rmdir /S/Q ClientApp\src\app\produces\models
rmdir /S/Q ClientApp\src\app\produces\services
rmdir /S/Q ClientApp\src\app\rename\models
rmdir /S/Q ClientApp\src\app\rename\services
rmdir /S/Q ClientApp\src\app\routed\models
rmdir /S/Q ClientApp\src\app\routed\services
rmdir /S/Q ClientApp\src\app\versioned-api\models
rmdir /S/Q ClientApp\src\app\versioned-api\services
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||    UrlVersionedWebApiController      ||
REM ==========================================
REM 
cd UrlVersionedWebApiController
rmdir /S/Q bin
rmdir /S/Q ClientApp\src\app\models
rmdir /S/Q ClientApp\src\app\services
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

REM ==========================================
REM ||             Formatting                ||
REM ==========================================
REM 
cd Formatting\Generator
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.Reflection --prerelease
dotnet build --no-incremental
cd ..\..

REM ==========================================
REM ||           WebApiFluent               ||
REM ==========================================
REM 
rmdir /S/Q WebApiFluent.Generator\bin
rmdir /S/Q WebApiFluent\ClientApp\src\app\models
rmdir /S/Q WebApiFluent\ClientApp\src\app\services
cd WebApiFluent.Generator
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Angular --prerelease
dotnet add package KY.Generator.AspDotNet --prerelease
dotnet build --no-incremental
cd ..