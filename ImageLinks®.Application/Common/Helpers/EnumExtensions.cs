using static ImageLinks_.Application.Common.Enums.GeneralEnums;
namespace ImageLinks_.Application.Common.Helpers;

public static class EnumExtensions
{
    public static string ToId(this USERS value)
        => ((int)value).ToString();
}
