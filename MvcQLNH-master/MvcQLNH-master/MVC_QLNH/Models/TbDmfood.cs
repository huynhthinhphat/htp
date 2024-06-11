using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbDmfood
{
    public int DmfoodId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<TbFood> TbFoods { get; set; } = new List<TbFood>();
}
