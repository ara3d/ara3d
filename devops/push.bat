echo off
call :subroutine "%~dp0\..\domo" %1
call :subroutine "%~dp0\..\plato" %1
call :subroutine "%~dp0\..\parakeet" %1
call :subroutine "%~dp0\.." %1
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
popd
exit /b
