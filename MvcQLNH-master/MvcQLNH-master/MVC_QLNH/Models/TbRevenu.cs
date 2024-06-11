using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbRevenu
{
    public int RevenuId { get; set; }

    public int? BillId { get; set; }

    public DateOnly RevenuDate { get; set; }

    public int RevenuAmount { get; set; }

    public virtual TbBillHistory? Bill { get; set; }
}
