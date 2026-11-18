using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.Aggregates;
using PC28168.U202222074.Openpay.Platform.Settlements.Domain.Model.ValueObjects;

namespace PC28168.U202222074.Openpay.Platform.Settlements.Infrastructure.Persistence.EFC.Configuration;

/// <summary>
///     Fluent API mapping of the <see cref="DisbursementOrder" /> aggregate to the disbursement_orders table.
/// </summary>
/// <remarks>
///     Keeps persistence concerns outside the domain model. Value objects are flattened into columns.
///     Author: Rodrigo Velasquez Velasquez
/// </remarks>
public class DisbursementOrderConfiguration : IEntityTypeConfiguration<DisbursementOrder>
{
    /// <summary>
    ///     Configures keys, value object conversions, column sizes and the unique tracking code index.
    /// </summary>
    /// <param name="builder">Builder of the DisbursementOrder entity type.</param>
    public void Configure(EntityTypeBuilder<DisbursementOrder> builder)
    {
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id).ValueGeneratedOnAdd();

        builder.Property(order => order.OrderTrackingCode)
            .HasConversion(reference => reference.Value, value => new OpenpayReference(value))
            .IsRequired();
        builder.HasIndex(order => order.OrderTrackingCode).IsUnique();

        builder.Property(order => order.AffiliateStoreId)
            .HasConversion(store => store.Value, value => new StoreIdentifier(value))
            .IsRequired();

        builder.ComplexProperty(order => order.ExecutionSchedule, schedule =>
        {
            schedule.Property(interval => interval.ScheduledFrom)
                .HasConversion(day => day.ToDateTime(TimeOnly.MinValue), stored => DateOnly.FromDateTime(stored))
                .HasColumnName("scheduled_from").HasColumnType("date");
            schedule.Property(interval => interval.ScheduledTo)
                .HasConversion(day => day.ToDateTime(TimeOnly.MinValue), stored => DateOnly.FromDateTime(stored))
                .HasColumnName("scheduled_to").HasColumnType("date");
        });

        builder.ComplexProperty(order => order.TransferCap, cap =>
        {
            cap.Property(money => money.Amount).HasColumnName("transfer_cap_amount").HasPrecision(18, 2);
            cap.Property(money => money.CurrencyCode).HasColumnName("transfer_cap_currency_code").HasMaxLength(10);
        });

        builder.Property(order => order.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(order => order.User).HasMaxLength(DisbursementOrder.MaxUserLength).IsRequired();
        builder.Property(order => order.Remarks).HasMaxLength(DisbursementOrder.MaxRemarksLength);
    }
}
