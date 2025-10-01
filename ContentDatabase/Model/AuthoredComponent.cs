using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class AuthoredComponent : CreationDetails, Id, IComponentPeripheral
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(PageComponent.Id))]
        [AllowNull]
        public Guid? PageComponentId { get; set; }
        [ForeignKey(nameof(PageVersion.Id))]
        [AllowNull]
        public Guid? PageVersionId { get; set; }
        public ComponentMarkup? PayLoad { get; set; }
        [MaxLength(1000)]
        //Use Name of Author, Component Position and descriptive Name and Version
        [Required] public string ComponentName { get; set; }
        //Raw Html CSS to be included in the header
        public string? CssHeaderTags { get; set; }
        //Raw Html, Tag or populated script tag.
        public string? JsHeaderTags { get; set; }
        //Jstag or populated script tag.
        public string? JsBodyTags { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Version { get; set; }
        [AllowNull]
        public PageVersion? PageVersion { get; set; } = null!;
        [AllowNull]
        public PageComponent? PageComponent { get; set; } = null!;
        public ICollection<Assets>? Assets { get; set; } = new List<Assets>();
        public string? OtherHeaders { get; set; }
    }
}
