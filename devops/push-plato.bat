call :subroutine "%~dp0\..\plato" %1
exit /b

rem <Directory> <message>
:subroutine
pushd %1 
git add .
git commit -m %2
git push
popd
exit /b