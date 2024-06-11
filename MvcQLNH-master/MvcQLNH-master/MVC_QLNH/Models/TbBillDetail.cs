using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbBillDetail
{
    public int BillDetailId { get; set; }

    public int? BillId { get; set; }

    public int? FoodId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual TbBillHistory? Bill { get; set; }

    public virtual TbFood? Food { get; set; }
}
