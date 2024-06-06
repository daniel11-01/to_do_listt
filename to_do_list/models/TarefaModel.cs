using System;

namespace to_do_list.models
{
    public class Tarefa
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Periodicidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime HoraFim { get; set; }
        public string Prioridade { get; set; }
        public string Descricao { get; set; }
        public bool Alerta { get; set; }
        public DateTime? DataAlerta { get; set; }
        

    }
}

