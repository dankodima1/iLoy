namespace Tms.Dto.Extensions
{
    public class DtoExtensions
    {
        public const int TaskItem_Name_MaxLength = 100;
        public const int TaskItem_Description_MaxLength = 400;
        public const string Message_TaskItem_Name_MaxLength = "max length is 100";
        public const string Message_TaskItem_Description_MaxLength = "max length is 400";

        public static string GetErrorMessage_NotFound(string name, object value)
        {
            return $"{name} with Id = ({value}) not found";
        }

        public static string GetErrorMessage_IsNull(string name)
        {
            return $"{name} is null";
        }

        public static string GetErrorMessage_ShouldBe(string fieldName, string entityName, object value)
        {
            return $"{fieldName} of the {entityName} should be {value}";
        }
    }
}
