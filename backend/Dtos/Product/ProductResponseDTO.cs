namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductResponseDTO
    {
        public ProductResponseDTO(int id, string defaultImageUrl, string nameEn, string nameAr, decimal? minPrice, bool? isFavorite)
        {
            Id = id;
            DefaultImageUrl = defaultImageUrl;
            NameEn = nameEn;
            NameAr = nameAr;
            MinPrice = minPrice;
            IsFavorite = isFavorite;
        }

        public int Id { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal? MinPrice { get; set; }
        public bool? IsFavorite { get; set; } = false;
    }
}
