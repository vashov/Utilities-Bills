namespace UtilitiesBills.Validations.ViewModelValidators
{
    public interface IValidator
    {
        void AddValidations();
        bool Validate();
    }
}
