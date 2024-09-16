using Lista_de_tarefas_a_fazer;
using System;
using System.Collections.Generic;
using System.Windows;


namespace Lista_de_Tarefas
{
    public partial class MainWindow : Window
    {
        private List<Tarefa> tarefas = new List<Tarefa>();

        public MainWindow()
        {
            InitializeComponent();
            AtualizarListaTarefas();
        }

        private void AdicionarTarefa_Click(object sender, RoutedEventArgs e)
        {
            string titulo = txtTitulo.Text;
            string descricao = txtDescricao.Text;
            DateTime dataConclusao = dpDataConclusao.SelectedDate ?? DateTime.Now;

            if (!string.IsNullOrEmpty(titulo))
            {
                // Adiciona a tarefa ao banco de dados
                using (var db = new TarefaContext())
                {
                    db.Conectar();
                    db.AdicionarTarefa(titulo, descricao, dataConclusao);
                    db.Desconectar();
                }

                // Atualiza a interface gráfica com a nova tarefa
                AtualizarListaTarefas();
            }
            else
            {
                MessageBox.Show("O título da tarefa não pode estar vazio.");
            }
        }

        private void RemoverTarefa_Click(object sender, RoutedEventArgs e)
        {
            if (lstTarefas.SelectedItem != null)
            {
                var tarefaSelecionada = (Tarefa)lstTarefas.SelectedItem;

                // Remove a tarefa do banco de dados
                using (var db = new TarefaContext())
                {
                    db.Conectar();
                    db.RemoverTarefa(tarefaSelecionada.Id); // Supondo que Tarefa tem uma propriedade Id
                    db.Desconectar();
                }

                // Atualiza a interface gráfica
                AtualizarListaTarefas();
            }
            else
            {
                MessageBox.Show("Selecione uma tarefa para remover.");
            }
        }

        private void AtualizarListaTarefas()
        {
            using (var db = new TarefaContext())
            {
                db.Conectar();
                tarefas = db.ObterTarefas(); // Método que retorna a lista de tarefas do banco de dados
                db.Desconectar();
            }

            lstTarefas.ItemsSource = null;
            lstTarefas.ItemsSource = tarefas;
        }

        private void AlternarTema_Click(object sender, RoutedEventArgs e)
        {
            var temaAtual = Application.Current.Resources.MergedDictionaries[0];

            if (temaAtual.Source.ToString().Contains("TemaClaro.xaml"))
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                {
                    Source = new Uri("TemaEscuro.xaml", UriKind.Relative)
                };
            }
            else
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary()
                {
                    Source = new Uri("TemaClaro.xaml", UriKind.Relative)
                };
            }
        }
    }
}
