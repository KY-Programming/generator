@echo off
rem Installs the packages the generated Angular output imports, so the TypeScriptValidation* projects can
rem type-check it against the real @angular/* and rxjs types instead of skipping those files.
rem npm ci is used, so the committed package-lock.json decides the versions - not whatever is newest.

setlocal
set "V10=%~dp0..\.."

if exist "%V10%\node_modules" exit /b 0

echo Installing the v10 validation packages...
pushd "%V10%"
call npm ci --no-audit --no-fund
set "RESULT=%ERRORLEVEL%"
popd

if not "%RESULT%"=="0" echo INSTALL FAILED: npm ci exited with %RESULT%.
exit /b %RESULT%
