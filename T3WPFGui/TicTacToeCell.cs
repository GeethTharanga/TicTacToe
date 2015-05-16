using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T3WPFGui
{
    public enum CellType
    {
        O,X
    }
    public class TicTacToeCell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CellType? _type;

        public CellType? Type
        {
            get { return _type; }
            set { _type = value; Notify("Type"); }
        }

        public int Row { get; set; }
        public int Col { get; set; }

        private void Notify(string field)
        {
            if(PropertyChanged != null )
                 PropertyChanged(this, new PropertyChangedEventArgs(field));
        }

        public override string ToString()
        {
            return String.Format("{0} : {1}x{2}", Type, Row, Col);
        }
    }
}
