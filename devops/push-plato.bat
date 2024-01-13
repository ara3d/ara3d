echo off
call :subroutine "%~dp0\..\plato" %1
git status
git add .
git status
git commit -m %1
git push
git status
exit /b

rem <Directory> <message>
:subroutine
pushd %1 
git status
git add .
git status
git commit -m %2
git push
git status
popd
exit /b
