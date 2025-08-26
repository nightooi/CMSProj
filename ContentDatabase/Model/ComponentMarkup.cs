using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    public class ComponentMarkup : CreationDetails, Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        /// <summary>
        ///     Json Dump
        ///     
        /// SERIALIZATION AND DESERIALIZATION ARE PROVIDED.
        /// </summary>
        public string Markup { get; set; }
        public string Content { get; set; }
        public ICollection<AuthoredComponent> Pages { get; set; } = null!;
    }
}
