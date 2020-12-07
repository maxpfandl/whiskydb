using System;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace whiskyserverapp.Data
{
    public class WhiskyService
    {
        private string _whiskyFile;
        public WhiskyService()
        {
            var path = Directory.GetCurrentDirectory();
            _whiskyFile = Path.Combine(path, "wwwroot", "whisky.json");
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
                    whisky.Pours = whisky.Pours.Value+1;
                else whisky.Pours = 1;
                string json = JsonSerializer.Serialize(whiskeys);
                File.WriteAllText(_whiskyFile, json);
            }

        }

        public async Task UpdateWhisky(Whisky toUpdate)
        {
            var whiskeys = await GetWhiskys();

            whiskeys.RemoveAll(p => p.Id == toUpdate.Id);

            whiskeys.Add(toUpdate);

            string json = JsonSerializer.Serialize(whiskeys);

            File.WriteAllText(_whiskyFile, json);
        }

        public async Task AddWhisky(Whisky whiskyToAdd)
        {

            var whiskeys = await GetWhiskys();
            whiskeys.Add(whiskyToAdd);
            string json = JsonSerializer.Serialize(whiskeys);

            File.WriteAllText(_whiskyFile, json);
        }
    }
}
