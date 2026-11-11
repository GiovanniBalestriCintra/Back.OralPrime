using MySqlConnector;
using OralPrime.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class TipoRecibo
    {

        // Variáveis
        private int _id;
        private string _nome;

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }



        // Método construtor sem ID
        public TipoRecibo(string nome)
        {
            _nome = nome;
        }

        //Método construtor com ID
        public TipoRecibo(int id, string nome)
        {
            _id = id;
            _nome = nome;
        }




        // Inserir
        public static int Inserir(TipoRecibo tipoRecibo)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirTipoRecibo(@Nome)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", tipoRecibo.Nome);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir


        // Deletar
        public static int Deletar(TipoRecibo tipoRecibo)
        {
            // String SQL
            string sql = "CALL sp_DeletarTipoRecibo(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", tipoRecibo.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar


        // ValidarCampo
        private static string ValidarCampo(object campo)
        {

            if (campo == DBNull.Value || campo == null)
            {
                return "";
            }
            else
            {
                return campo.ToString();

            }


        } // Fim ValidarCampo


        // Listar Todos
        public static List<TipoRecibo> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosTipoRecibo()";

            List<TipoRecibo> listaTiposRecibo = new();

            using (var cmd = new MySqlCommand(sql))
            {

                // Executa a leitura. 0 bloco 'using' aqui e FUNDAMENTAL para garantir que o
                // reader seja fechado no final, o que por sua vez fecha a conexao com o banco.
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {

                    // O while vai rodar uma vez para CADA Linha que existir na tabela Cliente
                    while (reader.Read())
                    {

                        // Extrai os dados da linha atual.
                        int id = reader.GetInt32("id");

                        string nome = ValidarCampo(reader["Nome"]);


                        // Cria o objeto TipoPedido com os dados extraídos
                        TipoRecibo tipoRecibo = new TipoRecibo(id, nome);

                        // Adiciona este tipo de pedido à nossa lista
                        listaTiposRecibo.Add(tipoRecibo);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaTiposRecibo;

            }

        } //Fim ListarTodos


        // listar por ID
        public static TipoRecibo? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdTipoRecibo(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                TipoRecibo tipoRecibo = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    string nome = ValidarCampo(reader["Nome"]);

                    // Cria o objeto TipoPedido con os dados extraidos
                    tipoRecibo = new TipoRecibo(idListado, nome);
                }

                return tipoRecibo;
            }
        } // Fim ListarPorId


        // Atualizar
        public static int Atualizar(TipoRecibo tipoRecibo)
        {
            string sql = "CALL sp_AtualizarTipoRecibo(@Nome)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", tipoRecibo.Nome);

                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", tipoRecibo.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar







    } // Classe

} // Namespace
