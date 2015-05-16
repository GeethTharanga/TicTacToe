using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace T3WPFGui
{
    public enum CellType
    {
        Clear,O,X
    }
    //Cell Mark Command
   

    //public class MarkCellCommand : ICommand 
    //{
        
    //    public bool CanExecute(object parameter)
    //    {
    //        return false;
    //    }

    //    public event EventHandler CanExecuteChanged;

    //    public void Execute(object parameter)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void RaiseChanged()
    //    {
    //        if (CanExecuteChanged != null)
    //            CanExecuteChanged(this, new EventArgs());
    //    }
    //}

    public class TicTacToeCell : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private CellType _type;

        public CellType Type
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
