using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Як вас величати")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введіть першу адресу доставки")]
        [Display(Name="First Address")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Вкажіть своє місто")]
        [Display(Name="City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Вкажіть свою країну")]
        [Display(Name="Country")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }

    }
}
