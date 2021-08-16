namespace Tms.Dto
{
    public class ApiList
    {
        public const string API_TEMPLATE = "api/[controller]";

        // Api Task

        public const string API_TASK_GET = "";
        public const string API_TASK_GET_BY_ID = "{id}";
        public const string API_TASK_CREATE = "Create";
        public const string API_TASK_UPDATE = "Update";
        public const string API_TASK_DELETE = "{id}";

        // Client Api Task

        public const string CLIENT_API_TASK = "api/Task/";
        public const string CLIENT_API_TASK_GET = CLIENT_API_TASK + API_TASK_GET;
        public const string CLIENT_API_TASK_GET_BY_ID = CLIENT_API_TASK + API_TASK_GET_BY_ID;
        public const string CLIENT_API_TASK_CREATE = CLIENT_API_TASK + API_TASK_CREATE;
        public const string CLIENT_API_TASK_UPDATE = CLIENT_API_TASK + API_TASK_UPDATE;
        public const string CLIENT_API_TASK_DELETE = CLIENT_API_TASK + API_TASK_DELETE;

        // Api Report

        public const string API_REPORT_GET = "";

        // Client Api Report

        public const string CLIENT_API_REPORT = "api/Report/";
        public const string CLIENT_API_REPORT_GET = CLIENT_API_REPORT + API_REPORT_GET;
    }
}
