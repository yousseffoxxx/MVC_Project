﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Department
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public DateTime CDate { get; set; }
    }
}
