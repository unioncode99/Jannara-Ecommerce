namespace Jannara_Ecommerce.DTOs.Product
{
    public class ProductResponseDTO
    {
        public ProductResponseDTO(int id, Guid publicId, string defaultImageUrl, string nameEn, string nameAr, decimal? minPrice, bool? isFavorite, decimal? averageRating, int? ratingCount)
        {
            Id = id;
            PublicId = publicId;
            DefaultImageUrl = defaultImageUrl;
            NameEn = nameEn;
            NameAr = nameAr;
            MinPrice = minPrice;
            IsFavorite = isFavorite;
            AverageRating = averageRating;
            RatingCount = ratingCount;
        }

        public int Id { get; set; }
        public Guid PublicId { get; set; }
        public string DefaultImageUrl { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal? MinPrice { get; set; }
        public bool? IsFavorite { get; set; } = false;
        public decimal? AverageRating { get; set; }
        public int? RatingCount { get; set; }
    }
}
