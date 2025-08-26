using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class InventarioContext : DbContext
    {
        public InventarioContext(DbContextOptions<InventarioContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Material> Materiales { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.NombreUsuario)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(u => u.Contraseña)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.NombreCompleto)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(u => u.LocalAsignado)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.HasOne(u => u.Rol)
                    .WithMany(r => r.Usuarios)
                    .HasForeignKey(u => u.RolId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurar Roles
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("materiales_def"); // Nombre de la tabla en la base de datos
                entity.HasKey(m => m.Id);

                entity.Property(m => m.Id).HasColumnName("id");
                entity.Property(m => m.MaterialCod).HasColumnName("material").HasMaxLength(20);
                entity.Property(m => m.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
                entity.Property(m => m.Almacen).HasColumnName("almacen").HasMaxLength(20);
                entity.Property(m => m.Lote).HasColumnName("lote").HasMaxLength(50);
                entity.Property(m => m.Cantidad).HasColumnName("cantidad").HasColumnType("decimal(10,2)");
                entity.Property(m => m.Conteo).HasColumnName("conteo").HasColumnType("decimal(10,2)");
                entity.Property(m => m.Reconteo).HasColumnName("reconteo").HasColumnType("decimal(10,2)");
                entity.Property(m => m.FecReg).HasColumnName("fec_reg");
                entity.Property(m => m.Obs).HasColumnName("obs").HasMaxLength(255);
                entity.Property(m => m.Local).HasColumnName("local").HasMaxLength(100);
                entity.Property(m => m.Umb).HasColumnName("umb").HasMaxLength(50);
                entity.Property(m => m.Parihuela).HasColumnName("parihuela").HasMaxLength(100);
                entity.Property(m => m.Ubicacion).HasColumnName("ubicacion").HasMaxLength(100);
                entity.Property(m => m.Fec).HasColumnName("fec");
                entity.Property(m => m.Cta).HasColumnName("cta").HasMaxLength(50);
                entity.Property(m => m.Usuario).HasColumnName("usuario").HasMaxLength(50);
                entity.Property(m => m.FecSys).HasColumnName("fec_sys");
                entity.Property(m => m.Estado).HasColumnName("estado").HasMaxLength(50);
            });

        }
    }
}
