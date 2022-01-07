wsl rsync -a -v piServer:/var/dotnet/whiskydb/wwwroot/images/* /mnt/d/Development/whiskydb/DataBackup/images
wsl rsync -a -v piServer:/var/dotnet/whiskydb/app.db /mnt/d/Development/whiskydb/DataBackup/app.db

rmdir /q /s d:\Documents\Development\publishfolder\
dotnet publish -c Release -o d:\Development\_publish\ -r ubuntu-arm64 --self-contained false

wsl rsync -a -v /mnt/d/Development/_publish/ piServer:/home/madmap/deployscripts/whisky/

ssh piServer '/home/madmap/deployscripts/whisky.sh'
