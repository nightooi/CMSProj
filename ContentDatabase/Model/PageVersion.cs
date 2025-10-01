﻿using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(Id))]
    public class PageVersion : CreationDetails, Id, IVersionable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; }
        [ForeignKey(nameof(Page.Id))]
        public Guid? PageId { get; set; }
        [ForeignKey(nameof(Page.Id))]
        public Guid? PageTemplateId { get; set; }
        public int Version { get; set; }
        public Page? Page { get; set; } = null!;
        public PageTemplate PageTemplate { get; set; } = null!;
        public ICollection<AuthoredComponent> Components { get; set; } = new List<AuthoredComponent>();
    }
}
