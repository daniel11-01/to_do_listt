using System;

namespace to_do_list.models
{
    public class AlertaModel
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataHora { get; set; }
        public string Tipos { get; set; }
        public bool Desligado { get; set; }
    }
}
