using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Models;

public partial class MydatabaseContext : DbContext
{
    public MydatabaseContext()
    {
    }

    public MydatabaseContext(DbContextOptions<MydatabaseContext> options)
        : base(options)
    {
    }

    

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.ToTable("ShoppingList");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.ItemName).HasMaxLength(450);
            entity.Property(e => e.ShopName).HasMaxLength(450);
            entity.Property(e => e.UserId).HasMaxLength(450);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
