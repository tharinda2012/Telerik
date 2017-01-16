@echo off

echo %DATE% %TIME% >C:\GIT\Telerik\CS\SupportingScripts\pull.log
cd C:\GIT\Telerik
git pull >>C:\GIT\Telerik\CS\SupportingScripts\pull.log
