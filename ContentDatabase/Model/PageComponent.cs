using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageComponent : CreationDetails, Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key] public Guid Id { get; }
        [MaxLength(3000)]

        [ForeignKey(nameof(PageTemplate.Id))]
        [Required] public Guid TemplateId { get; set; }
        public string ComponentHtml { get; set; }
        //0 Based index where child insertion happens
        public int ChildOffset { get; set; }
        //0 Based on how many components before self
        public int SelfPageOrder { get; set; }
        public ICollection<AuthoredComponent> AuthoredComponent { get; set; } = new List<AuthoredComponent>();
        public PageTemplate PageTemplate { get; set; } = null!;
    }
}
