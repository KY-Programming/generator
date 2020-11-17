REM ====================================
REM ||    ReflectionFromAttributes    ||
REM ====================================
REM 
cd ReflectionFromAttributes
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromCore       ||
REM ====================================
REM 
cd ReflectionFromCore
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionFromExecutable    ||
REM ====================================
REM 
cd ReflectionFromExecutable
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromIndex      ||
REM ====================================
REM 
cd ReflectionFromIndex
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ===========================================
REM ||    ReflectionFromMultipleAssemblies    ||
REM ===========================================
REM 
cd ReflectionFromMultipleAssemblies\MainAssembly
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..\..
REM 
REM ====================================
REM ||     ReflectionFromStandard     ||
REM ====================================
REM 
cd ReflectionFromStandard
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionIgnoreAttribute    ||
REM ====================================
REM 
cd ReflectionIgnoreAttribute
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||          ReflectionX86          ||
REM ====================================
REM 
cd ReflectionX86
del type-to-read.ts
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromNet5       ||
REM ====================================
REM 
cd ReflectionFromNet5
rmdir /S/Q Output
dotnet add package KY.Generator
dotnet add package KY.Generator.Annotations
dotnet build --no-incremental
cd ..