﻿using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.DatabaseContext.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.GrossValue).HasPrecision(18, 2);
            builder.Property(x => x.NetValue).HasPrecision(18, 2);
            builder.Property(x => x.DiscountPerc).HasPrecision(18, 2);
            builder.Property(x => x.DiscountValue).HasPrecision(18, 2);
            builder.Property(x => x.CustomerName).HasMaxLength(100);

            builder.HasMany(x => x.OrderProducts).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
        }
    }
}
