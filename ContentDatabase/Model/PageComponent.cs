using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageComponent : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [MaxLength(300)]
        [Key] public string ComponentPosition { get; set; }
        public ICollection<AuthoredComponent> AuthoredComponent { get; set; } = new List<AuthoredComponent>();
        public ICollection<PageTemplate> PageTemplate { get; set; } = new List<PageTemplate>();
    }
}
