using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NMMModel.Model
{
    public class NMMMoveToEventArgs : EventArgs
    {
        private ColorFields _color;
        private int _ind;
        public ColorFields color { get { return _color; } }
        public int ind { get { return _ind; } }
        public NMMMoveToEventArgs(ColorFields color, int ind)
        {
            _color = color;
            _ind = ind;
        }
    }
    public class NMMFromToEventArgs : EventArgs
    {
        private ColorFields _color;
        private int _ind;
        private int _transpInd;

        public ColorFields color { get { return _color; } }
        public int ind { get { return _ind; } }
        public int transpInd { get { return _transpInd; } }
        public NMMFromToEventArgs(ColorFields color, int ind, int transpInd)
        {
            _color = color;
            _ind = ind;
            _transpInd = transpInd;
        }
    }

    public class NMMRemoveEventArgs : EventArgs
    {
        private int _transpInd;

        public int transpInd { get { return _transpInd; } }
        public NMMRemoveEventArgs(int transpInd)
        {
            _transpInd = transpInd;
        }
    }

    public class NMMGameOverEventArgs : EventArgs {
        string _winner;
        public NMMGameOverEventArgs(string winner)
        {
            this._winner= winner;
        }
        public string winner { get { return _winner; } }
    }
    public class NMMChangeSatusEventArgs : EventArgs
    {
        private string _status;
        public string status { get { return _status; } }
        public NMMChangeSatusEventArgs(string status)
        {
            _status = status;
        }
    }
    public enum ColorFields
    {
        GREEN = 0, BLUE, TRANSP
    }
    public enum Turns
    {
        GREEN, BLUE
    }
    public enum Move
    {
        CHOOSE, PLACE
    }
}
