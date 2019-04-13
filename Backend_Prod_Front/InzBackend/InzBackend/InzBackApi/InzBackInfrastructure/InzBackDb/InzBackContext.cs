using InzBackCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace InzBackInfrastructure.InzBackDb
{
    public class InzBackContext : DbContext
    {
       
        public InzBackContext(DbContextOptions<InzBackContext> options)
            : base(options)
        {
        }
        public DbSet<Player> Players { get; set; }
        public DbSet<Matchh> Matches { get; set; }
        public DbSet<PlayersStatictics> PlrStats { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=InzBackDB28;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Player>()   
               .HasKey(x => x.Id);

            modelBuilder.Entity<Matchh>()   
                .HasKey(x => x.Id); 


            modelBuilder.Entity<PlayersStatictics>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<MatchhPlayer>()
                .HasKey(t => new { t.MatchhId, t.PlayerId });


            modelBuilder.Entity<Player>()
                            .HasOne(s => s.user)    
                            .WithMany(g => g.players)
                            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Matchh>()
                            .HasOne(s => s.user)
                            .WithMany(g => g.matches)
                            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MatchhPlayer>()
                            .HasOne(s => s.Matchh)
                            .WithMany(s => s.Players2Match)
                            .HasForeignKey(s => s.MatchhId);
     

            modelBuilder.Entity<MatchhPlayer>()
                            .HasOne(s => s.Player)
                            .WithMany(s => s.Matchhs2Player)
                            .HasForeignKey(s => s.PlayerId);
 

        }

    }
}




