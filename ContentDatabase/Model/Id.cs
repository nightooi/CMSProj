using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public interface Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
    }
}
