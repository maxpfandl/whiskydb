REM rmdir /q /s \\192.168.0.15\docker\rssfeed\bin\
REM dotnet publish -c Release -o \\192.168.0.15\docker\rssfeed\bin\
REM c:\Data\publishfolder\


wsl rsync -a -v ttrss:/var/dotnet/whiskydb/wwwroot/images/* /mnt/d/Documents/Development/whiskydb/DataBackup/images
wsl rsync -a -v ttrss:/var/dotnet/whiskydb/app.db /mnt/d/Documents/Development/whiskydb/DataBackup/app.db

rmdir /q /s d:\Documents\Development\publishfolder\
dotnet publish -c Release -o d:\Documents\Development\publishfolder\ -r linux-x64 --self-contained false

wsl rsync -a -v /mnt/d/Documents/Development/publishfolder/ ttrss:/var/dotnet/whiskydb/

ssh ttrss '/home/madmap/tmp/deploywhisky.sh'
