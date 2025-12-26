namespace Jannara_Ecommerce.DTOs.State
{
    public class StateDTO
    {
        public StateDTO(int id, string code, string nameEn, string nameAr, decimal extraFeeForShipping, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            Code = code;
            NameEn = nameEn;
            NameAr = nameAr;
            ExtraFeeForShipping = extraFeeForShipping;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal ExtraFeeForShipping { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
