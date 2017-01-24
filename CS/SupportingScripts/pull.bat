@echo off
echo *** Git Sync info: %DATE% %TIME% >C:\GIT\Telerik\CS\SupportingScripts\pull.log
cd C:\GIT\Telerik
git reset --hard HEAD >>C:\GIT\Telerik\CS\SupportingScripts\pull.log
git pull >>C:\GIT\Telerik\CS\SupportingScripts\pull.log
