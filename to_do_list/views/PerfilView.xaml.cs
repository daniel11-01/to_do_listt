using System.Windows;
using to_do_list.viewmodels;

namespace to_do_list.views
{
    public partial class PerfilView : Window
    {
        public PerfilView()
        {
            InitializeComponent();
            var viewModel = new PerfilViewModel();
            DataContext = viewModel;
            viewModel.RequestClose += (sender, e) => this.Close();
        }
    }
}
