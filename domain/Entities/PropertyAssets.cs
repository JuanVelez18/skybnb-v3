﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace domain.Entities
{
    [Index(nameof(Url), IsUnique = true)]
    public class PropertyAssets
    {
        public Guid Id { get; set; }

        [Required]
        public Uri? Url { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? Order { get; set; }

        public Guid PropertyId { get; set; }


        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;


        public Properties? Property { get; set; }
    }
}
