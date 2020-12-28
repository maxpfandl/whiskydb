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
        private ApplicationDbContext _context;
        public WhiskyService(AuthenticationStateProvider authenticationStateProvider, ApplicationDbContext context)
        {
            var path = Directory.GetCurrentDirectory();
            _authenticationStateProvider = authenticationStateProvider;
            _context = context;
            _whiskyFile = Path.Combine(path, "wwwroot", "data", "whisky.json");
            _imagePath = Path.Combine(path, "wwwroot", "images");
        }

        public async Task<List<Whisky>> GetWhiskys(bool archive = false)
        {

            var whiskys = _context.Whiskys.Where(a => a.Finished == archive).ToList();
            return whiskys;
        }

        public async Task CleanImages()
        {
            var whiskeys = _context.Whiskys;
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
            return _context.Whiskys.Single(p => p.Id == id);
        }

        public async Task<List<Whisky>> DeleteWhisky(string id)
        {
            if (await UserIsAdmin())
            {
                var whisky = _context.Whiskys.Single(p => p.Id == id);
                _context.Whiskys.Remove(whisky);
                await _context.SaveChangesAsync();
            }
            return _context.Whiskys.ToList();

        }
        public async Task<List<Whisky>> PourOne(string id)
        {
            if (await UserIsAdmin())
            {
                var whisky = _context.Whiskys.Single(p => p.Id == id);
                whisky.LastPour = DateTime.Now;
                whisky.PourDates += DateTime.Now.ToShortDateString()+"||";
                if (whisky.Pours.HasValue)
                    whisky.Pours = whisky.Pours.Value + 1;
                else whisky.Pours = 1;
                if (!whisky.Opened.HasValue)
                {
                    whisky.Opened = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }
            return await GetWhiskys(false);
        }

        public async Task UpdateWhisky(Whisky toUpdate)
        {
            if (await UserIsAdmin())
            {
                
                _context.Whiskys.Attach(toUpdate);
                _context.Entry(toUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task AddWhisky(Whisky whiskyToAdd)
        {
            if (await UserIsAdmin())
            {
                _context.Whiskys.Add(whiskyToAdd);
                await _context.SaveChangesAsync();
            }
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



    }
}
