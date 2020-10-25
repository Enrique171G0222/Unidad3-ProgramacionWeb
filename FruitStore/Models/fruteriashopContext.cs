using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FruitStore.Models
{
    public partial class fruteriashopContext : DbContext
    {
        public fruteriashopContext()
        {
        }

        public fruteriashopContext(DbContextOptions<fruteriashopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;port=3306;database=fruteriashop;uid=root; password=root; TreatTinyAsBoolean=false");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorias>(entity =>
            {
                entity.ToTable("categorias");

                entity.HasIndex(e => e.Nombre)
                    .HasName("NombreGrupo");

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.Eliminado)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Productos>(entity =>
            {
                entity.ToTable("productos");

                entity.HasIndex(e => e.Id)
                    .HasName("IdProducto");

                entity.HasIndex(e => e.IdCategoria)
                    .HasName("IdGrupo");

                entity.Property(e => e.Id).HasColumnType("int(10)");

                entity.Property(e => e.Descripcion).HasColumnType("text");

                entity.Property(e => e.IdCategoria).HasColumnType("int(10)");

                entity.Property(e => e.Nombre).HasColumnType("varchar(50)");

                entity.Property(e => e.Precio).HasColumnType("decimal(19,4)");

                entity.Property(e => e.UnidadMedida).HasColumnType("varchar(45)");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.Productos)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("fk_categorias");
            });
        }
    }
}
