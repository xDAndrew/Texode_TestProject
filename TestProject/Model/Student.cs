using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Model
{
    public class Student : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private string last;
        private int age;
        private bool gender;

        public Student()
        {
            id = -1;
            name = "";
            last = "";
            age = 16;
            gender = false;
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Last
        {
            get
            {
                return last;
            }
            set
            {
                last = value;
                OnPropertyChanged("Last");
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                OnPropertyChanged("Age");
            }
        }

        public bool Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public string ViewName
        {
            get
            {
                return name + " " + last;
            }
            private set {}
        }

        public string ViewAge
        {
            get
            {
                var str = age.ToString();

                switch (age % 10)
                {
                    case 1:
                        str += " год";
                        break;
                    case 2:
                    case 3:
                    case 4:
                        str += " года";
                        break;
                    default:
                        str += " лет";
                        break;
                }

                return str;
            }
            private set {}
        }
        
        public string ViewGender
        {
            get
            {
                if (gender)
                    return "Женщина";
                else
                    return "Мужчина";
            }
            private set {}
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
