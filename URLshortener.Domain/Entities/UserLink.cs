using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLshortener.Domain.Entities
{
    /*
    class UserLink<TPrimaryKey> : Entity
    {
        public TPrimaryKey UserId { get; set; }
        public TPrimaryKey UrlId { get; set; }
    }
    */

    public class UserLink : Entity
    {
        public int UserId { get; set; }
        public int UrlId { get; set; }
    }
}
