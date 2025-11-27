namespace ImageLinks_.Application.Common.Enums;

public class GeneralEnums
{
    public enum USERS
    {
        SYSADMIN = 0
    }

    public enum DATABASE_SHARING_TYPE
    {
        SINGLE_TENANT = 1,
        MULTI_TENANT = 2
    }

    public enum FILE_SOURCE
    {
        SCANNED = 1,
        IMPORTED = 2,
        INTEGRATION = 3,
        OCR_RESULT = 4
    }

    public enum INDEX_FIELD_TYPE
    {
        NUMBER = 0,
        TEXT = 2,
        DATE = 3,
        SYSTEM_CODE = 6,
        SERIAL = 7,
        HIJRI = 8,
        DYNAMIC = 9,
        BIG_NUMBER = 10,
        DB_CODE = 20
    }

    [Flags]
    public enum MANAGER_VIEWER_SECURITY_FLAG : uint
    {
        SYSTEM_CODE = 1,
        SETTINGS = 2,
        EXPORT = 8,
        PRINT = 16,        
        OCR = 32,
        FAX = 512,
        OUTLOOK_MAIL = 2048,   
        ROUTE = 4096,
        SEARCH = 32768,
        GLOBAL_SEARCH = 131072,
        CRITERIA_SEARCH = 262144,
        COMMON_SEARCH = 524288,
        APPLICATION_SEARCH = 1048576,
        CONTENT_SEARCH = 2097152,
        LINKED_SEARCH = 4194304,
        DOCUMENT_INDEX_VIEW = 33554432,
        SCANNER_SETTINGS = 67108864
    }

    [Flags]
    public enum DOCUMENT_SECURITY_FLAG : ulong
    {
        VIEW_DOCUMENT = 1,
        ADD_IMPORT_NEW_DOCUMENT = 2,
        SCAN_IMPORT_NEW_PAGES = 2,        
        UPDATE_DOCUMENT = 4,

        MOVE_DOCUMENT = 8,
        DELETE_DOCUMENT = 8,        
        DELETE_ALL_PAGES = 8,        
        EXPORT_PAGES = 8,         

        ADD_CATEGORIES_CONTENT = 16,
        UPDATE_CATEGORIES_CONTENT = 32,
        OCR_TO_CONTENT = 32,       
        DELETE_CATEGORIES_CONTENT = 64,
        VIEW_CATEGORIES_CONTENT = 128,

        REARRANGE_PAGES = 256,
        COPY_DOCUMENT = 512,
        COPY_PAGES = 65536,
        CUT_PAGES = 131072,
        REPLACE_PAGES = 32768,
        UPDATE_FILE = 1024,
        DELETE_PAGES = 2048,
        VIEW_DOCUMENT_VERSIONS = 4096,
        DELETE_DOCUMENT_VERSION = 16384,
        REINSTATE_DOCUMENT_VERSION = 8192,

        WATERMARK_EDIT = 262144,
        WATERMARK_HIDE = 524288,

        SEND_BY_EMAIL = 1048576,
        ACCESS_KEY = 2097152,
        PRINT = 4194304,
        DOWNLOAD = 8388608,
        DOWNLOAD_PDF = 16777216,

        ALLOW_SHOW_SIGNTURE = 33554432,
        ALLOW_SHOW_OTHER_USER_SIGNTURE = 67108864,

        ADD_STAMP = 134217728,
        REMOVE_STAMP = 268435456,

        DELETE_ROUTE = 536870912,
        SEND_ROUTE = 1073741824,
        RESEND_ROUTE = 2147483648,

        IMAGE_EDITING = 4294967296,

        UPLOAD_ROUTE = 8589934592,
        VIEW_MINE = 17179869184,
        COPY_ROUTE = 34359738368
    }

    [Flags]
    public enum REPORTS_SECURITY_FLAG : uint
    {
        PRODUCTIVITY = 1,
        USER_DOCUMENTS = 2,
        USER_DOCUMENTS_PRIVILEGE = 4,
        GROUP_DOCUMENTS = 8,
        GROUP_DOCUMENTS_PRIVILEGES = 16,
        GROUP_PRIVILEGES = 32,
        ADDED_DOCUMENT_INSTANCES = 64,
        TREES_INFORMATION = 128,
        DOCUMENTS_ROUTING = 256,
        OVERDUE_TASKS = 512,
        DOCUMENTS_SUMMARY = 1024,
        SYSTEM = 2048,
        LOGGEDIN_USERS = 8192,
        OTHERS_TRANSACTIONS = 16384,
        DOCUMENTS_AND_PAGES = 32768
    }

    public enum UserPrivileges
    {
        // Manager Viewer Privileges
        CanSearch = 1,
        CanOCR = 2,
        CanChangeSettings = 3,
        CanChangePassword = 4,
        CanGlobalSearch = 5,
        CanApplicationSearch = 6,
        CanCommonSearch = 7,
        CanContentSearch = 8,
        CanCriteriaSearch = 9,
        CanLinkedSearch = 10,
        CanAccessSystemCode = 11,
        CanDocumentIndexView = 12,
        CanScannerSettings = 13,

        // Version Privileges
        CanDeleteVersion = 20,
        CanReinstateVersion = 21,

        // Other Privileges
        CanUseRoute = 30,
        CanOthersTransactions = 31,
        IsTwain = 32,

        // Report Privileges
        CanShowProductivityReport = 40,
        CanShowUserDocuments = 41,
        CanShowAddedDocumentReport = 42,
        CanShowTransactionSummaryReport = 43,
        CanShowSystemTransactionReport = 44,
        CanShowTreeInfoReport = 45,
        CanShowLoggedinUsersReport = 46,
        CanShowOverdueTasksReport = 47,
        CanShowDocumentsRouteReport = 48,
        CanShowDocumentsAndPagesReport = 49,

        // Has Privileges (Role-level)
        HasManagerPrivileges = 60,
        HasManagerViewerPrivileges = 61,
        HasAnnotationPrivileges = 62,
        HasTreePrivileges = 63,
        HasReportPrivileges = 64,
        HasDeleteVersionPrivileges = 65,
        HasReinstateVersionPrivileges = 66
    }

    public enum Operations
    {
        AND,
        OR
    }


    public enum DOCUMENT_LOCK_FLAG
    {
        UNLOCKED = 0,
        LOCKED = 1
    }

    public enum DOCUMENT_INFORMATION_FIELDS
    {
        ENGLISH_TITLE = 1,
        ARABIC_TITLE = 2,
        SECURITY_LEVEL = 3,
        PRIVACY_LEVEL = 4,
        DOCUMENT_SOURCE = 5
    }


    public enum DOCUMENT_SOURCE
    {
        COPY = 1,
        CUT = 2,
        ORIGINAL = 3,
        MIGRATION = 4,
        INTEGRATION = 5
    }


    public enum OBJ_LEVEL
    {
        CABINET_PRIVILEGES = 1,
        DRAWER_PRIVILEGES = 2,
        FOLDER_PRIVILEGES = 3,
        DOCUMENT_PRIVILEGES = 4,
        TREE_PRIVILEGES = 5,
        MANAGER_VIEWER_PRIVILEGES = 6,
        MANAGER_PRIVILEGES = 7,
        ANNOTATION_PRIVILEGES = 8,
        REPORT_PRIVILEGES = 9,
        ROLE = 11,
        USER_SECURITY_LEVEL = 21,
        SCREENS_PRIVILEGES = 31
    }

    public enum SYS_SETT_TYPE
    {
        EMAIL_NOTIFICATION_CONFIGURATIONS = 1,
        FAX_CONFIGURATIONS = 2,
        OCR_SCHEDULE_CONFIGURATIONS = 3,
        OCR_TREE_CONFIGURATIONS = 4,
        DASHBOARD_SETTINGS = 6,
        SMTP_SETTINGS = 7,
        OCR_TREE_CONFIGURATION = 8,
        MAIL_NOTIFICATION_ACTIONS = 9,
        REPORTS_XML_DATA = 10,
        STATISTICS_SCHEDULE = 11,
        STATISTICS_DATA = 12,
        DOCUMENT_SOURCE = 13,
        FIELD_TYPE = 15,
        BARCODE_ZONE = 16,
        OCR_TYPE = 17,
        SYSTEM_OPTIONS = 18,
        CONFIGURATION_VALUES = 19,
        OCR__TYPE = 20
    }
}