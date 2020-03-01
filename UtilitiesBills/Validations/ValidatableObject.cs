using System;
using System.Collections.Generic;
using System.Linq;
using UtilitiesBills.ViewModels.Base;

namespace UtilitiesBills.Validations
{
    public class ValidatableObject<T> : BaseNotifier, IValidity
    {
        private readonly Action _onValueChanged;
        private List<string> _errors = new List<string>();
        private bool _isValid = true;
        private T _value;

        public List<IValidationRule<T>> Validations { get; } = new List<IValidationRule<T>>();

        public List<string> Errors
        {
            get => _errors;
            set => SetProperty(ref _errors, value);
        }

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value, _onValueChanged);
        }

        public bool IsValid
        {
            get => _isValid;
            set => SetProperty(ref _isValid, value);
        }

        public ValidatableObject()
        {
        }

        public ValidatableObject(Action onValueChanged)
        {
            _onValueChanged = onValueChanged;
        }

        public bool Validate()
        {
            Errors.Clear();
            var errors = Validations.Where(v => !v.Check(Value)).Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
