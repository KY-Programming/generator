REM ==========================================
REM ||  SelfReferencingAnnotationsNullable  ||
REM ==========================================
REM 
cd SelfReferencingAnnotationsNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||     StrictAnnotationsNotNullable     ||
REM ==========================================
REM 
cd StrictAnnotationsNotNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||      StrictAnnotationsNullable       ||
REM ==========================================
REM 
cd StrictAnnotationsNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||     TypesAnnotationsNotNullable      ||
REM ==========================================
REM 
cd TypesAnnotationsNotNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||       TypesAnnotationsNullable       ||
REM ==========================================
REM 
cd TypesAnnotationsNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||        TypesFluentNotNullable        ||
REM ==========================================
REM 
cd TypesFluentNotNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||          TypesFluentNullable         ||
REM ==========================================
REM 
cd TypesFluentNullable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..

