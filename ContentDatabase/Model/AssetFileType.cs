﻿using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentDatabase.Model
{
    [Index(nameof(FileType), IsUnique =true)]
    [PrimaryKey(nameof(Id))]
    public class AssetFileType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }
        [MaxLength(20)]
        public string FileType { get; set; }
        public ICollection<Assets>? Assets { get; set; } = null!;
    }
}
