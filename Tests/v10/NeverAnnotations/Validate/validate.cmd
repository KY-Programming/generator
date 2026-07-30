@echo off
rem Validates GenerateNever. Unlike every other v10 project this one must NOT build:
rem the generator has to abort with an error that names the file the forbidden type
rem would have been written to.
rem Exit code 200 = validation passed, anything else = validation failed.

setlocal
set "PROJECT=%~dp0.."
set "LOG=%TEMP%\ky-generator-never-annotations.log"

if exist "%PROJECT%\Output" rmdir /S /Q "%PROJECT%\Output"

dotnet build "%PROJECT%\NeverAnnotations.csproj" --no-incremental > "%LOG%" 2>&1
set "BUILD=%ERRORLEVEL%"
type "%LOG%"

if "%BUILD%"=="0" (
    echo VALIDATION FAILED: the build succeeded, but GenerateNever should have aborted it.
    exit /b 1
)

findstr /C:"is decorated with GenerateNeverAttribute and must never be generated" "%LOG%" > nul
if errorlevel 1 (
    echo VALIDATION FAILED: the build failed, but not with the GenerateNever error.
    exit /b 2
)

findstr /C:"Output\never-generated-model.ts" "%LOG%" > nul
if errorlevel 1 (
    echo VALIDATION FAILED: the GenerateNever error does not name the generated file.
    exit /b 3
)

if exist "%PROJECT%\Output\never-generated-model.ts" (
    echo VALIDATION FAILED: the forbidden file was written anyway.
    exit /b 4
)

echo VALIDATION PASSED
exit /b 200
