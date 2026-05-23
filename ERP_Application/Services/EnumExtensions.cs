using System.ComponentModel;
using System.Reflection;

namespace ERP_Application.Services
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            if (value is null)
                return string.Empty;

            var field = value.GetType().GetField(value.ToString());

            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }
    }
}
