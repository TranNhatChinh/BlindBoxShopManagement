using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Account
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string Role { get; set; } = null!;

    public virtual AccountDetail? AccountDetail { get; set; }
}
