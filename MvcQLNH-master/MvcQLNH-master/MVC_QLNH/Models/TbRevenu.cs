using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbRevenu
{
    public int RevenuId { get; set; }

    public int SlTable { get; set; }

    public DateOnly RevenuDateIn { get; set; }

    public DateOnly RevenuDateOut { get; set; }

    public int SumMoney { get; set; }

    public virtual TbDstable SlTableNavigation { get; set; } = null!;
}
