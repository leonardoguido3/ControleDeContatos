using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeContatos.Data.Map
{
    public class CidadeMap : IEntityTypeConfiguration<CidadeModel>
    {
        public void Configure(EntityTypeBuilder<CidadeModel> builder)
        {
            builder.HasKey(y => y.Id);
        }
    }
}
