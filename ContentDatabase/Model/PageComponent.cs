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
        public string ComponentPosition { get; set; }
        public int ChildOffset { get; set; }
        public int SelfPageOrder { get; set; }
        public ICollection<AuthoredComponent> AuthoredComponent { get; set; } = new List<AuthoredComponent>();
        public ICollection<PageTemplate> PageTemplate { get; set; } = new List<PageTemplate>();
    }
}
