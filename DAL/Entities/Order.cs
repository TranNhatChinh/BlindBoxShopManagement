using System;
using System.Collections.Generic;

namespace DAL.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? PhoneNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
