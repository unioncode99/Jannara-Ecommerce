namespace Jannara_Ecommerce.Business.Interfaces
{
    public interface IPasswordService 
    {
        public string HashPassword<T>(T user, string password) where T : class;
        public bool VerifyPassword<T>(T user,string password, string enteredPassword) where T : class;
    }
}
