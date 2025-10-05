using System.Linq.Expressions;
using Store.Data.Entities.OrderEntities;

namespace Store.Repository.Specifications.OrderSpecification;

public class OrderWithPaymentIntentSpecification:BaseSpecification<Order>
{
    public OrderWithPaymentIntentSpecification(string? paymentIntentId) 
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
    }
}