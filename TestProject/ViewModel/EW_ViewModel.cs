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

        public EW_ViewModel(Window WinHWDL)
        {
            student = new Model.Student();
            this.WinHWDL = WinHWDL;
        }

        public EW_ViewModel(Model.Student student, Window WinHWDL)
        {
            var ns = new Model.Student
            {
                Id = student.Id,
                Name = student.Name,
                Last = student.Last,
                Age = student.Age,
                Gender = student.Gender
            };
            this.student = ns;
            this.WinHWDL = WinHWDL;
        }

        private Command saveChange;
        public Command SaveChange
        {
            get
            {
                return saveChange ??

                (saveChange = new Command(obj =>
                {
                    var str = "";
                    if (student.Name == "") str += "Поле \"Имя\" должно быть заполнено";
                    if (student.Last == "") str += "\nПоле \"Фамилия\" должно быть заполнено";
                    int age = 0;
                    if (int.TryParse(student.Age.ToString(), out age))
                    {
                        if (age < 16 || age > 100)
                        {
                            str += "\nВозраст должен быть в диапазоне 16...100 лет";
                        }
                    }
                    else
                    {
                        str += "\nВ поле \"Возраст\" неверное значение";
                    }

                    if (str == "")
                    {
                        this.WinHWDL.DialogResult = true;
                        this.WinHWDL.Close();
                    }
                    else
                    {
                        MessageBox.Show(this.WinHWDL, str, "Не верные данные");
                    }
                }));
            }
        }
    }
}
