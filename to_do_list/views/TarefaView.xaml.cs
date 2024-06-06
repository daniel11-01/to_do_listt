using System.Windows;
using to_do_list.ViewModels;

namespace to_do_list.views
{
    public partial class NovaTarefa : Window
    {
        public NovaTarefa()
        {
            InitializeComponent();
            DataContext = new TarefaViewModel();
        }
    }
}
