using Xamarin.Forms;

namespace UtilitiesBills.Controls
{
    public class ColoredListView : ListView
    {
        private ViewCell _lastCell;
        private Color _lastCellColor;

        public Color EvenBackgroundColor { get; set; }
        public Color SelectedColor { get; set; }

        public ColoredListView() : base()
        {
            EvenBackgroundColor = BackgroundColor;
        }

        public ColoredListView(ListViewCachingStrategy cachingStrategy) : base(cachingStrategy)
        {
            EvenBackgroundColor = BackgroundColor;
        }

        protected override void SetupContent(Cell content, int index)
        {
            base.SetupContent(content, index);

            var viewCell = content as ViewCell;
            viewCell.Tapped += ViewCell_Tapped;

            SetBackgroundColor(viewCell, index);
        }

        private void SetBackgroundColor(ViewCell viewCell, int index)
        {
            if (viewCell?.View == null)
            {
                return;
            }

            if (EvenBackgroundColor == BackgroundColor)
            {
                return;
            }

            if (index % 2 == 1)
            {
                viewCell.View.BackgroundColor = EvenBackgroundColor;
            }
        }

        private void ViewCell_Tapped(object sender, System.EventArgs e)
        {
            var viewCell = (ViewCell)sender;

            if (_lastCell != null && _lastCell.Id == viewCell.Id)
            {
                return;
            }

            if (_lastCell?.View != null)
            {
                _lastCell.View.BackgroundColor = _lastCellColor;
            }

            if (viewCell.View != null)
            {
                _lastCell = viewCell;
                _lastCellColor = viewCell.View.BackgroundColor;
                viewCell.View.BackgroundColor = SelectedColor;
            }
        }
    }
}
