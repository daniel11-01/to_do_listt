using System.IO;
using System.Windows.Input;
using System;
using to_do_list.models;
using to_do_list.ViewModels;
using to_do_list;
using System.Windows;
using to_do_list.viewmodels;
using to_do_list.views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;


public class TarefaViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tarefas.xml");
    private string _counterFilePath = Path.Combine(Directory.GetCurrentDirectory(), "contador_id.txt");

    private static int _idCounter;
    public int ID { get; set; }
    public string Titulo { get; set; }
    public string Periodicidade { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFim { get; set; }
    public DateTime DataFim { get; set; }
    public string Prioridade { get; set; }
    public string Descricao { get; set; }
    public bool Alerta { get; set; }
    public DateTime? DataAlerta { get; set; }
    public ICommand ConcluirCommand { get; }

    public TarefaViewModel()
    {
        ConcluirCommand = new RelayCommand(Concluir);
        InitializeIdCounter();
    }

    public TarefaViewModel(Tarefa tarefa) : this()
    {
        if (tarefa != null)
        {
            ID = tarefa.ID;
            Titulo = tarefa.Titulo;
            Periodicidade = tarefa.Periodicidade;
            DataInicio = tarefa.DataInicio;
            HoraInicio = tarefa.HoraInicio;
            DataFim = tarefa.DataFim;
            HoraFim = tarefa.HoraFim;
            Prioridade = tarefa.Prioridade;
            Descricao = tarefa.Descricao;
            Alerta = tarefa.Alerta;
            DataAlerta = tarefa.DataAlerta;
        }
    }

    private void InitializeIdCounter()
    {
        if (File.Exists(_counterFilePath))
        {
            string counterValue = File.ReadAllText(_counterFilePath);
            _idCounter = int.Parse(counterValue);
        }
        else
        {
            _idCounter = 1;
        }
    }

    private void UpdateIdCounter()
    {
        File.WriteAllText(_counterFilePath, _idCounter.ToString());
    }

    private void Concluir(object parameter)
    {

        //

        // Crie uma nova tarefa com os dados fornecidos
        var tarefa = new Tarefa
        {
            ID = ID == 0 ? _idCounter++ : ID,
            Titulo = Titulo,
            Periodicidade = Periodicidade,
            DataInicio = DataInicio,
            HoraInicio = HoraInicio,
            DataFim = DataFim,
            HoraFim = HoraFim,
            Prioridade = Prioridade,
            Descricao = Descricao,
            Alerta = Alerta,
            DataAlerta = Alerta ? DataInicio.Add(HoraInicio.TimeOfDay).AddDays(-1) : (DateTime?)null,

        };

        var tarefas = TarefaManager.LoadTarefas(_filePath);

        if (ID == 0)
        {
            tarefas.Add(tarefa);
            UpdateIdCounter();
        }
        else
        {
            var index = tarefas.FindIndex(t => t.ID == ID);
            if (index != -1)
            {
                tarefas[index] = tarefa;
            }
        }

        TarefaManager.SaveTarefas(tarefas, _filePath); 
    }

        private Visibility _tarefaViewVisibility = Visibility.Visible;

        public Visibility TarefaViewVisibility
        {
            get { return _tarefaViewVisibility; }
            set
            {
                _tarefaViewVisibility = value;
                OnPropertyChanged(nameof(TarefaViewVisibility));
            }
        }


}

