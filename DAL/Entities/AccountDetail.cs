using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class AccountDetail
{
    public int AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? IdentityNumber { get; set; }

    public string? Avatar { get; set; }

    public virtual Account Account { get; set; } = null!;
}
