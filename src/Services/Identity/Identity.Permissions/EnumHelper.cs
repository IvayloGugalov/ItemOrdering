using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Identity.Permissions
{
    public static class EnumHelper
    {
        public static (string, string) GetNameAndDescription(this Permissions enumValue)
        {
            var name = enumValue.ToString();
            try
            {
                var description = enumValue.GetEnumAttributes().Description;

                return string.IsNullOrEmpty(description)
                    ? (name, string.Empty)
                    : (name, description);
            }
            catch
            {
                return (name, string.Empty);
            }
        }

        public static string GetDisplayName(this Permissions enumValue)
        {
            try
            {
                return enumValue.GetEnumAttributes().Name;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static char GetPermissionAsChar(this Permissions enumValue)
        {
            return (char)Convert.ChangeType(Enum.Parse<Permissions>(enumValue.ToString()), typeof(char));
        }

        private static DisplayAttribute GetEnumAttributes(this Permissions enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfos = enumType.GetMember(enumValue.ToString());

            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            var valueAttributes = enumValueMemberInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);

            return (DisplayAttribute)valueAttributes[0];
        }
    }
}
