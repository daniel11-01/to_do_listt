using System.ComponentModel;
using System.Xml.Serialization;

namespace to_do_list.models
{
    [XmlRoot("Perfil")]
    public class PerfilModel : INotifyPropertyChanged
    {
        private string nome;
        private string email;
        private string fotografia;

        public string Nome
        {
            get { return nome; }
            set
            {
                if (nome != value)
                {
                    nome = value;
                    OnPropertyChanged(nameof(Nome));
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (email != value)
                {
                    email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
