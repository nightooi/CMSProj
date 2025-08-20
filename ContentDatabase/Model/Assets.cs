using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [Index(nameof(Url), IsUnique =true)]
    public class Assets : CreationDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(AssetFileType.Id))]
        public Guid AssetFileTypeId { get; set; }
        [MaxLength(2000)]
        [Required] public string Url { get; set; }
        public string AssetName { get; set; }
        [ForeignKey(nameof(AssetFileType.FileType))]
        public string FileType { get; set; }
        public AssetHostDomain AssetDomain { get; set; } = null!;
        public AssetFileType AssetFileType { get; set; } = new();
    }
}
