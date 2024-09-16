using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lista_de_tarefas_a_fazer
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataConclusao { get; set; }
        public bool Concluida { get; set; }


        public Tarefa(int id ,string titulo, DateTime dataconclusao, bool concluida, string descricao = "")
        {
            Id = id;
            Titulo = titulo;
            DataConclusao = dataconclusao;
            Descricao = descricao;
            Concluida = concluida;
        }

        public void MarcarComoConcluida()
        {
            Concluida = true;
        }

        public override string ToString()
        {
            string descricaoInfo = string.IsNullOrEmpty(Descricao) ? "" : $"Descrição: {Descricao} - ";
            return $"{Titulo} - {(Concluida ? "Concluída" : "Pendente")} - Concluir até: " +
                $"{DataConclusao.ToString("DD/MM/YYYY")}";
        }

    }
}
