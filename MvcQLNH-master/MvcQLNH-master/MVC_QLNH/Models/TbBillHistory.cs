using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbBillHistory
{
    public int BillId { get; set; }

    public string? FoodName { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    public string? TableName { get; set; }

    public DateOnly BillDate { get; set; }

    public int TotalAmount { get; set; }

    public int? UserInfoId { get; set; }

    public string? CustomerName { get; set; }

    public int? Sdt { get; set; }

    public virtual TbUserInfo? UserInfo { get; set; }
}
