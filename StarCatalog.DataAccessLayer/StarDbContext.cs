using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StarCatalog.Core;

namespace StarCatalog.DataAccessLayer
{
    
    /// <summary>
    /// Обеспечивает доступ Entity Framework к базе данных каталога звезд.
    /// </summary>
    [DbConfigurationType(typeof(StarDbConfiguration))]
    public class StarDbContext : DbContext
    {
        /// <summary>
        /// Создает контекст с указанной строкой подключения.
        /// </summary>
        /// <param name="connectionString">
        /// Строка подключения к базе данных SQL Server.
        /// </param>
        public StarDbContext(string connectionString)
            : base(connectionString)
        {
            System.Data.Entity.Database
                .SetInitializer<StarDbContext>(null);
        }

        /// <summary>
        /// Предоставляет доступ к записям таблицы звезд.
        /// </summary>
        public DbSet<Star> Stars { get; set; }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Star>().ToTable("Stars", "dbo");

            modelBuilder.Entity<Star>().HasKey(star => star.Id);

            modelBuilder.Entity<Star>()
                .Property(star => star.Name)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Star>()
                .Property(star => star.Discoverer)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Star>()
                .Property(star => star.StarType)
                .IsRequired()
                .HasMaxLength(100);

        }
    }
}
