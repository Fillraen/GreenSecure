using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NT_GreenSecure.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VaultPage : ContentPage
    {
        VaultViewModel _viewModel;
        public ICommand CopyPasswordCommand { get; set; }
        public ICommand DeletePasswordCommand { get; set; }

        public VaultPage()
        {
            InitializeComponent();
            _viewModel = new VaultViewModel();
            CopyPasswordCommand = new Command<int>(_viewModel.CopyPassword);
            DeletePasswordCommand = new Command<int>(_viewModel.DeletePassword);
        }

    }
}