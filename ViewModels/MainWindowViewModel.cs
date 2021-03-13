using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemExam.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            InitPages();
            currentPage = mainPage;
        }

        private void InitPages()
        {
            mainPage = new MainPageViewModel(this);
        }

        private MainPageViewModel mainPage;

        private BaseViewModel currentPage;
        public BaseViewModel CurrentPage
        {
            get { return currentPage; }
            set 
            { 
                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
    }
}
