using System;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;

namespace whiskydb.Data
{

    public class WhiskyService
    {
        private string _whiskyFile;
        private string _imagePath;
        private object _lock = new object();

        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public WhiskyService(AuthenticationStateProvider authenticationStateProvider)
        {
            var path = Directory.GetCurrentDirectory();
            _authenticationStateProvider = authenticationStateProvider;
            _whiskyFile = Path.Combine(path, "wwwroot", "data", "whisky.json");
            _imagePath = Path.Combine(path, "wwwroot", "images");
        }

        public async Task<List<Whisky>> GetWhiskys(bool archive = false)
        {
            var whiskys = ReadAll();
            whiskys.RemoveAll(p => p.Finished != archive);
            return whiskys;
        }

        public async Task CleanImages()
        {
            var whiskeys = ReadAll();
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
                if (delete)
                {
                    File.Delete(file);
                }
            }
        }

        public async Task<Whisky> GetWhisky(string id)
        {
            var whiskeys = ReadAll();
            return whiskeys.Find(p => p.Id == id);
        }

        public async Task<List<Whisky>> DeleteWhisky(string id)
        {
            var whiskeys = ReadAll();
            if (await UserIsAdmin())
            {

                whiskeys.RemoveAll(p => p.Id == id);

                return WriteAll(whiskeys);
            }
            return whiskeys;

        }
        public async Task<List<Whisky>> PourOne(string id)
        {
            var whiskeys = ReadAll();
            if (await UserIsAdmin())
            {
                var whisky = whiskeys.Find(p => p.Id == id);
                whisky.LastPour = DateTime.Now;
                if (whisky.PourDates == null)
                {
                    whisky.PourDates = new List<DateTime>();
                }
                whisky.PourDates.Add(DateTime.Now);
                if (whisky.Pours.HasValue)
                    whisky.Pours = whisky.Pours.Value + 1;
                else whisky.Pours = 1;
                if (!whisky.Opened.HasValue)
                {
                    whisky.Opened = DateTime.Now;
                }
                var result = WriteAll(whiskeys);
                result.RemoveAll(p => p.Finished);
                return result;
            }
            return whiskeys;
        }

        public async Task<List<Whisky>> UpdateWhisky(Whisky toUpdate)
        {

            var whiskeys = ReadAll();

            if (await UserIsAdmin())
            {

                whiskeys.RemoveAll(p => p.Id == toUpdate.Id);

                whiskeys.Add(toUpdate);

                return WriteAll(whiskeys);
            }
            return whiskeys;
        }

        public async Task<List<Whisky>> AddWhisky(Whisky whiskyToAdd)
        {
            var whiskeys = ReadAll();
            if (await UserIsAdmin())
            {
                whiskeys.Add(whiskyToAdd);
                return WriteAll(whiskeys);
            }
            return whiskeys;

        }

        private async Task<bool> UserIsAdmin()
        {
            var status = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return status.User.IsInRole("Admin");
        }

        public async Task<Tuple<string, string>> SaveImage(IBrowserFile file)
        {
            if (!await UserIsAdmin())
            {
                return null;
            }
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

        private List<Whisky> ReadAll()
        {
            lock (_lock)
            {
                if (File.Exists(_whiskyFile))
                {
                    var whiskeyString = File.ReadAllText(_whiskyFile);
                    var whiskeys = JsonSerializer.Deserialize<List<Whisky>>(whiskeyString);
                    whiskeys.Sort();
                    return whiskeys;
                }
                else return new List<Whisky>();
            }
        }
        private List<Whisky> WriteAll(List<Whisky> whiskys)
        {
            lock (_lock)
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                string json = JsonSerializer.Serialize(whiskys, options);
                File.WriteAllText(_whiskyFile, json);
            }
            return whiskys;
        }
    }
}
