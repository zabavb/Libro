using System.ComponentModel.DataAnnotations;

namespace Library.Validators
{
    public class EnumRangeAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumRangeAttribute(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type must be an enum");
            }
            _enumType = enumType;

            ErrorMessage = $"The value is not a valid member of the {_enumType.Name} enum.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            return Enum.GetNames(_enumType).Contains(value.ToString());
        }
    }
}
