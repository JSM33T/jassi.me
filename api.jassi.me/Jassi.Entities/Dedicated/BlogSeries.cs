﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jassi.Entities.Dedicated
{
    public class BlogSeries
    {
        public int Id { get; set; }
        public string SeriesName { get; set; }
        public string Slug { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
