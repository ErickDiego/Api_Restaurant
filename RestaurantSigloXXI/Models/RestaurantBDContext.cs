using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RestaurantSigloXXI.Models
{
    public partial class RestaurantBDContext : DbContext
    {
        public RestaurantBDContext()
        {
        }

        public RestaurantBDContext(DbContextOptions<RestaurantBDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Detalle> Detalle { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Mesa> Mesa { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public virtual DbSet<Seleccion> Seleccion { get; set; }
        public virtual DbSet<SeleccionProducto> SeleccionProducto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Venta> Venta { get; set; }

        // Unable to generate entity type for table 'dbo.INVENTARIO'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=RestaurantBD;Integrated Security=True;");
                 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Rut);

                entity.ToTable("CLIENTE");

                entity.Property(e => e.Rut)
                    .HasColumnName("RUT")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasColumnName("APELLIDO_MATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasColumnName("APELLIDO_PATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Detalle>(entity =>
            {
                entity.HasKey(e => e.IdDetalle);

                entity.ToTable("DETALLE");

                entity.Property(e => e.IdDetalle)
                    .HasColumnName("ID_DETALLE")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");

                entity.Property(e => e.IdSeleccion).HasColumnName("ID_SELECCION");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.Detalle)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALLE_SELECCION");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.ToTable("ESTADO");

                entity.Property(e => e.IdEstado)
                    .HasColumnName("ID_ESTADO")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.HasKey(e => e.IdMesa);

                entity.ToTable("MESA");

                entity.Property(e => e.IdMesa)
                    .HasColumnName("ID_MESA")
                    .ValueGeneratedNever();

                entity.Property(e => e.Capacidad).HasColumnName("CAPACIDAD");

                entity.Property(e => e.IdEstadoMesa).HasColumnName("ID_ESTADO_MESA");

                entity.Property(e => e.Ubicacion)
                    .IsRequired()
                    .HasColumnName("UBICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo);

                entity.ToTable("MODULO");

                entity.Property(e => e.IdModulo)
                    .HasColumnName("ID_MODULO")
                    .ValueGeneratedNever();

                entity.Property(e => e.NombreModulo)
                    .IsRequired()
                    .HasColumnName("NOMBRE_MODULO")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido);

                entity.ToTable("PEDIDO");

                entity.Property(e => e.IdPedido)
                    .HasColumnName("ID_PEDIDO")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdEstadoPedido).HasColumnName("ID_ESTADO_PEDIDO");

                entity.Property(e => e.IdMesa).HasColumnName("ID_MESA");

                entity.HasOne(d => d.IdMesaNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.IdMesa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PEDIDO_MESA");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil);

                entity.ToTable("PERFIL");

                entity.Property(e => e.IdPerfil)
                    .HasColumnName("ID_PERFIL")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdModulo).HasColumnName("ID_MODULO");

                entity.Property(e => e.NombrePefil)
                    .IsRequired()
                    .HasColumnName("NOMBRE_PEFIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdModuloNavigation)
                    .WithMany(p => p.Perfil)
                    .HasForeignKey(d => d.IdModulo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PERFIL_MODULO");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto);

                entity.ToTable("PRODUCTO");

                entity.Property(e => e.IdProducto)
                    .HasColumnName("ID_PRODUCTO")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.IdReserva);

                entity.ToTable("RESERVA");

                entity.Property(e => e.IdReserva)
                    .HasColumnName("ID_RESERVA")
                    .ValueGeneratedNever();

                entity.Property(e => e.CantidadPersonas).HasColumnName("CANTIDAD_PERSONAS");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("date");

                entity.Property(e => e.IdMesa).HasColumnName("ID_MESA");

                entity.Property(e => e.RutCliente).HasColumnName("RUT_CLIENTE");

                entity.HasOne(d => d.IdMesaNavigation)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.IdMesa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RESERVA_MESA");

                entity.HasOne(d => d.RutClienteNavigation)
                    .WithMany(p => p.Reserva)
                    .HasForeignKey(d => d.RutCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RESERVA_CLIENTE");
            });

            modelBuilder.Entity<Seleccion>(entity =>
            {
                entity.HasKey(e => e.IdSeleccion);

                entity.ToTable("SELECCION");

                entity.Property(e => e.IdSeleccion)
                    .HasColumnName("ID_SELECCION")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnName("VALOR");

                entity.HasOne(d => d.IdSeleccionNavigation)
                    .WithOne(p => p.Seleccion)
                    .HasForeignKey<Seleccion>(d => d.IdSeleccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SELECCION_SELECCION_PRODUCTO");
            });

            modelBuilder.Entity<SeleccionProducto>(entity =>
            {
                entity.HasKey(e => e.IdSeleccion);

                entity.ToTable("SELECCION_PRODUCTO");

                entity.Property(e => e.IdSeleccion)
                    .HasColumnName("ID_SELECCION")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");

                entity.Property(e => e.Unidades).HasColumnName("UNIDADES");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.SeleccionProducto)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SELECCION_PRODUCTO_PRODUCTO");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Rut);

                entity.ToTable("USUARIO");

                entity.Property(e => e.Rut)
                    .HasColumnName("RUT")
                    .ValueGeneratedNever();

                entity.Property(e => e.Activo).HasColumnName("ACTIVO");

                entity.Property(e => e.ApellidoMaterno)
                    .IsRequired()
                    .HasColumnName("APELLIDO_MATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno)
                    .IsRequired()
                    .HasColumnName("APELLIDO_PATERNO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdPerfilNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdPerfil)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIO_PERFIL");
            });

            modelBuilder.Entity<Venta>(entity =>
            {
                entity.HasKey(e => e.IdVenta);

                entity.ToTable("VENTA");

                entity.Property(e => e.IdVenta)
                    .HasColumnName("ID_VENTA")
                    .ValueGeneratedNever();

                entity.Property(e => e.FechaVenta)
                    .HasColumnName("FECHA_VENTA")
                    .HasColumnType("date");

                entity.Property(e => e.IdMesa).HasColumnName("ID_MESA");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO");

                entity.Property(e => e.MetodoPago)
                    .HasColumnName("METODO_PAGO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalAPagar).HasColumnName("TOTAL_A_PAGAR");

                entity.HasOne(d => d.IdMesaNavigation)
                    .WithMany(p => p.Venta)
                    .HasForeignKey(d => d.IdMesa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VENTA_MESA");
            });
        }
    }
}
