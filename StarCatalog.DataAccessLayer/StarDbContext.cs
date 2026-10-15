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
            // base(connectionString) передает строку подключения
            // конструктору родительского класса DbContext.
            //
            // Здесь мы не открываем окно и не выводим сообщения.
            // Контекст относится к работе с данными, а не к интерфейсу.
            // Таблица создается нашим SQL-скриптом.
            // Поэтому отключаем автоматическую инициализацию базы через EF.
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
            // Вызываем базовую настройку модели Entity Framework.
            base.OnModelCreating(modelBuilder);

            // Явно указываем название таблицы и схему.
            // Позже Dapper будет обращаться к той же таблице dbo.Stars.
            modelBuilder.Entity<Star>().ToTable("Stars", "dbo");

            // Указываем, какое свойство является первичным ключом.
            // Первичный ключ однозначно определяет запись в таблице.
            modelBuilder.Entity<Star>().HasKey(star => star.Id);

            // Название обязательно.
            // Ограничиваем его длину 200 символами.
            modelBuilder.Entity<Star>()
                .Property(star => star.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Сведения о первооткрывателе также обязательны.
            modelBuilder.Entity<Star>()
                .Property(star => star.Discoverer)
                .IsRequired()
                .HasMaxLength(200);

            // Тип звезды обязателен.
            modelBuilder.Entity<Star>()
                .Property(star => star.StarType)
                .IsRequired()
                .HasMaxLength(100);

            // Radius имеет тип double.
            // Его числовой тип EF определит самостоятельно.
            //
            // Проверки "радиус больше нуля" и "строка не из пробелов"
            // остаются в бизнес-логике. IsRequired их не заменяет.
        }
    }
}
