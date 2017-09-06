using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestProject.ViewModel
{
    class MW_ViewModel : INotifyPropertyChanged
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
                OnPropertyChanged("SelectedStudent");
            }
        }

        private Command addNewItem;
        public Command AddNewItem
        {
            get
            {
                return addNewItem ??

                (addNewItem = new Command(obj =>
                {
                    var EditForm = new EditWindow();
                    var EditFormVM = new EW_ViewModel();
                    EditForm.DataContext = EditFormVM;
                    EditForm.ShowDialog();
                    if (EditForm.DialogResult.Value == true)
                    {
                        var item = new Model.Student();
                        item = EditFormVM.Student;
                        Students.Add(item);
                    }
                }));
            }
        }

        private Command editItem;
        public Command EditItem
        {
            get
            {
                return editItem ??

                (editItem = new Command(obj =>
                {
                    var EditForm = new EditWindow();
                    var EditFormVM = new EW_ViewModel(SelectedStudent);
                    EditForm.DataContext = EditFormVM;
                    EditForm.ShowDialog();
                    if (EditForm.DialogResult.Value == true)
                    {
                        SelectedStudent.Name = EditFormVM.Student.Name;
                        SelectedStudent.Last = EditFormVM.Student.Last;
                        SelectedStudent.Age = EditFormVM.Student.Age;
                        SelectedStudent.Gender = EditFormVM.Student.Gender;
                    }
                }));
            }
        }

        private Command removeItem;
        public Command RemoveItem
        {
            get
            {
                return removeItem ??

                (removeItem = new Command(obj =>
                {
                    if (selectedStudent != null)
                    {
                        if (MessageBox.Show("Запись \"" + SelectedStudent.ViewName + "\" будет удалена\nПродолжить?", 
                            "Удаление записи", 
                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Students.Remove(selectedStudent);
                        }
                    }
                }));
            }
        }

        public MW_ViewModel()
        {
            Students = new ObservableCollection<Model.Student> 
            {
                new Model.Student {Id = 0, Age = 26, Gender = false, Last = "Иванов", Name="Андрей"},
                new Model.Student {Id = 0, Age = 40, Gender = false, Last = "Васильев", Name="Сергей"},
                new Model.Student {Id = 0, Age = 37, Gender = true, Last = "Петрова", Name="Инна"},
                new Model.Student {Id = 0, Age = 21, Gender = false, Last = "Генннов", Name="Иван"}
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
