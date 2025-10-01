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
        public DbSet<PageSlug> PageSlugs { get; set; }
        public DbSet<PublishedPageSlug> PublishedPages { get; set; }
        public DbSet<AssetPageComponent> AssetComponentJoinTable { get; set; }
        public DbSet<AuthoredAssetJoin> AssetAuthoredJoinTable { get; set; }
        public ContentContext(DbContextOptions<ContentContext> opts) : base(opts)
        {

        }
        public ContentContext() : base()
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PageComponent>()
                .HasOne(x => x.PageTemplate)
                .WithMany(x => x.PageComponents)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<PageVersion>()
                .HasOne(x => x.PageTemplate)
                .WithMany(x => x.PageVersions)
                .HasForeignKey(x => x.PageTemplateId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<PageVersion>()
                .HasOne(x => x.Page)
                .WithMany(x => x.PageVersions)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<PageComponent>()
                .HasMany(x => x.Assets)
                .WithMany(x => x.Components)
                .UsingEntity<AssetPageComponent>(
                x => x.HasOne<Assets>(x => x.Asset).WithMany().HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.NoAction).IsRequired(false),
                y => y.HasOne<PageComponent>(x => x.Component).WithMany().HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.NoAction).IsRequired(false));

            modelBuilder.Entity<AuthoredComponent>()
                .HasOne(x => x.PayLoad)
                .WithMany(x => x.Pages)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AuthoredComponent>()
                .HasOne(x => x.PageComponent)
                .WithMany(x => x.AuthoredComponent)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuthoredComponent>()
                .HasOne(x => x.PageVersion)
                .WithMany(x => x.Components)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<PublishedPageSlug>()
                .HasOne(x => x.Page)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PublishedPageSlug>()
                .HasOne(x => x.PageVersion)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PublishedPageSlug>()
                .HasOne(x => x.Slug)
                .WithOne()
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade)
                .HasPrincipalKey<PageSlug>(x=> x.Id);

            modelBuilder.Entity<AuthoredComponent>()
                .HasMany(x => x.Assets)
                .WithMany(x => x.Page)
                .UsingEntity<AuthoredAssetJoin>(
                x=> x.HasOne<Assets>().WithMany().IsRequired(false).OnDelete(DeleteBehavior.Cascade),
                y => y.HasOne<AuthoredComponent>().WithMany().IsRequired(false).OnDelete(DeleteBehavior.NoAction),
                z => z.HasAlternateKey(x=> x.Asset));
        }
    }
}
