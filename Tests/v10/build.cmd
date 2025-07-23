REM ===============================================
REM ||      EnumAnnotationsNullableDisabled      ||
REM ===============================================
REM 
cd EnumAnnotationsNullableDisabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||  InheritanceAnnotationsNullableDisabled   ||
REM ===============================================
REM 
cd InheritanceAnnotationsNullableDisabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||  SelfReferencingAnnotationsNullableEnabled  ||
REM ===============================================
REM 
cd SelfReferencingAnnotationsNullableEnabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||     StrictAnnotationsNullableDisabled     ||
REM ===============================================
REM 
cd StrictAnnotationsNullableDisabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||      StrictAnnotationsNullableEnabled       ||
REM ===============================================
REM 
cd StrictAnnotationsNullableEnabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||     TypesAnnotationsNullableDisabled      ||
REM ===============================================
REM 
cd TypesAnnotationsNullableDisabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||     TypesAnnotationsNullableEnabled       ||
REM ===============================================
REM 
cd TypesAnnotationsNullableEnabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||        TypesFluentNullableDisabled        ||
REM ===============================================
REM 
cd TypesFluentNullableDisabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Reflection --prerelease
dotnet build --no-incremental
cd ..

REM ===============================================
REM ||        TypesFluentNullableEnabled         ||
REM ===============================================
REM 
cd TypesFluentNullableEnabled
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet add package KY.Generator.Fluent --prerelease
dotnet add package KY.Generator.Reflection --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||             VALIDATE                 ||
REM ==========================================
REM
cd TypeScriptValidationNotStrict
REM start validate
cd ..

REM ==========================================
REM ||         VALIDATE STRICT             ||
REM ==========================================
REM
cd TypeScriptValidationStrict
start validate
cd ..
