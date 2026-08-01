@echo off
rem Validates GenerateNever. Unlike every other v10 project this one must NOT build:
rem the generator has to abort with an error that names the file the forbidden type
rem would have been written to.
rem
rem Reports the result as JSON on the last line of stdout - the Builder reads that, not the exit code:
rem   {"state":"passed","errors":0,"validated":1}
rem "validated" is the one output file this project guards - never-generated-model.ts.

setlocal
set "PROJECT=%~dp0.."
set "LOG=%TEMP%\ky-generator-never-annotations.log"

if exist "%PROJECT%\Output" rmdir /S /Q "%PROJECT%\Output"

dotnet build "%PROJECT%\NeverAnnotations.csproj" --no-incremental > "%LOG%" 2>&1
set "BUILD=%ERRORLEVEL%"
type "%LOG%"

if "%BUILD%"=="0" (
    echo VALIDATION FAILED: the build succeeded, but GenerateNever should have aborted it.
    echo {"state":"failed","errors":1,"validated":0}
    exit /b 1
)

findstr /C:"is decorated with GenerateNeverAttribute and must never be generated" "%LOG%" > nul
if errorlevel 1 (
    echo VALIDATION FAILED: the build failed, but not with the GenerateNever error.
    echo {"state":"failed","errors":1,"validated":0}
    exit /b 1
)

findstr /C:"Output\never-generated-model.ts" "%LOG%" > nul
if errorlevel 1 (
    echo VALIDATION FAILED: the GenerateNever error does not name the generated file.
    echo {"state":"failed","errors":1,"validated":0}
    exit /b 1
)

if exist "%PROJECT%\Output\never-generated-model.ts" (
    echo VALIDATION FAILED: the forbidden file was written anyway.
    echo {"state":"failed","errors":1,"validated":0}
    exit /b 1
)

echo VALIDATION PASSED
echo {"state":"passed","errors":0,"validated":1}
exit /b 0
