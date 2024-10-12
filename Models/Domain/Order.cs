using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP.NET_Core_MVC_Piacom.Models.Domain;

public class Order
{
    [Required]
    public Guid OrderID { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public DateTime RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    [Required]
    public string OrderStatus { get; set; }

    public string? Comment { get; set; }

    [Required]
    [ForeignKey("Customer")]
    public Guid CustomerID { get; set; }

    [Required]
    public Guid EmployeeID { get; set; }

    public string? SysU { get; set; }

    public DateTime SysD { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }

    public Customer Customer { get; set; }
}

