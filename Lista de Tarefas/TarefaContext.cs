using Lista_de_tarefas_a_fazer;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Lista_de_Tarefas
{
    public class TarefaContext : IDisposable
    {
        private MySqlConnection connection;
        private bool disposed = false;

        public TarefaContext()
        {
            string connectionString = "Server=localhost;Database=gerenciadordetarefas;Uid=root;";
            connection = new MySqlConnection(connectionString);
        }

        public void Conectar()z
        {
            connection.Open();
        }

        public void Desconectar()
        {
            connection.Close();
        }


        // Adiciona uma tarefa ao banco de dados
        public void AdicionarTarefa(string titulo, string descricao, DateTime dataConclusao)
        {

            try
            {

                string query = "INSERT INTO Tarefas (Titulo, Descricao, DataConclusao, Concluida) VALUES (@titulo, @descricao, @dataConclusao, @concluida)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@dataConclusao", dataConclusao);
                cmd.Parameters.AddWithValue("@concluida", false);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar tarefa: {ex.Message}");
            }
        }

        // Remove uma tarefa do banco de dados
        public void RemoverTarefa(int id)
        {
            string query = "DELETE FROM Tarefas WHERE Id = @id";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }

        // Busca todas as tarefas do banco de dados
        public List<Tarefa> ObterTarefas()
        {
            var tarefas = new List<Tarefa>();

            try
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    Conectar();
                }

                string query = "SELECT * FROM Tarefas";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tarefa = new Tarefa(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Titulo")),
                            reader.GetDateTime(reader.GetOrdinal("DataConclusao")),
                            reader.GetBoolean(reader.GetOrdinal("Concluida")),
                            reader.GetString(reader.GetOrdinal("Descricao"))
                        );

                        tarefas.Add(tarefa);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar tarefas: {ex.Message}");
            }
            finally
            {
                Desconectar();
            }

            return tarefas;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }

                disposed = true;
            }
        }

        ~TarefaContext()
        {
            Dispose(false);
        }
    }
}
