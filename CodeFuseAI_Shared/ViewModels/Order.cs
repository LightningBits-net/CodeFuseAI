using System;
using CodeFuseAI_Shared.Data;

namespace CodeFuseAI_Shared.ViewModels
{
    public class Order
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}

