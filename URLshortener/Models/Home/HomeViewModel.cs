using System.ComponentModel.DataAnnotations;
using URLshortener.Domain.Entities;
namespace URLshortener.Models.Home
{
    public class HomeViewModel
    {
        [Required(ErrorMessage="Данное поле обязательно")]
        public string LongURL { get; set; }

        public IEnumerable<UrlEntry>? UrlEntries { get; set; }
        public IEnumerable<UserLink>? UserLinks { get; set; }
    }
}
