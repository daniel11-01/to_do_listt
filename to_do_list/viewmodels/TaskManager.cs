using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using to_do_list.models;

namespace to_do_list
{
    public static class TarefaManager
    {
        public static List<Tarefa> LoadTarefas(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<Tarefa>();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(List<Tarefa>));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (List<Tarefa>)serializer.Deserialize(reader);
            }
        }

        public static void SaveTarefas(List<Tarefa> tarefas, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tarefa>));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, tarefas);
            }
        }
    }
}
