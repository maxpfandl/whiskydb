REM rmdir /q /s \\192.168.0.15\docker\rssfeed\bin\
REM dotnet publish -c Release -o \\192.168.0.15\docker\rssfeed\bin\
REM c:\Data\publishfolder\

rmdir /q /s d:\Documents\Development\publishfolder\
dotnet publish -c Release -o d:\Documents\Development\publishfolder\ -r linux-x64 --self-contained false
rmdir /q /s d:\Documents\Development\publishfolder\wwwroot\images\
del d:\Documents\Development\publishfolder\wwwroot\whisky.json
cd d:\Documents\Development\publishfolder\
scp -r * ttrss:/home/madmap/tmp/whis
ssh ttrss '/home/madmap/tmp/deploywhis.sh'
