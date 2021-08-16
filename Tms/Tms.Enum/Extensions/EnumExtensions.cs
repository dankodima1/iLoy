namespace Tms.Enum.Extensions
{
    public static class EnumExtensions
    {
        public static string Name(this TaskItemState val)
        {
            return System.Enum.GetName(typeof(TaskItemState), val);
        }
    }
}
