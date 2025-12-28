namespace Jannara_Ecommerce.DTOs.Order
{
    public class PlaceOrderResponseDTO
    {
        public PlaceOrderResponseDTO(OrderDTO order, string clientSecret)
        {
            Order = order;
            ClientSecret = clientSecret;
        }

        public OrderDTO Order { get; set; }
        public string ClientSecret { get; set; }
    }
}
