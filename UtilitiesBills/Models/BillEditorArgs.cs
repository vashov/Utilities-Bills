namespace UtilitiesBills.Models
{
    public class BillEditorArgs
    {
        public BillItem Bill { get; set; }
        public MeterReadingItem PreviousMeterReading { get; set; }
    }
}
