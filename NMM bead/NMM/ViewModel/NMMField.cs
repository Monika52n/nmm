using NMMModel.Model;

namespace NMM.ViewModel 
{
    public class NMMField : ViewModelBase
    {
        public DelegateCommand? PushCircleButton { get; set; }
        private ColorFields color;
        private int ind;
        private LOCKED locked;
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
        public int Ind { get { return ind; } set { ind = value; } }
        public LOCKED Locked { get { return locked; } set { locked = value; } }

        public static int convert(int ind_)
        {
            if (ind_ == 0) return 0;
            else if (ind_ == 3) return 1;
            else if (ind_ == 6) return 2;
            else if (ind_ == 8) return 3;
            else if (ind_ == 10) return 4;
            else if (ind_ == 12) return 5;
            else if (ind_ == 16) return 6;
            else if (ind_ == 17) return 7;
            else if (ind_ == 18) return 8;
            else if (ind_ == 21) return 9;
            else if (ind_ == 22) return 10;
            else if (ind_ == 23) return 11;
            else if (ind_ == 25) return 12;
            else if (ind_ == 26) return 13;
            else if (ind_ == 27) return 14;
            else if (ind_ == 30) return 15;
            else if (ind_ == 31) return 16;
            else if (ind_ == 32) return 17;
            else if (ind_ == 36) return 18;
            else if (ind_ == 38) return 19;
            else if (ind_ == 40) return 20;
            else if (ind_ == 42) return 21;
            else if (ind_ == 45) return 22;
            else if (ind_ == 48) return 23;
            else return -1;
        }

        public static int convertBack(int ind_)
        {
            if (ind_ == 0) return 0;
            else if (ind_ == 1) return 3;
            else if (ind_ == 2) return 6;
            else if (ind_ == 3) return 8;
            else if (ind_ == 4) return 10;
            else if (ind_ == 5) return 12;
            else if (ind_ == 6) return 16;
            else if (ind_ == 7) return 17;
            else if (ind_ == 8) return 18;
            else if (ind_ == 9) return 21;
            else if (ind_ == 10) return 22;
            else if (ind_ == 11) return 23;
            else if (ind_ == 12) return 25;
            else if (ind_ == 13) return 26;
            else if (ind_ == 14) return 27;
            else if (ind_ == 15) return 30;
            else if (ind_ == 16) return 31;
            else if (ind_ == 17) return 32;
            else if (ind_ == 18) return 36;
            else if (ind_ == 19) return 38;
            else if (ind_ == 20) return 40;
            else if (ind_ == 21) return 42;
            else if (ind_ == 22) return 45;
            else if (ind_ == 23) return 48;
            else return -1;
        }
    }

    public enum LOCKED
    {
        YES, NO
    }
}


