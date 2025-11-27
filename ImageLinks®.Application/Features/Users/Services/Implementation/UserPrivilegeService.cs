using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Requests;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.Requests;
using ImageLinks_.Application.Features.Users.Services.Interface;

namespace ImageLinks_.Application.Features.Users.Services.Implementation;

public class UserPrivilegeService : IUserPrivilegeService
{
    private readonly IGroupMbrService _groupMbrService;
    private readonly IGroupSecService _groupSecService;
    private readonly IUserSecService _userSecService;

    public UserPrivilegeService(IGroupMbrService groupMbrService, IUserSecService userSecService, IGroupSecService groupSecService)
    {
        _groupMbrService = groupMbrService;
        _userSecService = userSecService;
        _groupSecService = groupSecService;
    }

    public async Task<UserPrivilegeDto> GetUserPrivilege(UserDto filter, CancellationToken ct = default)
    {
        if (filter.UserId == GeneralEnums.USERS.SYSADMIN.ToId())
        {
            return CreateSysAdminPrivileges();
        }

        List<string> groupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = filter.UserId }, ct);

        if (groupIds.Count > 0)
        {
            return await GetPrivilegesForGroups(groupIds, ct);
        }

        return await GetPrivilegesForUser(filter.UserId, ct);
    }

    private static UserPrivilegeDto CreateSysAdminPrivileges()
    {
        return new UserPrivilegeDto
        {
            CanUseRoute = true,
            CanChangeSettings = true,
            CanChangePassword = true,
            CanShowProductivityReport = true,
            CanShowUserDocuments = true,
            CanShowAddedDocumentReport = true,
            CanShowTransactionSummaryReport = true,
            CanShowSystemTransactionReport = true,
            CanShowTreeInfoReport = true,
            CanDeleteVersion = true,
            CanReinstateVersion = true,
            IsTwain = true,
            CanSearch = true,
            CanOCR = true,
            CanShowLoggedinUsersReport = true,
            CanShowOverdueTasksReport = true,
            CanShowDocumentsRouteReport = true,
            CanAccessSystemCode = true,
            CanGlobalSearch = true,
            CanCriteriaSearch = true,
            CanCommonSearch = true,
            CanApplicationSearch = true,
            CanContentSearch = true,
            CanLinkedSearch = true,
            CanDocumentIndexView = true,
            CanScannerSettings = true,
            CanOthersTransactions = true,
            CanShowDocumentsAndPagesReport = true,
            HasManagerViewerPrivileges = true,
            HasManagerPrivileges = true,
            HasDeleteVersionPrivileges = true,
            HasReinstateVersionPrivileges = true,
            HasAnnotationPrivileges = true,
            HasTreePrivileges = true,
            HasReportPrivileges = true
        };
    }

    private async Task<UserPrivilegeDto> GetPrivilegesForUser(string userId, CancellationToken ct)
    {
        UserPrivilegeDto privilegeDto = new UserPrivilegeDto();

        List<UserSecDto> reportPrivileges = await _userSecService.SelectAsync(
            new UserSecRequest { UserId = userId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.REPORT_PRIVILEGES) }, ct);

        if (reportPrivileges?.Count > 0)
        {
            ApplyReportPrivileges(privilegeDto, Convert.ToInt64(reportPrivileges[0].ObjFlag));
        }

        List<UserSecDto> managerViewerPrivileges = await _userSecService.SelectAsync(
            new UserSecRequest { UserId = userId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.MANAGER_VIEWER_PRIVILEGES) }, ct);

        if (managerViewerPrivileges?.Count > 0)
        {
            ApplyManagerViewerPrivileges(privilegeDto, Convert.ToInt64(managerViewerPrivileges[0].ObjFlag));
        }

        privilegeDto.HasManagerPrivileges = await HasPrivilegeFlag(
            () => _userSecService.SelectAsync(new UserSecRequest { UserId = userId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.MANAGER_PRIVILEGES) }, ct));

        privilegeDto.HasAnnotationPrivileges = await HasPrivilegeFlag(
            () => _userSecService.SelectAsync(new UserSecRequest { UserId = userId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.ANNOTATION_PRIVILEGES) }, ct));

        privilegeDto.HasTreePrivileges = await HasPrivilegeFlag(
            () => _userSecService.SelectAsync(new UserSecRequest { UserId = userId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.TREE_PRIVILEGES) }, ct));

        return privilegeDto;
    }

    private async Task<UserPrivilegeDto> GetPrivilegesForGroups(List<string> groupIds, CancellationToken ct)
    {
        UserPrivilegeDto privilegeDto = new UserPrivilegeDto();
        List<string> validGroupIds = groupIds.Where(id => id != "0").ToList();

        foreach (string groupId in validGroupIds)
        {
            UserPrivilegeDto tempDto = await GetPrivilegesForSingleGroup(groupId, ct);
            MergePrivileges(privilegeDto, tempDto);
        }

        return privilegeDto;
    }

    private async Task<UserPrivilegeDto> GetPrivilegesForSingleGroup(string groupId, CancellationToken ct)
    {
        UserPrivilegeDto tempDto = new UserPrivilegeDto();

        List<GroupSecDto> reportPrivileges = await _groupSecService.SelectAsync(
            new GroupSecRequest { GroupId = groupId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.REPORT_PRIVILEGES) }, ct);

        if (reportPrivileges?.Count > 0)
        {
            ApplyReportPrivileges(tempDto, Convert.ToInt64(reportPrivileges[0].ObjFlag));
        }

        List<GroupSecDto>? managerViewerPrivileges = await _groupSecService.SelectAsync(
            new GroupSecRequest { GroupId = groupId, ObjLevel = GetObjLevel(GeneralEnums.OBJ_LEVEL.MANAGER_VIEWER_PRIVILEGES) }, ct);

        if (managerViewerPrivileges?.Count > 0)
        {
            ApplyManagerViewerPrivileges(tempDto, Convert.ToInt64(managerViewerPrivileges[0].ObjFlag));
        }

        tempDto.HasManagerPrivileges = await HasGroupPrivilegeFlag(groupId, GeneralEnums.OBJ_LEVEL.MANAGER_PRIVILEGES, ct);
        tempDto.HasAnnotationPrivileges = await HasGroupPrivilegeFlag(groupId, GeneralEnums.OBJ_LEVEL.ANNOTATION_PRIVILEGES, ct);
        tempDto.HasTreePrivileges = await HasGroupPrivilegeFlag(groupId, GeneralEnums.OBJ_LEVEL.TREE_PRIVILEGES, ct);

        return tempDto;
    }

    private static void ApplyReportPrivileges(UserPrivilegeDto dto, long flag)
    {
        dto.HasReportPrivileges = flag > 0;
        dto.CanShowProductivityReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.PRODUCTIVITY);
        dto.CanShowAddedDocumentReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.ADDED_DOCUMENT_INSTANCES);
        dto.CanShowTransactionSummaryReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.DOCUMENTS_SUMMARY);
        dto.CanShowSystemTransactionReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.SYSTEM);
        dto.CanShowUserDocuments = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.USER_DOCUMENTS);
        dto.CanShowTreeInfoReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.TREES_INFORMATION);
        dto.CanShowLoggedinUsersReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.LOGGEDIN_USERS);
        dto.CanShowDocumentsRouteReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.DOCUMENTS_ROUTING);
        dto.CanShowOverdueTasksReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.OVERDUE_TASKS);
        dto.CanOthersTransactions = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.OTHERS_TRANSACTIONS);
        dto.CanShowDocumentsAndPagesReport = HasFlag(flag, GeneralEnums.REPORTS_SECURITY_FLAG.DOCUMENTS_AND_PAGES);
    }

    private static void ApplyManagerViewerPrivileges(UserPrivilegeDto dto, long flag)
    {
        dto.HasManagerViewerPrivileges = flag > 0;

        if (flag <= 0)
        {
            dto.CanChangePassword = false;
            return;
        }

        dto.CanAccessSystemCode = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.SYSTEM_CODE);
        dto.CanChangeSettings = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.SETTINGS);
        dto.CanSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.SEARCH);
        dto.CanOCR = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.OCR);
        dto.CanGlobalSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.GLOBAL_SEARCH);
        dto.CanApplicationSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.APPLICATION_SEARCH);
        dto.CanCommonSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.COMMON_SEARCH);
        dto.CanContentSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.CONTENT_SEARCH);
        dto.CanCriteriaSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.CRITERIA_SEARCH);
        dto.CanLinkedSearch = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.LINKED_SEARCH);
        dto.CanDocumentIndexView = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.DOCUMENT_INDEX_VIEW);
        dto.CanScannerSettings = HasFlag(flag, GeneralEnums.MANAGER_VIEWER_SECURITY_FLAG.SCANNER_SETTINGS);
    }

    private static bool HasFlag<TEnum>(long flag, TEnum enumValue) where TEnum : Enum
    {
        return (flag & Convert.ToInt64(enumValue)) > 0;
    }

    private static string GetObjLevel(GeneralEnums.OBJ_LEVEL level) => level.GetHashCode().ToString();

    private async Task<bool> HasPrivilegeFlag(Func<Task<List<UserSecDto>?>> fetchFunc)
    {
        List<UserSecDto> privileges = await fetchFunc();
        return privileges?.Count > 0 && Convert.ToInt64(privileges[0].ObjFlag) > 0;
    }

    private async Task<bool> HasGroupPrivilegeFlag(string groupId, GeneralEnums.OBJ_LEVEL level, CancellationToken ct)
    {
        List<GroupSecDto> privileges = await _groupSecService.SelectAsync(
            new GroupSecRequest { GroupId = groupId, ObjLevel = GetObjLevel(level) }, ct);
        return privileges?.Count > 0 && Convert.ToInt64(privileges[0].ObjFlag) > 0;
    }

    private static void MergePrivileges(UserPrivilegeDto target, UserPrivilegeDto source)
    {
        target.CanAccessSystemCode |= source.CanAccessSystemCode;
        target.CanApplicationSearch |= source.CanApplicationSearch;
        target.CanChangePassword |= source.CanChangePassword;
        target.CanChangeSettings |= source.CanChangeSettings;
        target.CanCommonSearch |= source.CanCommonSearch;
        target.CanContentSearch |= source.CanContentSearch;
        target.CanCriteriaSearch |= source.CanCriteriaSearch;
        target.CanDeleteVersion |= source.CanDeleteVersion;
        target.CanDocumentIndexView |= source.CanDocumentIndexView;
        target.CanGlobalSearch |= source.CanGlobalSearch;
        target.CanLinkedSearch |= source.CanLinkedSearch;
        target.CanOCR |= source.CanOCR;
        target.CanOthersTransactions |= source.CanOthersTransactions;
        target.CanReinstateVersion |= source.CanReinstateVersion;
        target.CanScannerSettings |= source.CanScannerSettings;
        target.CanSearch |= source.CanSearch;
        target.CanShowAddedDocumentReport |= source.CanShowAddedDocumentReport;
        target.CanShowDocumentsRouteReport |= source.CanShowDocumentsRouteReport;
        target.CanShowLoggedinUsersReport |= source.CanShowLoggedinUsersReport;
        target.CanShowOverdueTasksReport |= source.CanShowOverdueTasksReport;
        target.CanShowProductivityReport |= source.CanShowProductivityReport;
        target.CanShowSystemTransactionReport |= source.CanShowSystemTransactionReport;
        target.CanShowTransactionSummaryReport |= source.CanShowTransactionSummaryReport;
        target.CanShowTreeInfoReport |= source.CanShowTreeInfoReport;
        target.CanShowUserDocuments |= source.CanShowUserDocuments;
        target.CanShowDocumentsAndPagesReport |= source.CanShowDocumentsAndPagesReport;
        target.CanUseRoute |= source.CanUseRoute;
        target.IsTwain |= source.IsTwain;
        target.HasReportPrivileges |= source.HasReportPrivileges;
        target.HasManagerViewerPrivileges |= source.HasManagerViewerPrivileges;
        target.HasManagerPrivileges |= source.HasManagerPrivileges;
        target.HasAnnotationPrivileges |= source.HasAnnotationPrivileges;
        target.HasTreePrivileges |= source.HasTreePrivileges;
    }
}
