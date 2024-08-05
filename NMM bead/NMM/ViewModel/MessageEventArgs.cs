using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMM.ViewModel
{
    public class MessageEventArgs : EventArgs
    {
        private string str;
        public string _str { get { return _str; } }
        public MessageEventArgs(string text)
        {
            str = text;
        }
    }
}
