using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class AuthoredComponent : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(PageComponent.Id))]
        public Guid PageComponentId { get; set; }
        [ForeignKey(nameof(PageVersion.Id))]
        public Guid VersionId { get; set; }
        public string PayLoad { get; set; }
        [MaxLength(1000)]
        //Use Name of Author, Component Position and descriptive Name and Version
        [Required] public string ComponentName { get; set; }
        public string? CssUrl { get; set; }
        public string? JsUrl { get; set; }
        public string? HeaderJsUrl { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? Version { get; set; }
        public PageVersion PageVersion { get; set; } = null!;
        public PageComponent PageComponent { get; set; } = null!;
    }
}
