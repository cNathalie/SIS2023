﻿using System;
using System.Collections.Generic;

namespace SIS.Infrastructure.EFRepository.Models;

public partial class TeacherCoordinationRoleInterest
{
    public int TeacherCoordinationRoleInterestId { get; set; }

    public int AcademicYearId { get; set; }

    public int TeacherId { get; set; }

    public int TeacherPreferenceId { get; set; }

    public int CoordinationRoleId { get; set; }

    public DateTime AutoTimeCreation { get; set; }

    public DateTime AutoTimeUpdate { get; set; }

    public int AutoUpdateCount { get; set; }

    public virtual AcademicYear AcademicYear { get; set; } = null!;

    public virtual CoordinationRole CoordinationRole { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual TeacherPreference TeacherPreference { get; set; } = null!;
}
