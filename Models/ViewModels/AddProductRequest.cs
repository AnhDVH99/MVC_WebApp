﻿using ASP.NET_Core_MVC_Piacom.Models.Domain;

namespace ASP.NET_Core_MVC_Piacom.Models.ViewModels
{
    public class AddProductRequest
    {
        public Guid ProductID { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductStatus { get; set; }

        public string SysU { get; set; }

        public DateTime SysD { get; set; }


    }
}
