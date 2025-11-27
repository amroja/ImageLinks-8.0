namespace ImageLinks_.Application.Features.Users.DTO;

public class UserPrivilegeDto
{
    public bool CanUseRoute { get; set; }
    public bool CanChangeSettings { get; set; }
    public bool CanChangePassword { get; set; }
    public bool CanShowProductivityReport { get; set; }
    public bool CanShowUserDocuments { get; set; }
    public bool CanShowAddedDocumentReport { get; set; }
    public bool CanShowTransactionSummaryReport { get; set; }
    public bool CanShowSystemTransactionReport { get; set; }
    public bool CanShowTreeInfoReport { get; set; }
    public bool CanDeleteVersion { get; set; }
    public bool CanReinstateVersion { get; set; }
    public bool IsTwain { get; set; }
    public bool CanSearch { get; set; }
    public bool CanOCR { get; set; }
    public bool CanShowLoggedinUsersReport { get; set; }
    public bool CanShowOverdueTasksReport { get; set; }
    public bool CanShowDocumentsRouteReport { get; set; }
    public bool CanAccessSystemCode { get; set; }
    public bool CanGlobalSearch { get; set; }
    public bool CanCriteriaSearch { get; set; }
    public bool CanCommonSearch { get; set; }
    public bool CanApplicationSearch { get; set; }
    public bool CanContentSearch { get; set; }
    public bool CanLinkedSearch { get; set; }
    public bool CanDocumentIndexView { get; set; }
    public bool CanScannerSettings { get; set; }
    public bool CanOthersTransactions { get; set; }
    public bool HasManagerViewerPrivileges { get; set; }
    public bool HasManagerPrivileges { get; set; }
    public bool HasDeleteVersionPrivileges { get; set; }
    public bool HasReinstateVersionPrivileges { get; set; }
    public bool HasAnnotationPrivileges { get; set; }
    public bool HasTreePrivileges { get; set; }
    public bool HasReportPrivileges { get; set; }
    public bool CanShowDocumentsAndPagesReport { get; set; }
}
