using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestProject.ViewModel
{
    class EW_ViewModel : INotifyPropertyChanged
    {
        private Model.Student student;
        public Model.Student Student
        {
            get
            {
                return student;
            }
            set
            {
                student = value;
            }
        }

        public EW_ViewModel()
        {
            student = new Model.Student();
        }

        public EW_ViewModel(Model.Student student)
        {
            Student = student;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
