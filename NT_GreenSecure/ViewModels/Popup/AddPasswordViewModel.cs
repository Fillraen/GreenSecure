using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NT_GreenSecure.ViewModels.Popup
{
    public class AddPasswordViewModel
    {
        public ICommand AddPasswordCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddPasswordViewModel()
        {
            AddPasswordCommand = new Command(AddPassword);
            CancelCommand = new Command(Cancel);
        }

        private void AddPassword()
        {
            // logique pour ajouter un mot de passe
        }

        private void Cancel()
        {
            // logique pour annuler
        }

    }
}
