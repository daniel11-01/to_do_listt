using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using to_do_list.models;
using to_do_list.Services;
using to_do_list.ViewModels;
using to_do_list.views;

namespace to_do_list.viewmodels
{
    public class PrincipalViewModel : DependencyObject, INotifyPropertyChanged
    {
        public ObservableCollection<Tarefa> Tarefas { get; } = new ObservableCollection<Tarefa>();

        private Tarefa _tarefaSelecionada;
        public Tarefa TarefaSelecionada
        {
            get { return _tarefaSelecionada; }
            set
            {
                _tarefaSelecionada = value;
                OnPropertyChanged(nameof(TarefaSelecionada));
                ((RelayCommand)ExcluirTarefaCommand).RaiseCanExecuteChanged();
                ((RelayCommand)EditarTarefaCommand).RaiseCanExecuteChanged();
            }
        }

        private void SaveTarefas()
        {
            try
            {
                string filePath = "C:\\Users\\Daniel\\source\\repos\\LPDSW2324\\PL3_G03\\to_do_list\\bin\\Debug\\tarefas.xml";

                List<Tarefa> tarefas = new List<Tarefa>(Tarefas);

                XmlSerializer serializer = new XmlSerializer(typeof(List<Tarefa>));
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, tarefas);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar tarefas: {ex.Message}");
            }
        }

        private Timer _alertTimer;
        private NotificationService _notificationService;

        public PrincipalViewModel()
        {
            _notificationService = new NotificationService();
            _alertTimer = new Timer(60000); // Verificar a cada minuto
            _alertTimer.Elapsed += CheckAlerts;
            _alertTimer.Start();

            CarregarTarefasDeExemplo();
            AdicionarCommand = new RelayCommand(AdicionarTarefa);
            EditarTarefaCommand = new RelayCommand(EditarTarefa, CanEditarTarefa);
            ExcluirTarefaCommand = new RelayCommand(ExcluirTarefa, CanExcluirTarefa);
            EditarPerfilCommand = new RelayCommand(EditarPerfil);
        }

        private void CheckAlerts(object sender, ElapsedEventArgs e)
        {
            foreach (var tarefa in Tarefas)
            {
                if (tarefa.Alerta && tarefa.DataAlerta.HasValue)
                {
                    if (DateTime.Now >= tarefa.DataAlerta.Value && DateTime.Now < tarefa.DataInicio)
                    {
                        // Enviar notificação
                        _notificationService.ShowNotification("Alerta de Tarefa", $"A tarefa '{tarefa.Titulo}' está próxima de iniciar.");

                        // Desativar alerta após envio
                        tarefa.Alerta = false;
                        SaveTarefas();
                    }
                }
            }
        }

        private void CarregarTarefasDeExemplo()
        {
            try
            {
                string filePath = "C:\\Users\\Daniel\\source\\repos\\LPDSW2324\\PL3_G03\\to_do_list\\bin\\Debug\\tarefas.xml";

                if (File.Exists(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Tarefa>));
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        List<Tarefa> tarefas = (List<Tarefa>)serializer.Deserialize(reader);

                        foreach (var tarefa in tarefas)
                        {
                            Tarefas.Add(tarefa);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("O arquivo XML de tarefas não foi encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar tarefas: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AdicionarCommand { get; }
        public ICommand EditarTarefaCommand { get; }
        public ICommand ExcluirTarefaCommand { get; }
        public ICommand EditarPerfilCommand { get; }

        private void AdicionarTarefa(object parameter)
        {
            NovaTarefa novaTarefaWindow = new NovaTarefa();
            novaTarefaWindow.ShowDialog();
        }

        private bool CanExcluirTarefa(object parameter)
        {
            return TarefaSelecionada != null;
        }

        private void ExcluirTarefa(object parameter)
        {
            if (TarefaSelecionada == null)
                return;

            MessageBoxResult result = MessageBox.Show("Tem certeza de que deseja excluir esta tarefa?", "Confirmar Exclusão", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Tarefas.Remove(TarefaSelecionada);
                SaveTarefas(); // Salvar as alterações no arquivo
            }
        }

        private void EditarPerfil(object parameter)
        {
            // Implementação da abertura da janela de edição do perfil aqui
            // Por exemplo:
            var perfilView = new PerfilView();
            perfilView.ShowDialog();
        }

        private bool CanEditarTarefa(object parameter)
        {
            // Este método verifica se uma tarefa está selecionada para edição
            return TarefaSelecionada != null;
        }

        private void EditarTarefa(object parameter)
        {
            // Este método abre uma nova janela para editar a tarefa selecionada
            if (TarefaSelecionada == null)
                return;

            var editarViewModel = new TarefaViewModel(TarefaSelecionada);
            var editarView = new NovaTarefa
            {
                DataContext = editarViewModel
            };

            editarView.ShowDialog();
        }
    }
}
