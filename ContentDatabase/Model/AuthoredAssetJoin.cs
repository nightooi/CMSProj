using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class AuthoredAssetJoin
    {
        [ForeignKey(nameof(CompAsset.Id))]
        public Guid Asset { get; set; }
        [ForeignKey(nameof(Comp.Id))]
        public Guid AuthoredComp { get; set; }
        public AuthoredComponent? Comp { get; set; } = null!;
        public Assets? CompAsset { get; set; } = null!;
    }
}
