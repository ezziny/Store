using System.Linq.Expressions;
using Store.Data.Entities.OrderEntities;

namespace Store.Repository.Specifications.OrderSpecification;

public class OrderWithItemSpecification: BaseSpecification<Order>
{
    public OrderWithItemSpecification(string buyerEmail) :
        base(order => order.BuyerEmail == buyerEmail)
    {
        AddInclude(order => order.DeliveryMethod);
        AddInclude(order => order.OrderItems);
        AddOrderByDescending(order => order.OrderDate);
    }    
    public OrderWithItemSpecification(Guid id) :
        base(order => order.Id == id)
    {
        AddInclude(order => order.DeliveryMethod);
        AddInclude(order => order.OrderItems);
    }
}