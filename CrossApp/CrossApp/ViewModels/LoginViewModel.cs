using CrossApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossApp.ViewModels
{
    class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidLoginPrompt;
        public Action StartLoader;
        public Action StopLoader;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string email;
        private string password;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EmailEntry"));
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PasswordEntry"));
            }
        }
        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
           SubmitCommand =  new Command(OnSubmit);
        }
        public void OnSubmit()
        {
           CheckLoginAsync();
        }

        public async Task CheckLoginAsync()
        {
            StartLoader();
            await Task.Delay(2000);
            StopLoader();

            var user = new { email = "opti@optisoft.it", password = "opti" };

            if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(Password))
            {
                if (Email.Trim().Equals("opti@optisoft.it") && Password.Trim().Equals("opti"))
                    ((App)Application.Current).OnLogin();
                else
                    DisplayInvalidLoginPrompt();
            }
            else
                await App.Current.MainPage.DisplayAlert("Error", "insert all info", "OK");
        }
    }
}
