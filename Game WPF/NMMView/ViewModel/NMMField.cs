using ModelNineMenMorris.Model;

namespace NMMView.ViewModel 
{
    public class NMMField : ViewModelBase
    {
        public MyCommand? PushCircleButton { get; set; }
        private ColorFields color;
        private int ind;
        private double top;
        private double left;
        private double size;
        public ColorFields Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Ind { get { return ind; } set {ind=value; } }
        public double Top { get { return top; } set {top = value;} }
        public double Left { get { return left; } set {left = value;} }
        public double Size { get { return size; } set {size = value;} }
    }
}
