:: This script pushes Aventura build files to their appropriate folders as if it was installed.  This should be placed in \Branch\PushBuildFiles.bat
:: Usage: PushBuildFiles [SourcePath] [DestinationFolder] [Configuration] [Platform]
:: 		ex. PushBuildFiles.bat  "C:\Source\trunk\BuildConfigurations\CompleteDotNetSolution\Debug\AVCT.exe" AVCT Release x64
:: Result: The AVCT executable will be located at: C:\Build\trunk\Release\AVCT\avct.exe

@echo off

:: Display Help
if "%1"=="" (
	goto:ShowHelp
)

:: Verify number of arguements
if %4=="" (
	echo Error: Too few arguements present!
	echo.
	goto:ShowHelp
)

:: Start the deploy
SET SourcePath=%1
SET TargetFolder=%2
SET Configuration=%3
SET Platform=%4

:: Get the current branch
set LaunchDir=%~dp0
set LaunchDirTrunk=%LaunchDir:~0,-1%
for %%f in (%LaunchDirTrunk%) do set Branch=%%~nxf

:: Create current branch build folder
set "TargetPath=%~dp0..\..\Build\%Branch%\%Configuration%\%TargetFolder%"

if not exist %TargetPath% (mkdir %TargetPath%)

:: Copy the file
copy /b /y %SourcePath% %TargetPath%

if %errorlevel%==0 (
	echo Destination: %TargetPath%
)
if %errorlevel%==1 (
	echo Failed to copy file
)
goto:eof

:ShowHelp
echo.
echo This script pushes Aventura build files to their appropriate folders as if it was installed.  This should be placed in \Branch\PushBuildFiles.bat
echo.
echo PushBuildFiles [SourcePath] [DestinationFolder] [Configuration] [Platform]
echo.
echo SourcePath: Specifies the file to be copied
echo.
echo Example: PushBuildFiles.bat  "C:\Source\trunk\BuildConfigurations\CompleteDotNetSolution\Debug\AVCT.exe" AVCT Release x64
echo Result: The AVCT executable will be located at: C:\Build\trunk\Release\AVCT\avct.exe
goto:eof
