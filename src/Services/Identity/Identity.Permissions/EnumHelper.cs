using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Identity.Permissions
{
    public static class EnumHelper
    {
        public static (string, string) GetAttributeInfo(this Enum enumValue)
        {
            try
            {
                var enumType = enumValue.GetType();
                var name = enumValue.ToString();
                var memberInfos = enumType.GetMember(name);

                var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
                var valueAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);

                if (valueAttributes == null) return (name, string.Empty);

                var description = ((DisplayAttribute)valueAttributes[0]).Description;
                return (name, description);
            }
            catch
            {
                return (enumValue.ToString(), string.Empty);
            }
        }

        public static char GetPermissionAsChar(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var name = enumValue.ToString();

            return (char)Convert.ChangeType(Enum.Parse(enumType, name), typeof(char));
        }
    }
}
