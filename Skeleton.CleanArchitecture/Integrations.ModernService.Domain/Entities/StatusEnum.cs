namespace Integrations.ModernService.Domain.Entities;

public abstract class StatusEnum<TKey, TEnum> where TEnum : StatusEnum<TKey, TEnum>
{
    public TKey Code { get; }
    public string ExternalCode { get; }

    protected StatusEnum(TKey code, string externalCode)
    {
        Code = code;
        ExternalCode = externalCode;
    }

    public override string ToString() => ExternalCode;

    public override bool Equals(object? obj)
    {
        if (obj is not StatusEnum<TKey, TEnum> other)
            return false;
        return EqualityComparer<TKey>.Default.Equals(Code, other.Code);
    }

    public override int GetHashCode() => Code?.GetHashCode() ?? 0;

    public static bool operator ==(StatusEnum<TKey, TEnum>? left, StatusEnum<TKey, TEnum>? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(StatusEnum<TKey, TEnum>? left, StatusEnum<TKey, TEnum>? right) => !(left == right);
}