using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbFood
{
    public int FoodId { get; set; }

    public string? FoodName { get; set; }

    public int? DmfoodId { get; set; }

    public int Price { get; set; }

    public string? AvtFood { get; set; }

    public virtual TbDmfood? Dmfood { get; set; }

    public virtual ICollection<TbBillHistory> TbBillHistories { get; set; } = new List<TbBillHistory>();
}
