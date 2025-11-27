namespace ImageLinks_.Application.Features.SysSettingDetail.DTO;

public record SysSettingDetailDto(
    string? SettId=null,
    string? SettTypeId = null,
    string? SettValue = null,
    string? UserId = null,
    string? DescEn = null,
    string? DescAr = null,
    string? CrtBy = null,
    string? CrtDt = null,
    string? UpdtBy = null);
