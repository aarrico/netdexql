using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NetDexQL.Data.Models;

public class TypeEffectiveness
{
    public Guid AttackingTypeId { get; set; }

    public Guid DefendingTypeId { get; set; }

    public double Multiplier { get; set; }

    public MonType AttackingType { get; set; }
    public MonType DefendingType { get; set; }

    public TypeEffectiveness(Guid attackingTypeId, Guid defendingTypeId, double multiplier)
    {
        AttackingTypeId = attackingTypeId;
        DefendingTypeId = defendingTypeId;
        Multiplier = multiplier;
    }
}

public class TypeEffectivenessConfiguration : IEntityTypeConfiguration<TypeEffectiveness>
{
    public void Configure(EntityTypeBuilder<TypeEffectiveness> builder)
    {
        builder.HasKey(t => new { t.AttackingTypeId, t.DefendingTypeId });
    }
}