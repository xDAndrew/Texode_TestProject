using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.ViewModel
{
    class StudentViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Model.Student> Students { get; set; }
        private Model.Student selectedStudent;

        public Model.Student SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedPhone");
            }
        }

        public StudentViewModel()
        {
            Students = new ObservableCollection<Model.Student>
            {
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
