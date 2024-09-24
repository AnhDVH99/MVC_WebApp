using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class OrderDetail
{
    public Guid OrderDetailID { get; set; }
    
    public Guid OrderID { get; set; }

    public Guid ProductID { get; set; }

    public Guid UnitID { get; set; }

    public int Quantity { get; set; }

    public Guid PriceDetailID { get; set; }
    
    public float Price { get; set; }

    public int Discount { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime DueDate { get; set; }

    public Unit? Unit { get; set; }

    public Order? Order { get; set; }

    public ICollection<Product>? Products { get; set; }
}