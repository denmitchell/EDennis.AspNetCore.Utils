REM Run this from PowerShell or CMD with ./BuildSftpContainer.bat  
@echo off

REM Logon to Docker
set /p user="Enter Docker User ID: "
docker login -u %user%

REM Get the latest image for the most popular SFTP server
docker pull atmoz/sftp:latest

REM Stop and Remove the existing server container, if present
docker stop sftp_server
docker rm sftp_server

REM Build a new container for the SFTP server
docker run -d -p 22:22 --name sftp_server atmoz/sftp:latest testuser:testpass:::upload