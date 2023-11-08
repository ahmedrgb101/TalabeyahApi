using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabeyahTaskApi.Domain.Common;

/// <summary>
/// Indicate Employer.
/// </summary>
public enum UserType
{
    [Description("Employer")]
    Employer,
    [Description("Applicant")]
    Applicant
}