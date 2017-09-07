using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace TestProject.ViewModel
{
    class MW_ViewModel : INotifyPropertyChanged
    {
        private Window WinHandle;
        private XDocument xmlDoc;

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

        public bool IntefaceEnabled
        {
            get
            {
                return Students.Count == 0 ? false : true;
            }
        }

        public int Count
        {
            get
            {
                return Students.Count;
            }
            private set {}
        }

        private bool? OpenDialog(Model.Student student, out ViewModel.EW_ViewModel VM)
        {
            var EditForm = new EditWindow();
            var EditFormVM = new EW_ViewModel(student, EditForm);
            EditForm.DataContext = EditFormVM;
            EditForm.Owner = WinHandle;
            EditForm.ShowDialog();
            VM = EditFormVM;
            return EditForm.DialogResult.Value;
        }

        private Command addNewItem;
        public Command AddNewItem
        {
            get
            {
                return addNewItem ?? (addNewItem = new Command(obj =>
                {
                    ViewModel.EW_ViewModel VM;
                    if (OpenDialog(new Model.Student(), out VM).Value == true)
                    {
                        var item = (Model.Student)VM.Student.Clone();
                        Students.Add(item);
                        OnPropertyChanged("Count");
                        OnPropertyChanged("IntefaceEnabled");

                        if (xmlDoc != null)
                        {
                            xmlDoc.Root.Add(new XElement("Student",
                                 new XAttribute("Id", "-1"),
                                 new XElement("FirstName", VM.Student.Name),
                                 new XElement("Last", VM.Student.Last),
                                 new XElement("Age", VM.Student.Age.ToString()),
                                 new XElement("Gender", VM.Student.Gender ? "1" : "0")));

                            int i = 0;
                            foreach (XElement ex in xmlDoc.Root.Elements("Student"))
                            {
                                ex.Attribute("Id").Value = i.ToString();
                                i++;
                            }
                            xmlDoc.Save("./Students.xml");
                        }
                    }
                }));
            }
        }

        private Command editItem;
        public Command EditItem
        {
            get
            {
                return editItem ?? (editItem = new Command(obj =>
                {
                    ViewModel.EW_ViewModel VM;
                    if (SelectedStudent != null && OpenDialog(SelectedStudent, out VM).Value == true)
                    {
                        SelectedStudent.Id = VM.Student.Id;
                        SelectedStudent.Name = VM.Student.Name;
                        SelectedStudent.Last = VM.Student.Last;
                        SelectedStudent.Age = VM.Student.Age;
                        SelectedStudent.Gender = VM.Student.Gender;

                        if (xmlDoc != null)
                        {
                            foreach (XElement ex in xmlDoc.Root.Elements("Student"))
                            {
                                int id = Int32.Parse(ex.Attribute("Id").Value);
                                if (id == VM.Student.Id)
                                {
                                    ex.Element("FirstName").Value = VM.Student.Name;
                                    ex.Element("Last").Value = VM.Student.Last;
                                    ex.Element("Age").Value = VM.Student.Age.ToString();
                                    ex.Element("Gender").Value = VM.Student.Gender ? "1" : "0";
                                    break;
                                }
                            }
                            xmlDoc.Save("./Students.xml");
                        }

                        SelectedStudent.OnPropertyChanged("ViewName");
                        SelectedStudent.OnPropertyChanged("ViewAge");
                        SelectedStudent.OnPropertyChanged("ViewGender");
                    }
                }));
            }
        }

        private Command removeItem;
        public Command RemoveItem
        {
            get
            {
                return removeItem ?? (removeItem = new Command(obj =>
                {
                    if (selectedStudent != null)
                    {
                        if (MessageBox.Show(
                            "Запись \"" + SelectedStudent.ViewName + "\" будет удалена\nПродолжить?", 
                            "Удаление записи", 
                            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            if (xmlDoc != null)
                            {
                                foreach (XElement ex in xmlDoc.Root.Elements("Student"))
                                {
                                    int id = Int32.Parse(ex.Attribute("Id").Value);
                                    if (id == selectedStudent.Id)
                                    {
                                        ex.Remove();
                                        break;
                                    }
                                }
                                xmlDoc.Save("./Students.xml");
                            }

                            Students.Remove(selectedStudent);
                            OnPropertyChanged("Count");
                            OnPropertyChanged("IntefaceEnabled");
                        }
                    }
                }));
            }
        }

        private Command closeApp;
        public Command CloseApp
        {
            get
            {
                return closeApp ?? (closeApp = new Command(obj => Application.Current.Shutdown() ));
            }
        }

        public MW_ViewModel(Window HWDL)
        {
            this.WinHandle = HWDL;
            Students = new ObservableCollection<Model.Student>();

            try
            {
                xmlDoc = XDocument.Load("./Students.xml");
                var items = from xe in xmlDoc.Element("Students").Elements("Student")
                            select new Model.Student
                            {
                                Id = int.Parse(xe.Attribute("Id").Value),
                                Name = xe.Element("FirstName").Value,
                                Last = xe.Element("Last").Value,
                                Age = int.Parse(xe.Element("Age").Value),
                                Gender = (xe.Element("Gender").Value.Equals("0") ? false : true)
                            };

                foreach (var item in items)
                {
                    Students.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
