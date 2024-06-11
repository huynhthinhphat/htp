using System;
using System.Collections.Generic;

namespace MVC_QLNH.Models;

public partial class TbUserInfo
{
    public int UserInfoId { get; set; }

    public int? AccountId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public int Sex { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual ICollection<TbBillHistory> TbBillHistories { get; set; } = new List<TbBillHistory>();
}
