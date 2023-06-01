namespace EndWerk.API2.Dto
{
    public class ProductDTO
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int UnitInStock { get; set; }

        public string ProductDescription { get; set; }

        public decimal ProductPrice { get; set; }

        public int supplierId { get; set; }

        public int ProductCategoryId { get; set; }

    }
}
