﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jassi.Entities.Dedicated
{
    public class BlogType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Slug { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }

}
