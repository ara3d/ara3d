echo off
call :subroutine "%~dp0\..\plato" %1
git status
git add .
git commit -m %1
git push
exit /b

rem <Directory> <message>
:subroutine
pushd %1 
git status
git add .
git commit -m %2
git push
popd
exit /b
