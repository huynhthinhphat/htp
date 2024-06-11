using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbDstable
{
    public int TableId { get; set; }

    public string TableName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<TbBillHistory> TbBillHistories { get; set; } = new List<TbBillHistory>();
}
