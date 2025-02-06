namespace Library.DTOs.Order
{
    public class DeliveryType
    {
        public Guid Id { get; set; }

        public string ServiceName { get; set; }

        public DeliveryType()
        {
            ServiceName = string.Empty;
        }
    }
}