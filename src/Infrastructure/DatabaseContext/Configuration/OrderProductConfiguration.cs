using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.DatabaseContext.Configuration
{
    public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.HasOne(x => x.Product).WithMany(x => x.OrderProducts).HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Order).WithMany(x => x.OrderProducts).HasForeignKey(x => x.OrderId);
        }
    }
}
