using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrdersModels
{
    public class OrderResultDto
    {


        // Id

        public Guid id { get; set; }

        // User Email

        public string UserEmail { get; set; }

        // Shipping Address

        public AddressDto ShippingAddress { get; set; }

        // Order Item 

        public ICollection<OrderItemDto> OrderItems { get; set; }  // Navigational Property 


        // Delivery Method 
        public string DeliveryMethod { get; set; }


        // Order Payment Status 

        public string OrderPaymentStatus { get; set; }

        // Sub Total 

        public decimal SubTotal { get; set; }

        // Order Date 

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;


        // Payment Id [El Id Of ElFatora]

        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal Total { get; set; }

    }
}
