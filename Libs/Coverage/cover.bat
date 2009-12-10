@echo off
SET CoveragePath=%CD%\Libs\Coverage
SET ReportPath=%CoveragePath%\Reports
rem %CD% = de current working dir 
%CoveragePath%\msxsl.exe %CoveragePath%\partcover-report.xml %CoveragePath%\aux-partcover-report.xsl -o %ReportPath%\modelcoverage.html
%CoveragePath%\msxsl.exe %CoveragePath%\partcover-repo-report.xml %CoveragePath%\aux-partcover-report.xsl -o %ReportPath%\repocoverage.html