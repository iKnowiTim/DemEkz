using DemExam.ModelDb;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DemExam.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private DemServerEntities db;
        private MainWindowViewModel mainWindowViewModel;
        public MainPageViewModel(MainWindowViewModel main) // Инициализация
        {
            mainWindowViewModel = main;
            db = new DemServerEntities(); // Подключение к БД
            db.Client_Import_good.Load(); // Загружаем даннные
            LoadOtherLib(); // Метод загрузки данных в таблицу
            InitCommand(); // Инициализация команд
        }

        private void InitCommand()
        {
            NextPageCommand = new RelayCommand(NextPage, X => CanNextPageCommandExecute()); // След страница
            PrevPageCommand = new RelayCommand(PrevPage, x => Count != 0); // Пред страница
            OutTenCommand = new RelayCommand(OutTen, x => true); // Вывод 10 клиентов на страницу
            OutFiftyCommand = new RelayCommand(OutFifty, x => true); // Вывод 50 клиентов на страницу
            OutOneHungCommand = new RelayCommand(OutOneHung, x => true); // Вывод 200 клиентов на страницу
            OutAllCommand = new RelayCommand(OutAll, x => true); // Вывод всех клиентов на страницу
        }

        private void OutAll() // метод вывода всех клиентов
        {
            PerPage = size; 
            InfoPerPage = PerPage;
            Count = 0; // Первая страница
            LoadOtherLib(); // Загружаем в таблицу данные из бд
        }

        public int Size // Свойство размера клиентов в БД
        {
            get { return size; }
            set 
            {
                size = value;
                OnPropertyChanged(nameof(Size));
            }
        }

        private int genderFilter;
        public int GenderFilter
        {
            get { return genderFilter; }
            set
            {
                genderFilter = value;
                LoadOtherLib();
                OnPropertyChanged(nameof(GenderFilter));
            }
        }


        private bool canExecuteNextPage = true;
        public bool CanExecuteNextPage
        {
            get { return canExecuteNextPage; }
            set 
            {
                canExecuteNextPage = value;
                OnPropertyChanged(nameof(CanExecuteNextPage));
            }
        }

        public bool CanNextPageCommandExecute() // Метод изменения состояния кнопки
        {
            if (size / PerPage > Count)
            {
                return CanExecuteNextPage = true;
            }
            else
            {
                return CanExecuteNextPage = false;
            }
        }


        private void OutOneHung() // метод вывода 200 клиентов
        {
            if (size < 200)
            {
                PerPage = size;
            }
            else
            {
                PerPage = 200;
            }            
            InfoPerPage = PerPage;
            Count = 0;
            LoadOtherLib();
        }

        private void OutFifty()
        {
            PerPage = 50;
            InfoPerPage = PerPage;
            Count = 0;
            LoadOtherLib();
        }// метод вывода 50 клиентов

        private void OutTen()
        {
            PerPage = 10;
            InfoPerPage = PerPage;
            Count = 0;
            LoadOtherLib();
        } // метод вывода 10 клиентов

        private void PrevPage()
        {
            
            if (InfoPerPage != size)
            {
                InfoPerPage -= PerPage;
            } else
            {
                InfoPerPage -= size / (PerPage * Count);
            }
            Count--;

            LoadOtherLib();
        } // Метод перехода на прошлую страницу

        private void LoadOtherLib() // Загрузка 
        {
            ListClients = (from f in db.Client_Import_good
                           join l in db.Tags
                             on f.TagID equals l.ID
                             into m
                           from mo in m.DefaultIfEmpty()
                           orderby f.ID
                           select new
                           {
                               Name = f.Name,
                               ID = f.ID,
                               Surname = f.Surname,
                               DateOfBirth = f.DateOfBirth,
                               Gender = f.Gender,
                               Email = f.Email,
                               Phone = f.Phone,
                               LastName = f.LastName,
                               Tags = mo.Title,
                               RegistrationDate = f.RegistrationDate
                           }
                           ).Where(a => GenderFilter > 0 ? (a.Gender == "м" && GenderFilter == 1) || (a.Gender == "ж" && GenderFilter == 2) : true                            
                           ).Skip(count * PerPage).Take(PerPage).ToList();
            size = db.Client_Import_good.Count();
        }        

        private int perPage = 10;
        public int PerPage
        {
            get { return perPage; }
            set 
            {
                perPage = value;
                OnPropertyChanged(nameof(PerPage));
            }
        }

        private int infoPerPage = 10;
        public int InfoPerPage
        {
            get { return infoPerPage; }
            set 
            {
                infoPerPage = value;
                OnPropertyChanged(nameof(InfoPerPage));
            }
        }


        private int count = 0;
        public int Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        int size;

        private IEnumerable listClients;
        public IEnumerable ListClients
        {
            get { return listClients; }
            set
            {
                listClients = value;
                OnPropertyChanged(nameof(ListClients));
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set 
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int id;
        public int ID
        {
            get { return id; }
            set 
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set 
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private char gender;
        public char Gender
        {
            get { return gender; }
            set 
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string genderName;
        public string GenderName
        {
            get { return genderName; }
            set 
            { 
                genderName = value;
                OnPropertyChanged(nameof(GenderName));
            }
        }

        public ICommand NextPageCommand { get; set; }
        public ICommand PrevPageCommand { get; set; }
        public ICommand OutTenCommand { get; set; }
        public ICommand OutFiftyCommand { get; set; }
        public ICommand OutOneHungCommand { get; set; }
        public ICommand OutAllCommand { get; set; }

        public void NextPage()
        {
            Count++;
            InfoPerPage += PerPage;
            if (InfoPerPage > size)
            {
                InfoPerPage = size;
            }
            LoadOtherLib();
        }

        private string surname;
        public string Surname
        {
            get { return surname; }
            set 
            { 
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set 
            {
                dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
            }
        }

        private string phone;
        public string Phone
        {
            get { return phone; }
            set 
            {
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string email;
        public string Email
        {
            get { return email; }
            set 
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private DateTime registrationDate;
        public DateTime RegistrationDate
        {
            get { return registrationDate; }
            set 
            {
                registrationDate = value;
                OnPropertyChanged(nameof(RegistrationDate));
            }
        }

        private int tagId;
        public int TagID
        {
            get { return tagId; }
            set 
            { 
                tagId = value;
                OnPropertyChanged(nameof(TagID));
            }
        }

        private Tag tags;
        public Tag Tags
        {
            get { return tags; }
            set 
            {
                tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }




    }
}
