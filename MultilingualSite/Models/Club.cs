using System.ComponentModel.DataAnnotations;

namespace MultilingualSite.Models
{
    public class Club
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "NameRequired")]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "CityRequired")]
        [Display(Name = "City")]
        public string? City { get; set; }

        [Required(ErrorMessage = "CountryRequired")]
        [Display(Name = "Country")]
        public string? Country { get; set; }
    }
}