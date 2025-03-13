namespace Skeleton.CleanArchitecture.Domain.Entities.Common.Options
{
    public sealed class FusionCacheConfiguration
    {
        public static readonly string DefaultFusionCacheConfiguration = nameof(DefaultFusionCacheConfiguration);
        public static readonly string CustomRoutingCacheOptions = nameof(CustomRoutingCacheOptions);
        public required TimeSpan Duration { get; set; }
        public required TimeSpan FailSafeMaxDuration { get; set; }
        public required TimeSpan FailSafeThrottleDuration { get; set; }
        public required TimeSpan JitterMaxDuration { get; set; }
    }
}
