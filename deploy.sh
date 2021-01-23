sudo chown -R dotnet /home/madmap/deployscripts/whisky/*
sudo chgrp -R dotnet /home/madmap/deployscripts/whisky/*
sudo chmod 644 /home/madmap/deployscripts/whisky/*
sudo chmod 744 /home/madmap/deployscripts/whisky/whiskydb
sudo find /home/madmap/deployscripts/whisky/ -type d -exec chmod 755 {} \;
sudo rsync -a /home/madmap/deployscripts/whisky/* /var/dotnet/whiskydb/
sudo rm -r /home/madmap/deployscripts/whisky/*
sudo systemctl restart whiskydb.service
