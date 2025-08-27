using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class AssetHostDomain
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        public string DomainName { get; set; }
        public string DomainUrl { get; set; }
        public ICollection<Assets> Assets { get; set; } = new List<Assets>();
    }
}
