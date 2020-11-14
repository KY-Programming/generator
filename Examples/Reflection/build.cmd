REM ====================================
REM ||    ReflectionFromAttributes    ||
REM ====================================
REM 
cd ReflectionFromAttributes
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromCore       ||
REM ====================================
REM 
cd ReflectionFromCore
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionFromExecutable    ||
REM ====================================
REM 
REM TODO
REM cd ReflectionFromExecutable
REM dotnet add package KY.Generator
REM dotnet build --no-incremental
REM cd ..
REM 
REM ====================================
REM ||       ReflectionFromIndex      ||
REM ====================================
REM 
cd ReflectionFromIndex
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ===========================================
REM ||    ReflectionFromMultipleAssemblies    ||
REM ===========================================
REM 
cd ReflectionFromMultipleAssemblies\MainAssembly
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..\..
REM 
REM ====================================
REM ||     ReflectionFromStandard     ||
REM ====================================
REM 
cd ReflectionFromStandard
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionIgnoreAttribute    ||
REM ====================================
REM 
cd ReflectionIgnoreAttribute
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||          ReflectionX86          ||
REM ====================================
REM 
cd ReflectionX86
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromNet5       ||
REM ====================================
REM 
cd ReflectionFromNet5
dotnet add package KY.Generator
dotnet add package KY.Generator.Reflection.Annotations
dotnet build --no-incremental
cd ..

PAUSE