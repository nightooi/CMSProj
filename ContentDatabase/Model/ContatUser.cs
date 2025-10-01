using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDatabase.Model
{
    [Index(nameof(ContatUser.Id))]
    public class ContatUser
    {
        [Key]
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }
        [MaxLength(14)]
        public string? PhoneNumber { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Message { get; set; }
        public DateTime CreatedUtc { get; set; }
    }
}
