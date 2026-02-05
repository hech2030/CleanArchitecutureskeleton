namespace Integrations.ModernService.Domain.Entities;

public class OrganizationSetting
{
    public int Id { get; set; }
    public string SettingName { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
}