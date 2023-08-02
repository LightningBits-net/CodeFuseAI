// LightningBits
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFuseAI_Shared.Data
{
    public class ProductPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string MyProperty { get; set; }

        public string Size { get; set; }

        public double Price { get; set; }

    }
}

