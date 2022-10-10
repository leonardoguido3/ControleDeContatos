using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleDeContatos.Data.Map
{
    public class ContatoMap : IEntityTypeConfiguration<ContatoModel>
    {
        //Metodo para mapeamento de tabelas, onde a tabela Id dos usuarios esta vinculada com a usuario na contato 
        public void Configure(EntityTypeBuilder<ContatoModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Usuario);
            builder.HasOne(x => x.Cidade);
        }
    }
}
