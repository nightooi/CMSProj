using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class ComponentMarkup : CreationDetails, Id
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        /// <summary>
        ///     Json Dump
        ///     
        /// serialization and deserialization types are provided. under clrtypes
        /// </summary>
        public string Markup { get; set; }
        /// <summary>
        /// do no for the life of me insert html, js or css here, this is direct enduser visible content of the markup.
        /// this will be searched during search queries from the enduser.
        /// 
        /// </summary>
        public string Content { get; set; }
        public ICollection<AuthoredComponent> Pages { get; set; } = null!;
    }
}
