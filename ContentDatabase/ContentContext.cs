using ContentDatabase.Model;

using Microsoft.EntityFrameworkCore;

using System.Runtime.InteropServices;

namespace ContentDatabase
{
    /// <summary>
    /// Glaring issue is that if there's a searchbar, you kinda default to searching in the entire page...
    /// 
    /// Shape of the relation between the content and the tags is ill defined, or rather the shape of the authored component
    /// </summary>
    public class ContentContext : DbContext
    {
        public DbSet<AssetFileType> AssetFileTypes { get; set; }
        public DbSet<AssetHostDomain> AssetHostDomains { get; set; }
        public DbSet<Assets> Assets { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthoredComponent> AuthoredComponents { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageTemplate> PageTemplates { get; set; }
        public DbSet<PageVersion> PageVersions { get; set; }
        public DbSet<ComponentMarkup> ComponentMarkups { get; set; }
        public DbSet<PageComponent> PageComponents { get; set; }
        public ContentContext(DbContextOptions<ContentContext> opts) : base(opts)
        {

        }
        public ContentContext() : base()
        {

        }
    }
}
