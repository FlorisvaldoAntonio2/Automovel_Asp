// <auto-generated />
using Automovel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Automovel.Migrations
{
    [DbContext(typeof(BancoContext))]
    [Migration("20220301173415_fots")]
    partial class fots
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Automovel.Models.AutomovelModel", b =>
                {
                    b.Property<int>("IdAutomovel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ano")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Cor")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Foto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdMontadora")
                        .HasColumnType("int");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Portas")
                        .HasColumnType("int");

                    b.Property<double>("Quilometragem")
                        .HasColumnType("float");

                    b.HasKey("IdAutomovel");

                    b.HasIndex("IdMontadora");

                    b.ToTable("Automoveis");
                });

            modelBuilder.Entity("Automovel.Models.MontadoraModel", b =>
                {
                    b.Property<int>("IdMontadora")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("IdMontadora");

                    b.ToTable("Montadoras");
                });

            modelBuilder.Entity("Automovel.Models.AutomovelModel", b =>
                {
                    b.HasOne("Automovel.Models.MontadoraModel", "Montadora")
                        .WithMany()
                        .HasForeignKey("IdMontadora")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Montadora");
                });
#pragma warning restore 612, 618
        }
    }
}
