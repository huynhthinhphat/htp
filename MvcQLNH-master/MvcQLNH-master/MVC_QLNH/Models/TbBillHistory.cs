using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbBillHistory
{
    public int BillId { get; set; }

    public int? TableID { get; set; }

    public int? UserInfoId { get; set; }

    public int? FoodId { get; set; }

    public int Quantity { get; set; }

    public int Price { get; set; }

    [DisplayFormat(DataFormatString = "{0:#,##0}")]
    public int TotalPrice { get; set; }

   /* [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]*/
    public DateOnly BillDate { get; set; }

    public virtual TbFood? Food { get; set; }

    public virtual TbDstable? Table { get; set; }

    public virtual ICollection<TbRevenu> TbRevenus { get; set; } = new List<TbRevenu>();

    public virtual TbUserInfo? UserInfo { get; set; }
}
