using NT_GreenSecure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NT_GreenSecure.ViewModels.Popup
{
    public class CredentialDetailViewModel : BaseViewModel
    {
        public Credentials SelectedCredential { get; set; }
        public CredentialDetailViewModel(Credentials selectedCredential)
        {
            Title = "Credential Detail";
            SelectedCredential = selectedCredential;
        }
    }
}
