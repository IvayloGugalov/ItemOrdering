@echo OFF
set MyProcess=Docker Desktop.exe
echo "%MyProcess%"
TASKLIST | FINDSTR "%MyProcess%"
if %ERRORLEVEL% EQU 1 (
    goto :StartDocker
) else (
    echo "%MyProcess%" is running
	docker start mongofortest
	exit
)

:StartDocker
echo Starting Docker exe
start " " "C:\Program Files\Docker\Docker\Docker Desktop.exe"
ping 127.0.0.1 -n 16 > nul
docker start mongofortest
exit