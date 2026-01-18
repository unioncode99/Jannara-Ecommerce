namespace Jannara_Ecommerce.DTOs.Customer
{
    public class CustomerResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
    }
}
