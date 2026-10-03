using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp4
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private string value = "";

        public int Row { get; }
        public int Column { get; }

        public string Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsEnabled));
                }
            }
        }

        public bool IsEnabled =>
            Value == "";

        public CellViewModel(int row, int column)
        {
            Row = row;
            Column = column;
            Value = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
