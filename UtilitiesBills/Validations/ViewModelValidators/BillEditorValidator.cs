using UtilitiesBills.Validations.Rules;

namespace UtilitiesBills.Validations.ViewModelValidators
{
    public class BillEditorValidatorContext
    {
        public ValidatableObject<decimal> HotWaterBulk { get; set; }
        public ValidatableObject<decimal> ColdWaterBulk { get; set; }
        public ValidatableObject<decimal> ElectricityBulk { get; set; }
        public ValidatableObject<string> Note { get; set; }
    }

    public class BillEditorValidator : IValidator
    {
        private readonly ValidatableObject<decimal> _hotWaterBulk;
        private readonly ValidatableObject<decimal> _coldWaterBulk;
        private readonly ValidatableObject<decimal> _electricityBulk;
        private readonly ValidatableObject<string> _note;

        public BillEditorValidator(BillEditorValidatorContext context)
        {
            _hotWaterBulk = context.HotWaterBulk;
            _coldWaterBulk = context.ColdWaterBulk;
            _electricityBulk = context.ElectricityBulk;
            _note = context.Note;
        }

        public void AddValidations()
        {
            _hotWaterBulk.Validations.Add(new IsPositiveValueRule<decimal>
            {
                ValidationMessage = "A positive bulk of hot water is required."
            });

            _coldWaterBulk.Validations.Add(new IsPositiveValueRule<decimal>
            {
                ValidationMessage = "A positive bulk of cold water is required."
            });

            _electricityBulk.Validations.Add(new IsPositiveValueRule<decimal>
            {
                ValidationMessage = "A positive bulk of electricity is required."
            });

            _note.Validations.Add(new MaxLenghtRule<string>());
        }

        public bool Validate()
        {
            bool isValidHotWaterBulk = _hotWaterBulk.Validate();
            bool isValidColdWaterBulk = _coldWaterBulk.Validate();
            bool isValidElectricityBulk = _electricityBulk.Validate();
            bool isValidNote = _note.Validate();

            return isValidHotWaterBulk && isValidColdWaterBulk && isValidElectricityBulk && isValidNote;
        }
    }
}
