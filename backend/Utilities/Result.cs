namespace Jannara_Ecommerce.Utilities
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public ResponseMessage Message { get; set; }
        public int ErrorCode { get; set; }
        public T? Data { get; set; }
        public Result(bool isSuccess, string message, T? data = default, int errorCode = 0)
        {
            IsSuccess = isSuccess;
            Message = new ResponseMessage(message);
            ErrorCode = errorCode;
            Data = data;
        }
        public Result(bool success, ResponseMessage message, T? data = default, int errorCode = 0)
        {
            IsSuccess = success;
            Message = message;
            ErrorCode = errorCode;
            Data = data;
        }
    }
}
