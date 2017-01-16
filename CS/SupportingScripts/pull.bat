@echo off
cd c:\GIT\Telerik
echo %DATE% %TIME% >pull.log
git pull >>pull.log
