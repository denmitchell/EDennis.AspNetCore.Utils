#This script will build an SFTP server container for
#for use in integration tests.

#Importantly, you need to have Docker for Windows installed 
#on your development machine, and you need a Docker user 
#account.

#Run this from PowerShell or CMD with ./BuildSftpContainer.ps1  

#NOTE: You may have to restart Docker service upon reboot of Win10 machine

#Logon to Docker.  The Docker CLI will prompt you for a password
$user = Read-Host -Prompt 'Enter Docker User ID'

Start-Process -FilePath "docker" -ArgumentList "login -u $($user)" -NoNewWindow -Wait

$image = "rastasheep/ubuntu-sshd" #"atmoz/sftp:latest"
$container = "sftp_server"
#$userPassFolder = "testuser:testpass" #:::upload

#Get the latest image for the most popular SFTP server image
Start-Process -FilePath "docker" -ArgumentList "pull $($image)" -NoNewWindow -Wait

#Stop and Remove the existing server container, if present
Start-Process -FilePath "docker" -ArgumentList "stop $($container)" -NoNewWindow -Wait
Start-Process -FilePath "docker" -ArgumentList "rm $($container)" -NoNewWindow -Wait

#Build a new container for the SFTP server
#Set the port = 22 (there must not be any conflicts)
#Setup a user with username = testuser, password = testpass
Start-Process -FilePath "docker" -ArgumentList "run -d -p 22:22 --name $($container) $($image)" # $($userPassFolder)" -NoNewWindow -Wait
