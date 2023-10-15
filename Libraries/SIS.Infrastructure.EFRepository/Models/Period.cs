﻿using System;
using System.Collections.Generic;

namespace SIS.Infrastructure.EFRepository.Models;

public partial class Period
{
    public int PeriodId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime AutoTimeCreation { get; set; }

    public DateTime AutoTimeUpdate { get; set; }

    public int AutoUpdateCount { get; set; }

    public virtual ICollection<LectorAssignmentPercentageInterest> LectorAssignmentPercentageInterests { get; set; } = new List<LectorAssignmentPercentageInterest>();
}