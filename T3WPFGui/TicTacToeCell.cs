// Copyright (c) 2015 Geeth Tharanga 
// Under the MIT licence - See licence.txt

namespace T3WPFGui
{
    using System.ComponentModel;

    public enum CellType
    {
        Clear, O, X
    }

    public class TicTacToeCell : INotifyPropertyChanged
    {
        private CellType type;

        public CellType Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                Notify("Type");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Row { get; set; }

        public int Col { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}x{2}", this.Type, this.Row, this.Col);
        }

        private void Notify(string field)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(field));
            }
        }
    }
}