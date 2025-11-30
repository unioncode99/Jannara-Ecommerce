namespace Jannara_Ecommerce.Utilities
{
    public class ResponseMessage
    {
        public ResponseMessage(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
