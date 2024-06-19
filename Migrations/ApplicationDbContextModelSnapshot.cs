﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetDexQL.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace netdexQL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NetDexQL.Data.Models.Pokemon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int?>("BaseExperience")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "base_experience");

                    b.Property<int>("Height")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "height");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<int>("Order")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "order");

                    b.Property<int>("Weight")
                        .HasColumnType("integer")
                        .HasAnnotation("Relational:JsonPropertyName", "weight");

                    b.HasKey("Id");

                    b.ToTable("Pokemon");
                });
#pragma warning restore 612, 618
        }
    }
}
