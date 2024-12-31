using System.ComponentModel.DataAnnotations;


namespace Library.DTOs.Order
{
    public enum OrderStatus
    {
        PENDING,
        PROCESSING,
        PAYMENT,
        TRANSIT,
        COMPLETED,
    }
}
