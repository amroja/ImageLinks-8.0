namespace ImageLinks_.API.Middleware
{
    using ImageLinks_.Application.Common.Enums;
    using ImageLinks_.Application.Features.Users.DTO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Security.Claims;
    using System.Text.Json;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class ApiAuthorizationAttribute : ActionFilterAttribute
    {
        private readonly List<GeneralEnums.UserPrivileges> _privileges;
        private readonly GeneralEnums.Operations _operation;

        public ApiAuthorizationAttribute(params GeneralEnums.UserPrivileges[] privileges)
        {
            _privileges = privileges.ToList();
            _operation = GeneralEnums.Operations.AND;
        }

        public ApiAuthorizationAttribute(GeneralEnums.Operations operation, params GeneralEnums.UserPrivileges[] privileges)
        {
            _privileges = privileges.ToList();
            _operation = operation;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                ClaimsPrincipal? user = context.HttpContext.User;

                if (!user.Identity?.IsAuthenticated ?? true)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var privilegesClaim = user.FindFirst("privileges")?.Value;

                if (string.IsNullOrEmpty(privilegesClaim))
                {
                    context.Result = new ForbidResult();
                    return;
                }

                UserPrivilegeDto? userPrivileges = JsonSerializer.Deserialize<UserPrivilegeDto>(privilegesClaim, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (userPrivileges == null)
                {
                    context.Result = new ForbidResult();
                    return;
                }

                if (!HasPermissions(userPrivileges))
                {
                    context.Result = new ForbidResult();
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool HasPermissions(UserPrivilegeDto privileges)
        {
            if (_operation == GeneralEnums.Operations.AND)
            {
                return _privileges.All(p => CheckPrivilege(privileges, p));
            }

            return _privileges.Any(p => CheckPrivilege(privileges, p));
        }

        private static bool CheckPrivilege(UserPrivilegeDto privileges, GeneralEnums.UserPrivileges privilege)
        {
            return privilege switch
            {
                // Manager Viewer Privileges
                GeneralEnums.UserPrivileges.CanSearch => privileges.CanSearch,
                GeneralEnums.UserPrivileges.CanOCR => privileges.CanOCR,
                GeneralEnums.UserPrivileges.CanChangeSettings => privileges.CanChangeSettings,
                GeneralEnums.UserPrivileges.CanChangePassword => privileges.CanChangePassword,
                GeneralEnums.UserPrivileges.CanGlobalSearch => privileges.CanGlobalSearch,
                GeneralEnums.UserPrivileges.CanApplicationSearch => privileges.CanApplicationSearch,
                GeneralEnums.UserPrivileges.CanCommonSearch => privileges.CanCommonSearch,
                GeneralEnums.UserPrivileges.CanContentSearch => privileges.CanContentSearch,
                GeneralEnums.UserPrivileges.CanCriteriaSearch => privileges.CanCriteriaSearch,
                GeneralEnums.UserPrivileges.CanLinkedSearch => privileges.CanLinkedSearch,
                GeneralEnums.UserPrivileges.CanAccessSystemCode => privileges.CanAccessSystemCode,
                GeneralEnums.UserPrivileges.CanDocumentIndexView => privileges.CanDocumentIndexView,
                GeneralEnums.UserPrivileges.CanScannerSettings => privileges.CanScannerSettings,

                // Version Privileges
                GeneralEnums.UserPrivileges.CanDeleteVersion => privileges.CanDeleteVersion,
                GeneralEnums.UserPrivileges.CanReinstateVersion => privileges.CanReinstateVersion,

                // Other Privileges
                GeneralEnums.UserPrivileges.CanUseRoute => privileges.CanUseRoute,
                GeneralEnums.UserPrivileges.CanOthersTransactions => privileges.CanOthersTransactions,
                GeneralEnums.UserPrivileges.IsTwain => privileges.IsTwain,

                // Report Privileges
                GeneralEnums.UserPrivileges.CanShowProductivityReport => privileges.CanShowProductivityReport,
                GeneralEnums.UserPrivileges.CanShowUserDocuments => privileges.CanShowUserDocuments,
                GeneralEnums.UserPrivileges.CanShowAddedDocumentReport => privileges.CanShowAddedDocumentReport,
                GeneralEnums.UserPrivileges.CanShowTransactionSummaryReport => privileges.CanShowTransactionSummaryReport,
                GeneralEnums.UserPrivileges.CanShowSystemTransactionReport => privileges.CanShowSystemTransactionReport,
                GeneralEnums.UserPrivileges.CanShowTreeInfoReport => privileges.CanShowTreeInfoReport,
                GeneralEnums.UserPrivileges.CanShowLoggedinUsersReport => privileges.CanShowLoggedinUsersReport,
                GeneralEnums.UserPrivileges.CanShowOverdueTasksReport => privileges.CanShowOverdueTasksReport,
                GeneralEnums.UserPrivileges.CanShowDocumentsRouteReport => privileges.CanShowDocumentsRouteReport,
                GeneralEnums.UserPrivileges.CanShowDocumentsAndPagesReport => privileges.CanShowDocumentsAndPagesReport,

                // Has Privileges
                GeneralEnums.UserPrivileges.HasManagerPrivileges => privileges.HasManagerPrivileges,
                GeneralEnums.UserPrivileges.HasManagerViewerPrivileges => privileges.HasManagerViewerPrivileges,
                GeneralEnums.UserPrivileges.HasAnnotationPrivileges => privileges.HasAnnotationPrivileges,
                GeneralEnums.UserPrivileges.HasTreePrivileges => privileges.HasTreePrivileges,
                GeneralEnums.UserPrivileges.HasReportPrivileges => privileges.HasReportPrivileges,
                GeneralEnums.UserPrivileges.HasDeleteVersionPrivileges => privileges.HasDeleteVersionPrivileges,
                GeneralEnums.UserPrivileges.HasReinstateVersionPrivileges => privileges.HasReinstateVersionPrivileges,

                _ => false
            };
        }
    }
}
