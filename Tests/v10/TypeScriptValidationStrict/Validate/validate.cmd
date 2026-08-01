@echo off
rem Type-checks the v10 output files that were generated with GenerateStrict against a strict tsconfig.
rem The file list is derived from the C# sources by Shared/Scripts/build-file-list.js - see that script.
rem Reports the result as JSON on the last line of stdout - see Shared/Scripts/validate-typescript.js.

setlocal
set "PROJECT=%~dp0.."

if exist "%PROJECT%\Output" rmdir /S /Q "%PROJECT%\Output"

call "%PROJECT%\..\Shared\Scripts\ensure-packages.cmd"
if not "%ERRORLEVEL%"=="0" exit /b %ERRORLEVEL%

pushd "%PROJECT%"
call node "..\Shared\Scripts\validate-typescript.js" --strict
set "RESULT=%ERRORLEVEL%"
popd

exit /b %RESULT%
