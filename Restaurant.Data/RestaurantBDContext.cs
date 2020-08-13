using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Restaurant.Data
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

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Detalle> Detalles { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<EstadoMesa> EstadoMesas { get; set; }
        public virtual DbSet<Mesa> Mesas { get; set; }
        public virtual DbSet<Modulo> Modulos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Perfil> Perfiles { get; set; }
        public virtual DbSet<PerfilXModulo> PerfilXModulos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Reserva> Reservas{ get; set; }
        public virtual DbSet<Seleccion> Selecciones { get; set; }
        public virtual DbSet<SeleccionProducto> SeleccionProductos { get; set; }
        public virtual DbSet<Ubicacion> Ubicaciones { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Venta> Ventas { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Data Source=rgpro.database.windows.net;Initial Catalog=RestaurantBD;User ID=rgpro;Password=C6T=yCFLU.a.Gpt4;");
//            }
//        }

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

                entity.Property(e => e.IdDetalle).HasColumnName("ID_DETALLE");

                entity.Property(e => e.Cantidad).HasColumnName("CANTIDAD");

                entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");

                entity.Property(e => e.IdSeleccion).HasColumnName("ID_SELECCION");

                entity.HasOne(d => d.IdPedidoNavigation)
                    .WithMany(p => p.Detalle)
                    .HasForeignKey(d => d.IdPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALLE_SELECCION");

                entity.HasOne(d => d.IdSeleccionNavigation)
                    .WithMany(p => p.Detalle)
                    .HasForeignKey(d => d.IdSeleccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DETALLE_SELECCION1");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.ToTable("ESTADO");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EstadoMesa>(entity =>
            {
                entity.HasKey(e => e.IdEstado);

                entity.ToTable("ESTADO_MESA");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.DescripcionEstado)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION_ESTADO")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mesa>(entity =>
            {
                entity.HasKey(e => e.IdMesa);

                entity.ToTable("MESA");

                entity.Property(e => e.IdMesa).HasColumnName("ID_MESA");

                entity.Property(e => e.Capacidad).HasColumnName("CAPACIDAD");

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.Ubicacion).HasColumnName("UBICACION");

                entity.HasOne(d => d.EstadoNavigation)
                    .WithMany(p => p.Mesa)
                    .HasForeignKey(d => d.Estado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESA_ESTADO_MESA");

                entity.HasOne(d => d.UbicacionNavigation)
                    .WithMany(p => p.Mesa)
                    .HasForeignKey(d => d.Ubicacion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESA_UBICACION");
            });

            modelBuilder.Entity<Modulo>(entity =>
            {
                entity.HasKey(e => e.IdModulo);

                entity.ToTable("MODULO");

                entity.Property(e => e.IdModulo).HasColumnName("ID_MODULO");

                entity.Property(e => e.NombreModulo)
                    .IsRequired()
                    .HasColumnName("NOMBRE_MODULO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Icono)
                    .HasColumnName("ICONO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RutaModulo)
                    .HasColumnName("RUTA_MODULO")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.HasKey(e => e.IdPedido);

                entity.ToTable("PEDIDO");

                entity.Property(e => e.IdPedido).HasColumnName("ID_PEDIDO");

                entity.Property(e => e.IdEstadoPedido).HasColumnName("ID_ESTADO_PEDIDO");

                entity.Property(e => e.IdMesa).HasColumnName("ID_MESA");

                entity.HasOne(d => d.IdEstadoPedidoNavigation)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.IdEstadoPedido)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PEDIDO_ESTADO");

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

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.NombrePefil)
                    .IsRequired()
                    .HasColumnName("NOMBRE_PEFIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PerfilXModulo>(entity =>
            {
                entity.ToTable("PERFIL_X_MODULO");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdModulo).HasColumnName("ID_MODULO");

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.IdProducto)
                    .HasName("PK_T_PRODUCTO");

                entity.ToTable("PRODUCTO");

                entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");

                entity.Property(e => e.CantidadRecomendada).HasColumnName("CANTIDAD_RECOMENDADA");

                entity.Property(e => e.FechaUltimaReposicion)
                    .HasColumnName("FECHA_ULTIMA_REPOSICION")
                    .HasColumnType("date");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Stock).HasColumnName("STOCK");
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.IdReserva);

                entity.ToTable("RESERVA");

                entity.Property(e => e.IdReserva).HasColumnName("ID_RESERVA");

                entity.Property(e => e.CantidadPersonas).HasColumnName("CANTIDAD_PERSONAS");

                entity.Property(e => e.Fecha)
                    .HasColumnName("FECHA")
                    .HasColumnType("datetime");

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
                    .HasConstraintName("FK_RESERVA_CLIENTE");
            });

            modelBuilder.Entity<Seleccion>(entity =>
            {
                entity.HasKey(e => e.IdSeleccion);

                entity.ToTable("SELECCION");

                entity.Property(e => e.IdSeleccion).HasColumnName("ID_SELECCION");

                entity.Property(e => e.Imagen)
                    .HasColumnName("IMAGEN")
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tiempo).HasColumnName("TIEMPO");

                entity.Property(e => e.Valor).HasColumnName("VALOR");
            });

            modelBuilder.Entity<SeleccionProducto>(entity =>
            {
                entity.HasKey(e => e.IdSeleccionProducto);

                entity.ToTable("SELECCION_PRODUCTO");

                entity.Property(e => e.IdSeleccionProducto).HasColumnName("ID_SELECCION_PRODUCTO");

                entity.Property(e => e.IdProducto).HasColumnName("ID_PRODUCTO");

                entity.Property(e => e.IdSeleccion).HasColumnName("ID_SELECCION");

                entity.Property(e => e.Unidades).HasColumnName("UNIDADES");

                entity.HasOne(d => d.IdProductoNavigation)
                    .WithMany(p => p.SeleccionProducto)
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SELECCION_PRODUCTO_PRODUCTO");

                entity.HasOne(d => d.IdSeleccionNavigation)
                    .WithMany(p => p.SeleccionProducto)
                    .HasForeignKey(d => d.IdSeleccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SELECCION_PRODUCTO_SELECCION");
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.HasKey(e => e.IdUbicacion);

                entity.ToTable("UBICACION");

                entity.Property(e => e.IdUbicacion).HasColumnName("ID_UBICACION");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);
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

                entity.Property(e => e.Contrasena)
                    .HasColumnName("contrasena")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasColumnName("correo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPerfil).HasColumnName("ID_PERFIL");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("NOMBRE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NombreUsuario)
                    .HasColumnName("nombreUsuario")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
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

                entity.Property(e => e.IdVenta).HasColumnName("ID_VENTA");

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
