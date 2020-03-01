using System;

namespace UtilitiesBills.Validations.Rules
{
    public class IsPositiveValueRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            switch (value)
            {
                case decimal dec:
                    return dec > 0;
                case int i:
                    return i > 0;
                case double doub:
                    return doub > 0;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
