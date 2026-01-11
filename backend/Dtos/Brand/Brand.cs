namespace Jannara_Ecommerce.DTOs.Brand
{
    //public class BrandDetailDTO
    public class BrandDTO
    {
		//public int BrandId { get; set; }
		public int Id { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }
		public string LogoUrl { get; set; }
		public string WebsiteUrl { get; set; }
		public string? DescriptionEn { get; set; }
		public string? DescriptionAr { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
