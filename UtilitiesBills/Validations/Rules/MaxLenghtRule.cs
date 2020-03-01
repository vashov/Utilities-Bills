using System;

namespace UtilitiesBills.Validations.Rules
{
    public class MaxLenghtRule<T> : IValidationRule<T>
    {
        private readonly int _requiredMaxLenght;

        public string ValidationMessage { get; set; }

        public MaxLenghtRule(int requiredMaxLenght = 100)
        {
            _requiredMaxLenght = requiredMaxLenght;
            ValidationMessage = $"Max lenght is {_requiredMaxLenght}";
        }

        public bool Check(T value)
        {
            if (value == null)
            {
                return true;
            }

            if (!(value is string str))
            {
                throw new ArgumentException();
            }

            return str.Length <= _requiredMaxLenght;
        }
    }
}
