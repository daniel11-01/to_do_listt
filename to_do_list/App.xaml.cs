using System;
using System.Windows;
using to_do_list.viewmodels;
using to_do_list.views;

namespace to_do_list
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Abrir a janela do perfil 
            PerfilViewModel perfilViewModel = new PerfilViewModel();
            PerfilView perfilWindow = new PerfilView();
            perfilWindow.DataContext = perfilViewModel;

            // Mostrar a janela do perfil e aguardar até que seja fechada
            bool? perfilResult = perfilWindow.ShowDialog();

            // Verificar se a janela do perfil foi fechada corretamente e o perfil foi salvo
            if (perfilResult == true && perfilViewModel.PerfilFoiSalvo)
            {
                // Abrir a janela "MainWindow" apenas se o perfil foi salvo
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            base.OnStartup(e);

            PrincipalView principalView = new PrincipalView();
            principalView.Show();
        }
    }
}
