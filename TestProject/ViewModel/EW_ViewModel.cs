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
    class EW_ViewModel
    {
        private Window WinHWDL;

        public EW_ViewModel(Window WinHWDL) : this(new Model.Student(), WinHWDL) { }
        public EW_ViewModel(Model.Student student, Window WinHWDL)
        {
            this.student = (Model.Student)student.Clone();
            this.WinHWDL = WinHWDL;
        }

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

        private Command saveChange;
        public Command SaveChange
        {
            get
            {
                return saveChange ?? (saveChange = new Command(obj =>
                {
                    var str = "";
                    if (student.Name == "") str += "Поле \"Имя\" должно быть заполнено";
                    if (student.Last == "") str += "\nПоле \"Фамилия\" должно быть заполнено";
                    if (student.Age < 16 || student.Age > 100) str += "\nВозраст должен быть в диапазоне 16...100 лет";

                    if (str == "")
                    {
                        this.WinHWDL.DialogResult = true;
                        this.WinHWDL.Close();
                    }
                    else
                        MessageBox.Show(this.WinHWDL, str, "Не верные данные");
                }));
            }
        }
    }
}
