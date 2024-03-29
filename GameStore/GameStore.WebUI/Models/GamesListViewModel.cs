﻿using System;
using System.Collections.Generic;
using GameStore.Domain.Entities;
using System.Linq;
using System.Web;

namespace GameStore.WebUI.Models
{
    public class GamesListViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}