﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS.Domain
{
    public class CoordinationRole
    {
        public int CoordinationRoleId { get; set; }
        public string Name { get; set; }
        public int AssignmentPercentage { get; set; }
    }
}