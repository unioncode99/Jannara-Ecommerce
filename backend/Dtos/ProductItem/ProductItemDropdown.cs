namespace Jannara_Ecommerce.DTOs.ProductItem
{
    public class ProductItemDropdown
    {
        public ProductItemDropdown(int id, string sku)
        {
            Id = id;
            Sku = sku;
        }

        public int Id { get; set; }
        public string Sku {  get; set; }   
    }
}
