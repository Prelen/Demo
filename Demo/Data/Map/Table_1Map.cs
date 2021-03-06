using Demo.Data.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data.Map
{
    public class Table_1Map : IEntityTypeConfiguration<Table_1>
    {
        public void Configure(EntityTypeBuilder<Table_1> builder)
        {
            builder.HasKey(t => t.Table_1ID);
            builder.Property(t => t.name).IsRequired();
            builder.Property(t => t.email).IsRequired();
            builder.Property(t => t.company).IsRequired();
            builder.Property(t => t.address).IsRequired();
            builder.Property(t => t.city).IsRequired();
            builder.Property(t => t.country).IsRequired();
        }
    }
}
