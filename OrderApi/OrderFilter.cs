using Library.DTOs.Order;
using Library.Interfaces;

namespace OrderAPI
{
    public class OrderFilter : IFilter<Order>
    {
        public DateTime? OrderDateStart { get; set; }
        public DateTime? OrderDateEnd { get; set; }
        public DateTime? DeliveryDateStart { get; set; }
        public DateTime? DeliveryDateEnd { get; set; }
        public OrderStatus? Status { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid? UserId { get; set; }

        public IQueryable<Order> Apply(IQueryable<Order> query)
        {
            if (OrderDateStart.HasValue)
            {
                query = query.Where(o => o.OrderDate >= OrderDateStart.Value);
            }

            if (OrderDateEnd.HasValue)
            {
                query = query.Where(o => o.OrderDate <= OrderDateEnd.Value);
            }
            if (Status != null) {
                query = query.Where(o => o.Status == Status);
            }
            if (DeliveryId != null)
            {
                query = query.Where(o => o.DeliveryTypeId == DeliveryId);
            }
            if (DeliveryDateEnd != null)
            {
                query = query.Where(o => o.DeliveryDate <= DeliveryDateEnd);
            }
            if (DeliveryDateStart != null)
            {
                query = query.Where(o => o.DeliveryDate >= DeliveryDateStart);
            }
            return query;
        }
    }
}
