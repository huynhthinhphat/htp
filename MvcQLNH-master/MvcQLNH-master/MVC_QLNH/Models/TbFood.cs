using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbFood
{
    public int FoodId { get; set; }

    public string FoodName { get; set; } = null!;

    public int? DmfoodId { get; set; }

    public int Price { get; set; }

    public string? AvtFood { get; set; }

    public virtual TbDmfood? Dmfood { get; set; }

    public virtual ICollection<TbReport> TbReports { get; set; } = new List<TbReport>();
}
