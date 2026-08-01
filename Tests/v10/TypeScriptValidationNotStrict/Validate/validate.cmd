@echo off
rem Type-checks every v10 output file against a non strict tsconfig - strict output is valid here too, so
rem this list is a superset of the strict one. Built by Shared/Scripts/build-file-list.js - see that script.
rem Reports the result as JSON on the last line of stdout - see Shared/Scripts/validate-typescript.js.

setlocal
set "PROJECT=%~dp0.."

if exist "%PROJECT%\Output" rmdir /S /Q "%PROJECT%\Output"

call "%PROJECT%\..\Shared\Scripts\ensure-packages.cmd"
if not "%ERRORLEVEL%"=="0" exit /b %ERRORLEVEL%

pushd "%PROJECT%"
call node "..\Shared\Scripts\validate-typescript.js" --all
set "RESULT=%ERRORLEVEL%"
popd

exit /b %RESULT%
