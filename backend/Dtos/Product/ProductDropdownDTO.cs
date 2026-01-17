namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductDropdownDTO
    {
        public ProductDropdownDTO(int id, string nameEn, string nameAr)
        {
            Id = id;
            NameEn = nameEn;
            NameAr = nameAr;
        }

        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
