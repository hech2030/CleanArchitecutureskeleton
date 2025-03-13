namespace Msc.DF.CommercialSchedules.Domain.Entities.Mappings;
public class GetLocationMappingParams
{
    public bool IsPort { get; set; }
    public string? LocationName { get; set; }
    public string? PortUNCode { get; set; }
    public string? EquipmentHandlingFacilitySmdgCode { get; set; }
    public int? PortId { get; set; }
    public string? PortName { get; set; }
    public string FallbackLocation { get; set; } = string.Empty;
}
