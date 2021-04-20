using System;
using System.Data.Entity;
using System.Linq;
using WcfServiceLibrary2.Classes;

namespace WcfServiceLibrary2
{
    public class Context : DbContext
    {
        // Контекст настроен для использования строки подключения "Context" из файла конфигурации  
        // приложения (App.config или Web.config). По умолчанию эта строка подключения указывает на базу данных 
        // "WcfServiceLibrary2.Context" в экземпляре LocalDb. 
        // 
        // Если требуется выбрать другую базу данных или поставщик базы данных, измените строку подключения "Context" 
        // в файле конфигурации приложения.
        public Context()
            : base("name=Context")
        {
        }

        // Добавьте DbSet для каждого типа сущности, который требуется включить в модель. Дополнительные сведения 
        // о настройке и использовании модели Code First см. в статье http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Datings> Datings { get; set; }
        public virtual DbSet<Filters> Filters { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Likes> Likes { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}