REM ====================================
REM ||    ReflectionFromAttributes    ||
REM ====================================
REM 
cd ReflectionFromAttributes
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromCore       ||
REM ====================================
REM 
cd ReflectionFromCore
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionFromExecutable    ||
REM ====================================
REM 
cd ReflectionFromExecutable
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromIndex      ||
REM ====================================
REM 
cd ReflectionFromIndex
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ===========================================
REM ||    ReflectionFromMultipleAssemblies    ||
REM ===========================================
REM 
cd ReflectionFromMultipleAssemblies\MainAssembly
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..\..
REM 
REM ====================================
REM ||     ReflectionFromStandard     ||
REM ====================================
REM 
cd ReflectionFromStandard
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||    ReflectionIgnoreAttribute    ||
REM ====================================
REM 
cd ReflectionIgnoreAttribute
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||          ReflectionX86          ||
REM ====================================
REM 
cd ReflectionX86
rmdir /S/Q bin
del type-to-read.ts
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||       ReflectionFromNet5       ||
REM ====================================
REM 
cd ReflectionFromNet5
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..
REM 
REM ====================================
REM ||     ReflectionFromConstant     ||
REM ====================================
REM 
cd ReflectionFromConstant
rmdir /S/Q bin
rmdir /S/Q Output
dotnet add package KY.Generator --prerelease
dotnet add package KY.Generator.Annotations --prerelease
dotnet build --no-incremental
cd ..