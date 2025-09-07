﻿using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [PrimaryKey(nameof(SlugId))]
    public class PublishedPageSlug
    {
        [ForeignKey(nameof(Slug.Id))]
        [Required]public Guid SlugId { get; set; }
        public PageSlug Slug { get; set; } = null!;
        [ForeignKey(nameof(Page.Id))]
        [Required] public Guid PageId { get; set; }
        public Page Page { get; set; } = null!;
        [ForeignKey(nameof(PageVersion.Id))]
        [Required]public Guid PageVersionId { get; set; }
        public PageVersion PageVersion { get; set; } = null!;
        public DateTime PublishedAt { get; set; }
    }
}
