﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Howest.MagicCards.Shared.Filters
{
    public class CardFilter : PaginationFilter
    {
        public string ArtistName { get; set; }

        public string Set { get; set; }

        public string CardName { get; set; }

        public string CardText { get; set; }

        public string CardRarity { get; set; }

        public string CardType { get; set; }

    }
}
