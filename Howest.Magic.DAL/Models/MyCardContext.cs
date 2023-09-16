using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Howest.MagicCards.DAL.Models
{
    public partial class MyCardContext : DbContext
    {
        public MyCardContext()
        {

        }

        public MyCardContext(DbContextOptions<MyCardContext> options) : base(options)
        {
        }

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Rarity> Rarities { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<Colors> Colors { get; set; }
        public virtual DbSet<CardColors> CardColors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__cards__3213E83F03749037");

                entity.ToTable("cards");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("Name");

                entity.Property(e => e.ManaCost)
                    .HasMaxLength(50)
                    .HasColumnName("Mana_Cost");

                entity.Property(e => e.ConvertedManaCost)
                    .HasColumnName("Converted_Mana_Cost");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .HasColumnName("Type");

                entity.Property(e => e.RarityCode)
                    .HasMaxLength(10)
                    .HasColumnName("Rarity_Code");

                entity.Property(e => e.SetCode)
                    .HasMaxLength(50)
                    .HasColumnName("set_code");

                entity.Property(e => e.Text)
                    .HasColumnName("Text");

                entity.Property(e => e.Flavor)
                    .HasColumnName("Flavor");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("Artist_Id");

                entity.Property(e => e.Number)
                    .HasMaxLength(10)
                    .HasColumnName("Number");

                entity.Property(e => e.Power)
                    .HasMaxLength(10)
                    .HasColumnName("Power");

                entity.Property(e => e.Toughness)
                    .HasMaxLength(10)
                    .HasColumnName("Toughness");

                entity.Property(e => e.Layout)
                    .HasMaxLength(50)
                    .HasColumnName("Layout");

                entity.Property(e => e.MultiverseId)
                    .HasColumnName("Multiverse_Id");

                entity.Property(e => e.OriginalImageUrl)
                    .HasMaxLength(500)
                    .HasColumnName("Original_Image_Url");

                entity.Property(e => e.Image)
                    .HasMaxLength(500)
                    .HasColumnName("Image");

                entity.Property(e => e.OriginalText)
                    .HasColumnName("Original_Text");

                entity.Property(e => e.OriginalType)
                    .HasColumnName("Original_Type");

                entity.Property(e => e.MtgId)
                    .HasColumnName("Mtg_Id");

                entity.Property(e => e.Variations)
                    .HasColumnName("Variations");

                entity.Property(e => e.Created_At)
                    .HasColumnName("Created_At");

                entity.Property(e => e.Updated_At)
                    .HasColumnName("Updated_At");

                entity.HasMany(e => e.CardColor)
                   .WithOne(p => p.Card)
                   .HasConstraintName("FK_card_colors_cards");
            });



            modelBuilder.Entity<Set>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__sets__3213E83F0F72A017");

                entity.ToTable("sets");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("Code");

                entity.Property(e => e.SetName)
                    .HasMaxLength(250)
                    .HasColumnName("Name");

                entity.Property(e => e.Created_At)
                    .HasColumnName("Created_At");

                entity.Property(e => e.Updated_At)
                    .HasColumnName("Updated_At");
            });

            modelBuilder.Entity<Rarity>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK__rarities__3213E83F710C9509");

                entity.ToTable("rarities");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("Code");

                entity.Property(e => e.RarityName)
                    .HasMaxLength(250)
                    .HasColumnName("Name");

                entity.Property(e => e.Created_At)
                    .HasColumnName("Created_At");

                entity.Property(e => e.Updated_At)
                    .HasColumnName("Updated_At");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__artists__3213E83FC867953C");

                entity.ToTable("artists");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .HasColumnName("full_name");

                entity.Property(e => e.Created_At)
                    .HasColumnName("Created_At");

                entity.Property(e => e.Updated_At)
                    .HasColumnName("Updated_At");

            });

            modelBuilder.Entity<CardColors>(entity =>
            {
                entity.HasKey(e => new { e.CardId, e.ColorId })
                    .HasName("card_colors_card_id_color_id_primary");

                entity.ToTable("card_colors");

                entity.Property(e => e.CardId).HasColumnName("card_id");

                entity.Property(e => e.ColorId).HasColumnName("color_id");

                entity.HasOne(e => e.Colors)
                   .WithMany(e => e.CardColor)
                   .HasForeignKey(e => e.ColorId)
                   .HasConstraintName("FK_card_colors_colors");

                entity.HasOne(e => e.Card)
                   .WithMany(e => e.CardColor)
                   .HasConstraintName("FK_card_colors_cards");
            });

            modelBuilder.Entity<Colors>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__colors__3213E83F2D3B99AB");

                entity.ToTable("colors");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(250)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .HasColumnName("name");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
