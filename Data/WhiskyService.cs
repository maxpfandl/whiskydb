using System;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;

namespace whiskyserverapp.Data
{
    public class WhiskyService
    {
        private string _whiskyFile;
        private string _imagePath;
        public WhiskyService()
        {
            var path = Directory.GetCurrentDirectory();
            _whiskyFile = Path.Combine(path, "wwwroot", "whisky.json");
            _imagePath = Path.Combine(path, "wwwroot", "images");
        }

        public async Task<List<Whisky>> GetWhiskys(bool includeFinished = false)
        {
            if (File.Exists(_whiskyFile))
            {
                var whiskeyString = await File.ReadAllTextAsync(_whiskyFile);
                var whiskys = JsonSerializer.Deserialize<List<Whisky>>(whiskeyString);
                if (!includeFinished)
                {
                    whiskys.RemoveAll(p => p.Finished);
                }
                whiskys.Sort();
                return whiskys;
            }
            else
            {
                return new List<Whisky>();
            }
        }

        public async Task CleanImages()
        {
            var whiskeys = await GetWhiskys(true);
            var files = Directory.GetFiles(_imagePath);
            foreach (var file in files)
            {
                var delete = true;
                var fileName = Path.GetFileName(file);
                foreach (var whiskey in whiskeys)
                {
                    var fileNameW = Path.GetFileName(whiskey.ImageUrl);
                    if (fileNameW == fileName)
                    {
                        delete = false;
                        break;
                    }
                    fileNameW = Path.GetFileName(whiskey.ImageUrlTh);
                    if (fileNameW == fileName)
                    {
                        delete = false;
                        break;
                    }
                }
                if(delete){
                    File.Delete(file);
                }
            }
        }

        public async Task<Whisky> GetWhisky(string id)
        {
            if (File.Exists(_whiskyFile))
            {
                var whiskeyString = await File.ReadAllTextAsync(_whiskyFile);
                var whiskeys = JsonSerializer.Deserialize<List<Whisky>>(whiskeyString);
                return whiskeys.Find(p => p.Id == id);
            }
            else
            {
                return null;
            }
        }

        public async Task PourOne(string id)
        {
            if (File.Exists(_whiskyFile))
            {
                var whiskeyString = await File.ReadAllTextAsync(_whiskyFile);
                var whiskeys = JsonSerializer.Deserialize<List<Whisky>>(whiskeyString);
                var whisky = whiskeys.Find(p => p.Id == id);
                whisky.LastPour = DateTime.Now;
                if (whisky.Pours.HasValue)
                    whisky.Pours = whisky.Pours.Value + 1;
                else whisky.Pours = 1;
                string json = JsonSerializer.Serialize(whiskeys);
                File.WriteAllText(_whiskyFile, json);
            }

        }

        public async Task UpdateWhisky(Whisky toUpdate)
        {
            var whiskeys = await GetWhiskys(true);

            whiskeys.RemoveAll(p => p.Id == toUpdate.Id);

            whiskeys.Add(toUpdate);

            string json = JsonSerializer.Serialize(whiskeys);

            File.WriteAllText(_whiskyFile, json);
        }

        public async Task AddWhisky(Whisky whiskyToAdd)
        {

            var whiskeys = await GetWhiskys(true);
            whiskeys.Add(whiskyToAdd);
            string json = JsonSerializer.Serialize(whiskeys);

            File.WriteAllText(_whiskyFile, json);
        }

        public async Task<Tuple<string, string>> SaveImage(IBrowserFile file)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", "");
            var fileName = guid + ".png";
            var fileNameThumb = guid + "_th.png";


            var format = "image/png";
            var thumbImage = await file.RequestImageFileAsync(format, 150, 150);
            var fullImage = await file.RequestImageFileAsync(format, 2048, 2048);


            using (var stream = File.Create(Path.Combine(_imagePath, fileName)))
            {
                var imageStream = fullImage.OpenReadStream(10 * 1024 * 1024 * 10);
                await imageStream.CopyToAsync(stream);
            }
            using (var stream = File.Create(Path.Combine(_imagePath, fileNameThumb)))
            {
                var imageStream = thumbImage.OpenReadStream(10 * 1024 * 1024 * 10);
                await imageStream.CopyToAsync(stream);
            }
            return new Tuple<string, string>($"images/{fileName}", $"images/{fileNameThumb}");
        }
    }
}
