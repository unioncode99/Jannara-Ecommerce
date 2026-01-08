namespace Jannara_Ecommerce.DTOs.User
{
    public class FilterUserDTO
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? CurrentUserId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public int? RoleId { get; set; }
        // allowed values:
        // email_asc | email_desc | newest | oldest | username_asc | username_desc 
    }
}
