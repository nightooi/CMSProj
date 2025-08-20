using ContentDatabase.Model;

using Microsoft.EntityFrameworkCore;

using System.Runtime.InteropServices;

namespace ContentDatabase
{
    public class ContentContext : DbContext
    {
        private string connectionString { get; set; }
        public ContentContext(string connString) : base()
        {
            connectionString = connString;
        }

        public DbSet<AssetFileType> AssetFileTypes { get; set; }
        public DbSet<AssetHostDomain> AssetHostDomains { get; set; }
        public DbSet<Assets> Assets { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthoredComponent> AuothoredComponents { get; set; }
        public DbSet<CreationDetails> CreationDetails { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageTemplate> PageTemplates { get; set; }
        public DbSet<PageVersion> PageVersions { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
