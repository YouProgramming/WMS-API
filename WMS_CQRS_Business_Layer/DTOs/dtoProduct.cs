using Microsoft.AspNetCore.Http;

namespace WMS_CQRS_Business_Layer.DTOs
{
    public class dtoProduct
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string? ProductImagePath { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? ProductImage { get; set; } = null;
        public decimal UnitPrice { get; set; }

        public int QuantityInStock { get; set; }

        public int CategoryId { get; set; }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(ProductName)
                && string.IsNullOrWhiteSpace(Description)
                && ProductId == 0
                && UnitPrice == 0
                && CategoryId == 0;
        }
        public bool Validate()
        {
            if (IsEmpty())
                return false;

            return !(UnitPrice <= 0 || CategoryId <= 0);
        }
    }
}
