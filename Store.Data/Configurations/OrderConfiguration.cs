using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entities.OrderEntities;

namespace Store.Data.Configurations;

public class OrderConfiguration:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.OwnsOne(Order => Order.ShipToAddress, x =>
        {
            x.WithOwner();
        });
        builder.HasMany(order => order.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
    }
}