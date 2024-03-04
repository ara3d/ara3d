echo off
if "%~1" == "" (echo "No message provided" && exit /b)
call :subroutine "%~dp0\..\domo" %1 %2
call :subroutine "%~dp0\..\plato" %1 %2 
call :subroutine "%~dp0\..\parakeet" %1 %2
call :subroutine "%~dp0\..\bowerbird" %1 %2
call :subroutine "%~dp0\.." %1 %2
call "%~dp0\nuget-init.bat"
exit /b

rem <Directory> <message>
:subroutine
pushd %1
echo %cd% 
git status
git add .
git status
git commit -m %2
git push
git status
rem If a version is provided, then make a tag. 
if not "%3"=="" (
	git tag -a v%3 -m "Version %3 created %~2"
	git push origin --tags
)
popd
exit /b
