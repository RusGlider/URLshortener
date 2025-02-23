using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLshortener.Domain.Entities
{
    public class UrlEntry : Entity
    {
        public string LongURL { get; set; }
        public string ShortURL { get; set; }
        public DateTime CreationDate { get; set; }
        public int Clicks { get; set; }
    }
}
