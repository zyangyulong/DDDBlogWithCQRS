﻿// <auto-generated />
using System;
using Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations.SqlServerD3BlogDbMigrations
{
    [DbContext(typeof(D3BlogDbContext))]
    partial class D3BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime");

                    b.Property<int>("AddUserId");

                    b.Property<int?>("ArticleCategoryId")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Author")
                        .HasMaxLength(120)
                        .IsUnicode(true);

                    b.Property<int>("CollectedCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("ContentHtml")
                        .IsRequired()
                        .HasColumnType("text")
                        .IsUnicode(true);

                    b.Property<string>("ContentMd")
                        .IsRequired()
                        .HasColumnType("text")
                        .IsUnicode(true);

                    b.Property<string>("ExternalUrl")
                        .HasMaxLength(300);

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(300)
                        .IsUnicode(true);

                    b.Property<bool>("IsPublish")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsRed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsSlide")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsTop")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("ModifyTime");

                    b.Property<int?>("ModifyUserId");

                    b.Property<int>("PromitCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("SeoDescription")
                        .HasMaxLength(250)
                        .IsUnicode(true);

                    b.Property<string>("SeoKeyword")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<string>("SeoTitle")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<int?>("Source")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(300)
                        .IsUnicode(false);

                    b.Property<int?>("VerifyUserId");

                    b.Property<int>("ViewCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("ArticleCategoryId");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.ArticleCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icon")
                        .HasMaxLength(50)
                        .IsUnicode(true);

                    b.Property<bool>("IsDelete");

                    b.Property<int?>("ParentId");

                    b.Property<string>("SeoDes")
                        .HasMaxLength(200)
                        .IsUnicode(true);

                    b.Property<string>("SeoKeywords")
                        .HasMaxLength(120)
                        .IsUnicode(true);

                    b.Property<string>("SeoTitle")
                        .HasMaxLength(120)
                        .IsUnicode(true);

                    b.Property<int>("Sort");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(120)
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("ArticleCategory");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnName("Birthday");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasColumnName("Name")
                        .HasMaxLength(200);

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("D3.Blog.Domain.Entitys.Article", b =>
                {
                    b.HasOne("D3.Blog.Domain.Entitys.ArticleCategory", "ArticleCategory")
                        .WithMany("Article")
                        .HasForeignKey("ArticleCategoryId")
                        .HasConstraintName("FK_Products_Categories")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
