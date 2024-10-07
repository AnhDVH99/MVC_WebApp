using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class OrderDetail
{
    public Guid OrderDetailID { get; set; }
    
    public Guid OrderID { get; set; }

    public Guid ProductID { get; set; }

    public Guid UnitID { get; set; }

    [Required(ErrorMessage ="Quantity is required")]
    public int Quantity { get; set; }

    public Guid PriceDetailID { get; set; }
    
    public float Price { get; set; }

    public float priceBeforeTax { get; set; }

    public float priceAfterTax { get; set; }

    public float VAT { get; set; }
    public float EnvironmentTax { get; set; }

    public decimal TotalAmount { get; set; }

    public Unit? Unit { get; set; }

    public Order? Order { get; set; }

    public ICollection<Product>? Products { get; set; }
}