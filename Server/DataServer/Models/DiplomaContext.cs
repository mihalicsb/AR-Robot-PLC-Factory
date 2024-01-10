using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Models;

public partial class DiplomaContext : DbContext
{
    public DiplomaContext()
    {
    }

    public DiplomaContext(DbContextOptions<DiplomaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Factory> Factories { get; set; }

    public virtual DbSet<IoPort> IoPorts { get; set; }

    public virtual DbSet<Plc> Plcs { get; set; }

    public virtual DbSet<PlcType> PlcTypes { get; set; }

    public virtual DbSet<Robot> Robots { get; set; }

    public virtual DbSet<RobotType> RobotTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=diploma", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.28-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Factory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("factory");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Width).HasColumnName("width");
        });

        modelBuilder.Entity<IoPort>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("io_port");

            entity.HasIndex(e => e.PlcId, "FK1_plc_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Bit)
                .HasColumnType("int(11)")
                .HasColumnName("bit");
            entity.Property(e => e.Direction)
                .HasComment("0 - In,   1-Out")
                .HasColumnType("tinyint(4)")
                .HasColumnName("direction");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Offset)
                .HasColumnType("int(11)")
                .HasColumnName("offset");
            entity.Property(e => e.PlcId)
                .HasColumnType("int(11)")
                .HasColumnName("plc_id");
            entity.Property(e => e.Value)
                .HasColumnType("int(11)")
                .HasColumnName("value");

            entity.HasOne(d => d.Plc).WithMany(p => p.IoPorts)
                .HasForeignKey(d => d.PlcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1_plc_id");
        });

        modelBuilder.Entity<Plc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plc");

            entity.HasIndex(e => e.TypeId, "FK1_plc_type_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.DbNumber)
                .HasColumnType("int(11)")
                .HasColumnName("db_number");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.TypeId)
                .HasColumnType("int(11)")
                .HasColumnName("type_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Plcs)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1_plc_type_id");
        });

        modelBuilder.Entity<PlcType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plc_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Robot>(entity =>
        {
            entity.HasKey(e => e.Guid).HasName("PRIMARY");

            entity.ToTable("robot");

            entity.HasIndex(e => e.TypeId, "FK1_robot_type_id");

            entity.HasIndex(e => e.FactoryId, "FK2_factor_id");

            entity.Property(e => e.Guid)
                .HasMaxLength(36)
                .HasDefaultValueSql("'0'")
                .HasColumnName("guid");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Alias)
                .HasColumnType("text")
                .HasColumnName("alias");
            entity.Property(e => e.FactoryId)
                .HasColumnType("int(11)")
                .HasColumnName("factory_id");
            entity.Property(e => e.HostName)
                .HasColumnType("text")
                .HasColumnName("host_name");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.SystemName)
                .HasColumnType("text")
                .HasColumnName("system_name");
            entity.Property(e => e.TypeId)
                .HasColumnType("int(11)")
                .HasColumnName("type_id");
            entity.Property(e => e.Version)
                .HasColumnType("text")
                .HasColumnName("version");
            entity.Property(e => e.Virtual)
                .HasComment("0 virtual, 1 real")
                .HasColumnType("tinyint(4)")
                .HasColumnName("virtual");
            entity.Property(e => e.Xpos).HasColumnName("xpos");
            entity.Property(e => e.Ypos).HasColumnName("ypos");
            entity.Property(e => e.ZOrinetation).HasColumnName("z_orinetation");

            entity.HasOne(d => d.Factory).WithMany(p => p.Robots)
                .HasForeignKey(d => d.FactoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK2_factor_id");

            entity.HasOne(d => d.Type).WithMany(p => p.Robots)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1_robot_type_id");
        });

        modelBuilder.Entity<RobotType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("robot_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
