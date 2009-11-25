@echo off

:: zet environment vars zodat we bij msbuild kunnen
call "%VS80COMNTOOLS%\vsvars32.bat"
cls

@title Generating coverage reports...
echo Generating coverage reports...

msbuild default.build /t:GenerateCoverage /p:Configuration=Release > GenerateCoverageReports.log

echo.
echo Finished.
echo.
echo Model coverage:      \Libs\Coverage\Reports\repocoverage.html
echo Repository coverage: \Libs\Coverage\Reports\modelcoverage.html
echo.
pause