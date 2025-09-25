using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageComponent : CreationDetails, Id, IComponentPeripheral
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key] public Guid Id { get; }
        [MaxLength(3000)]

        //Raw Html Render at the base Page body.
        public string ComponentHtml { get; set; }
        //0 Based index where child insertion happens
        public int ChildOffset { get; set; }
        //0 Based on how many components before self
        public int SelfPageOrder { get; set; }
        public virtual ICollection<AuthoredComponent>? AuthoredComponent { get; set; } = new List<AuthoredComponent>();
        public PageTemplate? PageTemplate { get; set; } = null!;
        public string? CssHeaderTags { get ; set ; }
        public string? JsHeaderTags { get ; set ; }
        public string? JsBodyTags { get ; set ; }
        /// <summary>
        /// If the Assets consumed by the component are not added to the db, they might not load at runtime
        /// </summary>
        public virtual ICollection<Assets>? Assets { get; set; } = [];
        public string? OtherHeaders { get; set; }
    }
}
