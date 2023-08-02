using System;
using CodeFuseAI_Shared.Data;

namespace CodeFuseAI_Shared.Models
{
	public class OrderDTO
    {
        public OrderHeaderDTO OrderHeaders { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public OrderHeaderDTO OrderHeader { get; set; }
    }
}

