using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log4NetIntro
{
    public class Movie
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Price { get; set; }
        public long CountryId { get; set; } // Fk
        public string CountryName { get; set; } 

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
