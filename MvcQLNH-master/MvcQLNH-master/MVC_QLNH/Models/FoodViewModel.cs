using MVC_QLNH.Models;
using System.Collections.Generic;

namespace MVC_QLNH.ViewModels
{
    public class CombinedViewModel
    {
        public List<TbFood> FoodViewModels { get; set; }
        public List<TbDmfood> CategoryViewModels { get; set; }
    }   
}
