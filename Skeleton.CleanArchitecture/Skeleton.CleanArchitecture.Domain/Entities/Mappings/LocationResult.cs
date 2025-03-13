namespace Skeleton.CleanArchitecture.Domain.Entities.Mappings
{
    public record LocationResult(string FacilityCodeProvider, string FacilityCode, string UnLocationCode)
    {
        public bool IsNotValidLocation()
        {
            return string.IsNullOrWhiteSpace(FacilityCodeProvider) && string.IsNullOrWhiteSpace(UnLocationCode);
        }
    }
}
