using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GameStore.Domain.Entities
{
    public class Game
    {
        [HiddenInput(DisplayValue = false)]
        public int GameId { get; set; }
        
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter name of game")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Desciption")]
        [Required(ErrorMessage = "Enter description of game")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Enter category of game")]
        public string Category { get; set; }

        [Display(Name = "Price (grn)")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "enter a positive value for the price")]
        public decimal Price { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
