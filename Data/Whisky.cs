using System;
using System.Collections.Generic;

namespace whiskyserverapp.Data
{
    public class Whisky : IComparable<Whisky>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int? Age { get; set; }
        public decimal? Alc { get; set; }
        public DateTime? Distilled { get; set; }
        public DateTime? Botteled { get; set; }
        public DateTime? Aquired { get; set; }
        public DateTime? Opened { get; set; }
        public DateTime? LastPour { get; set; }
        public List<DateTime> PourDates { get; set; }
        public int? Pours { get; set; }
        public decimal? Price { get; set; }
        public string Notes { get; set; }
        public string Nose { get; set; }
        public string Palate { get; set; }
        public string Finish { get; set; }
        public string Rating { get; set; }
        public string ImageUrlTh { get; set; }
        public string ImageUrl { get; set; }

        public bool Finished { get; set; }
        public bool UnChillfiltered { get; set; }
        public bool ArtColor { get; set; }
        public bool CaskStrength { get; set; }
        public bool SingleCask { get; set; }

        public string GetPoursString()
        {
            if (Pours.HasValue)
                return $"({Pours})";
            else return "";
        }

        public string GetFinishedString()
        {
            if (Finished && LastPour.HasValue)
                return $"Finished on {LastPour.Value.ToString("yyyy-MM-dd")}";
            else if (Finished)
                return "Finished";
            else return "Still there";
        }

        public int CompareTo(Whisky other)
        {

            if (other.LastPour.HasValue && this.LastPour.HasValue)
            {
                return other.LastPour.Value.CompareTo(this.LastPour.Value);
            }

            if (other.LastPour.HasValue)
                return 1;

            if (this.LastPour.HasValue)
                return -1;

            return 0;
        }
    }


}