using ControleDeContatos.Data.Map;
using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Data
{
    public class DataContext : DbContext
    {
        //Injeção Context para o Migration, para manipular o banco de dados
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //Modelo de banco de dados Contatos
        public DbSet<ContatoModel> Contatos { get; set; }

        //Modelo de banco de dados Usuarios
        public DbSet<UsuarioModel> Usuarios { get; set; }

        public DbSet<CidadeModel> Cidades { get; set; }

        //Metodo protegido que sobreescreve o OnModelCreating, para criar um mapeamento, e vinculo entre tabelas
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContatoMap());
            modelBuilder.ApplyConfiguration(new CidadeMap());

            base.OnModelCreating(modelBuilder);
        }

    }
}
