namespace Auth1796.Core.Application.Utilities;

public static class EnumExtensions
{
    public static string ToFriendlyString<TEnum>(this TEnum value) where TEnum : Enum
    {
        return value.ToString();
    }

    public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = true) where TEnum : struct, Enum
    {
        if (Enum.TryParse(value, ignoreCase, out TEnum result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"'{value}' is not a valid value for type {typeof(TEnum).Name}");
        }
    }
}