using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccess
{
    public partial class FuturamaContext : DbContext
    {
        public FuturamaContext(DbContextOptions<FuturamaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<Character> Characters { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Character>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Planet).HasMaxLength(50);

                entity.Property(e => e.Species).HasMaxLength(50);

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Characters)
                    .HasForeignKey(d => d.ActorId)
                    .HasConstraintName("FK_Characters_Actors");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
