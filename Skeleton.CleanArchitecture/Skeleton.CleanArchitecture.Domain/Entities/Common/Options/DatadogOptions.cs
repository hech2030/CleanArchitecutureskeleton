using System.ComponentModel.DataAnnotations;

namespace Skeleton.CleanArchitecture.Domain.Entities.Common.Options
{
    public sealed class DatadogOptions
    {
        [Required]
        public required bool Enabled { get; set; } = true;

        [Required]
        public required string Environment { get; set; }

        [Required]
        public required string Version { get; set; }

        [Required]
        public required string Service { get; set; }

        [Required]
        public required string ProjectCode { get; set; }
    }
}
