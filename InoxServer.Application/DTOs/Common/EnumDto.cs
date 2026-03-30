namespace InoxServer.Application.DTOs.Common;

public class EnumDto<TEnum>(int code, string name) where TEnum : struct, Enum
{
    public int Code { get; set; } = code;
    public string Name { get; set; } = name;

    public static EnumDto<TEnum> From(TEnum value)
    {
        return new EnumDto<TEnum>((int)(object)value!, value.ToString());
    }
}
