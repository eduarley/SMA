using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SMA.Entities
{
    public partial class BD_SMAContext : DbContext
    {
        public BD_SMAContext()
        {
        }

        public BD_SMAContext(DbContextOptions<BD_SMAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TArchivo> TArchivos { get; set; }
        public virtual DbSet<TComprobante> TComprobantes { get; set; }
        public virtual DbSet<TDepartamento> TDepartamentos { get; set; }
        public virtual DbSet<TEstadoAceptacion> TEstadoAceptacions { get; set; }
        public virtual DbSet<TNotificacionGeneral> TNotificacionGenerals { get; set; }
        public virtual DbSet<TPrioridadTicket> TPrioridadTickets { get; set; }
        public virtual DbSet<TRevisionTicket> TRevisionTickets { get; set; }
        public virtual DbSet<TRol> TRols { get; set; }
        public virtual DbSet<TTicket> TTickets { get; set; }
        public virtual DbSet<TUsuario> TUsuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=BD_SMA;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<TArchivo>(entity =>
            {
                entity.HasKey(e => e.IdArchivo)
                    .HasName("PK__T_Archiv__44419580B22BCEEB");

                entity.ToTable("T_Archivos");

                entity.Property(e => e.IdArchivo).HasColumnName("id_archivo");

                entity.Property(e => e.Archivo)
                    .IsRequired()
                    .HasColumnName("archivo");

                entity.Property(e => e.Extension)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("extension");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.NumTicket).HasColumnName("num_ticket");

                entity.HasOne(d => d.NumTicketNavigation)
                    .WithMany(p => p.TArchivos)
                    .HasForeignKey(d => d.NumTicket)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_Archivos_T_Tickets");
            });

            modelBuilder.Entity<TComprobante>(entity =>
            {
                entity.HasKey(e => e.IdComprobante)
                    .HasName("PK__T_Compro__55E5E2408925C337");

                entity.ToTable("T_Comprobante");

                entity.Property(e => e.IdComprobante).HasColumnName("id_comprobante");

                entity.Property(e => e.EstadoFinal)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("estado_final");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("justificacion");

                entity.Property(e => e.NumTicket).HasColumnName("num_ticket");

                entity.Property(e => e.Retroalimentacion)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("retroalimentacion");

                entity.HasOne(d => d.NumTicketNavigation)
                    .WithMany(p => p.TComprobantes)
                    .HasForeignKey(d => d.NumTicket)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comprobante_Tickets");
            });

            modelBuilder.Entity<TDepartamento>(entity =>
            {
                entity.HasKey(e => e.IdDepartamento)
                    .HasName("PK__T_Depart__64F37A16719D4F20");

                entity.ToTable("T_Departamento");

                entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<TEstadoAceptacion>(entity =>
            {
                entity.HasKey(e => e.IdEstado)
                    .HasName("PK__T_Estado__86989FB2E7C34798");

                entity.ToTable("T_EstadoAceptacion");

                entity.Property(e => e.IdEstado).HasColumnName("id_estado");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tipo");
            });

            modelBuilder.Entity<TNotificacionGeneral>(entity =>
            {
                entity.HasKey(e => e.IdNotificacion)
                    .HasName("PK_T_NotificacionPush");

                entity.ToTable("T_NotificacionGeneral");

                entity.Property(e => e.IdNotificacion).HasColumnName("id_notificacion");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdEmisor).HasColumnName("id_emisor");

                entity.Property(e => e.IdReceptor).HasColumnName("id_receptor");

                entity.Property(e => e.NumTicket).HasColumnName("num_ticket");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.Property(e => e.Visto).HasColumnName("visto");

                entity.HasOne(d => d.IdEmisorNavigation)
                    .WithMany(p => p.TNotificacionGeneralIdEmisorNavigations)
                    .HasForeignKey(d => d.IdEmisor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_NotificacionGeneral_T_Usuarios");

                entity.HasOne(d => d.IdReceptorNavigation)
                    .WithMany(p => p.TNotificacionGeneralIdReceptorNavigations)
                    .HasForeignKey(d => d.IdReceptor)
                    .HasConstraintName("FK_T_NotificacionGeneral_T_Usuarios1");

                entity.HasOne(d => d.NumTicketNavigation)
                    .WithMany(p => p.TNotificacionGenerals)
                    .HasForeignKey(d => d.NumTicket)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_NotificacionGeneral_T_Tickets");
            });

            modelBuilder.Entity<TPrioridadTicket>(entity =>
            {
                entity.HasKey(e => e.IdPrioridad)
                    .HasName("PK__T_Priori__EF3DAB40D2A458B0");

                entity.ToTable("T_PrioridadTickets");

                entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");

                entity.Property(e => e.TipoPrioridad)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tipo_prioridad");
            });

            modelBuilder.Entity<TRevisionTicket>(entity =>
            {
                entity.HasKey(e => e.IdRevision);

                entity.ToTable("T_RevisionTicket");

                entity.Property(e => e.IdRevision).HasColumnName("id_revision");

                entity.Property(e => e.FechaRevision)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_revision");

                entity.Property(e => e.IdEstado).HasColumnName("id_estado");

                entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasColumnName("observacion");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.TRevisionTickets)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_RevisionTicket_T_EstadoAceptacion");

                entity.HasOne(d => d.IdPrioridadNavigation)
                    .WithMany(p => p.TRevisionTickets)
                    .HasForeignKey(d => d.IdPrioridad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_RevisionTicket_T_PrioridadTickets");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TRevisionTickets)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_RevisionTicket_T_Usuarios");
            });

            modelBuilder.Entity<TRol>(entity =>
            {
                entity.HasKey(e => e.IdRol)
                    .HasName("PK__T_Rol__6ABCB5E01B7E4990");

                entity.ToTable("T_Rol");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.TipoRol)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("tipo_rol");
            });

            modelBuilder.Entity<TTicket>(entity =>
            {
                entity.HasKey(e => e.NumTicket)
                    .HasName("PK__T_Ticket__94E9292A538BBDB1");

                entity.ToTable("T_Tickets");

                entity.Property(e => e.NumTicket).HasColumnName("num_ticket");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(600)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.IdPrioridad).HasColumnName("id_prioridad");

                entity.Property(e => e.IdRevision).HasColumnName("id_revision");

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Revisado).HasColumnName("revisado");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("titulo");

                entity.HasOne(d => d.IdPrioridadNavigation)
                    .WithMany(p => p.TTickets)
                    .HasForeignKey(d => d.IdPrioridad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TICKETS_PRIORIDAD");

                entity.HasOne(d => d.IdRevisionNavigation)
                    .WithMany(p => p.TTickets)
                    .HasForeignKey(d => d.IdRevision)
                    .HasConstraintName("FK_T_Tickets_T_RevisionTicket");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TTickets)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TICKETS_USUARIOS");
            });

            modelBuilder.Entity<TUsuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__T_Usuari__4E3E04AD4FFE9787");

                entity.ToTable("T_Usuarios");

                entity.HasIndex(e => e.Cedula, "Cedula_Unique")
                    .IsUnique();

                entity.HasIndex(e => e.CorreoElectronico, "Correo_Unique")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Cedula)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("cedula");

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false)
                    .HasColumnName("clave");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("correo_electronico");

                entity.Property(e => e.Estado).HasColumnName("estado");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.Foto).HasColumnName("foto");

                entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");

                entity.Property(e => e.IdRol).HasColumnName("id_rol");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.PrimerIngreso).HasColumnName("primer_ingreso");

                entity.Property(e => e.Token)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.HasOne(d => d.IdDepartamentoNavigation)
                    .WithMany(p => p.TUsuarios)
                    .HasForeignKey(d => d.IdDepartamento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIOS_DEPARTAMENTO");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.TUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USUARIOS_ROL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
