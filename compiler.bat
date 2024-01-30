echo Compiling at path %cd%
set root=%cd%
set configType=%1
set outputDir=%2
set targetDir=%3
set resultFile=%4
set rar="C:\Program Files\WinRAR\winrar.exe"
set package="package"
set release="release"

echo configType %configType%
echo outputDir %outputDir%
echo targetDir %targetDir%
echo resultFile %resultFile%

:: Ignore if debug mode
IF "$(Configuration)" == "Debug" (
    echo Skipping release build
    exit 0
    )

echo Output at %outputPath%
echo Target at %targetPath%

:: Create release folder
echo Creating %release% at %outputDir%
cd %outputDir%
IF EXIST "%release%" (
    echo Deleting old %release%
    rmdir "%release%" /S /Q
)

mkdir "%release%"
cd "%release%"
set output=%cd%
cd %root%

:: Copy package to release
echo Copying %package% to %output%
IF EXIST "%package%" (
    copy "%package%" "%output%" /Y
)

:: Add executable to package
echo Copying %resultFile% to %output%
copy %resultFile% "%output%" /Y

:: Custom additions to package
echo Adding extra files to %output%
call:clone "CHANGELOG.md" "%output%"
call:clone "README.md" "%output%"
call:clone "manifest.json" "%output%"

:: Compress folder using winrar
cd %outputDir%
cd "%release%"
echo Creating zip release at %cd%
%rar% a -afzip -m5 release * 
cd %root%

exit

:: Functions

:clone
IF EXIST %1 (
    echo Cloning %1 to %2
    copy %1 %2 /Y
    )
exit /B %ERRORLEVEL%