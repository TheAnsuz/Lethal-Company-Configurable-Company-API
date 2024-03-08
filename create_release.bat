@echo off

cd %~dp0
rmdir /S /Q Release
call Resources\ProjectBuilder.exe /V Resources\BuildScript.pbs
pause