using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbDstable
{
    public int TableId { get; set; }

    public string TableName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<TbReport> TbReports { get; set; } = new List<TbReport>();

    public virtual ICollection<TbRevenu> TbRevenus { get; set; } = new List<TbRevenu>();
}
