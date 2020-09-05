REM ==========================================
REM ||    AnnotationsWithMultipleOutputs    ||
REM ==========================================
REM 
cd AnnotationsWithMultipleOutputs
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..

REM ==========================================
REM ||      WebApiControllerWithRoute      ||
REM ==========================================
REM 
cd WebApiControllerWithRoute
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..

PAUSE