using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class PagingInfo
    {
        // Кіл-сть товарів
        public int TotalItems { get; set; }

        // Кіл-сть товарів на одній сторінці
        public int ItemsPerPage { get; set; }

        // Номер сторінки
        public int CurrentPage { get; set; }

        // Загальна кіл-сть сторінок
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}