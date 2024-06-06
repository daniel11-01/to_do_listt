using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;
using Microsoft.Win32;
using to_do_list.models;
using to_do_list.views;
using to_do_list.ViewModels;
using System.Windows;

namespace to_do_list.viewmodels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private const string PerfilFilePath = "perfil.xml";
        private PerfilModel perfil;

        private bool perfilFoiSalvo;
        public bool PerfilFoiSalvo
        {
            get { return perfilFoiSalvo; }
            set
            {
                if (perfilFoiSalvo != value)
                {
                    perfilFoiSalvo = value;
                    OnPropertyChanged(nameof(PerfilFoiSalvo));
                }
            }
        }

        public ICommand SalvarPerfilCommand { get; }
        public ICommand CarregarFotoCommand { get; }

        public event EventHandler RequestClose;

        public PerfilViewModel()
        {
            perfil = CarregarPerfil() ?? new PerfilModel();
            SalvarPerfilCommand = new RelayCommand(SalvarPerfil);
            CarregarFotoCommand = new RelayCommand(ExecutarCarregarFoto);
        }

        public string Nome
        {
            get { return perfil.Nome; }
            set
            {
                if (perfil.Nome != value)
                {
                    perfil.Nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Email
        {
            get { return perfil.Email; }
            set
            {
                if (perfil.Email != value)
                {
                    perfil.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        private string fotografia;
        public string Fotografia
        {
            get { return fotografia; }
            set
            {
                if (fotografia != value)
                {
                    fotografia = value;
                    OnPropertyChanged(nameof(Fotografia));
                }
            }
        }

        public event EventHandler RequestShowPrincipalView; // Evento para solicitar a exibição da janela principal

        private void SalvarPerfil(object parameter)
        {
            try
            {
                // Atualiza o caminho da imagem no perfil antes de salvar
                perfil.Fotografia = Fotografia;

                XmlSerializer serializer = new XmlSerializer(typeof(PerfilModel));
                using (StreamWriter writer = new StreamWriter(PerfilFilePath))
                {
                    serializer.Serialize(writer, perfil);
                }

                // Definir PerfilFoiSalvo como true após o perfil ser salvo com sucesso
                PerfilFoiSalvo = true;

                // Acionar o evento RequestClose para fechar a janela do perfil
                RequestClose?.Invoke(this, EventArgs.Empty);

                // Acionar o evento RequestShowMainView para mostrar a janela principal
                RequestShowPrincipalView?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao guardar perfil: {ex.Message}");
            }
        }

        private PerfilModel CarregarPerfil()
        {
            try
            {
                if (File.Exists(PerfilFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(PerfilModel));
                    using (StreamReader reader = new StreamReader(PerfilFilePath))
                    {
                        PerfilModel loadedPerfil = (PerfilModel)serializer.Deserialize(reader);

                        // Carrega o caminho da imagem do perfil
                        Fotografia = loadedPerfil.Fotografia;

                        return loadedPerfil;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar perfil: {ex.Message}");
            }

            return null;
        }

        private void ExecutarCarregarFoto(object parameter)
        {
            OpenFileDialog dialogo = new OpenFileDialog
            {
                Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.bmp"
            };
            bool? resultado = dialogo.ShowDialog();

            if (resultado.HasValue && resultado.Value)
            {
                Fotografia = dialogo.FileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
