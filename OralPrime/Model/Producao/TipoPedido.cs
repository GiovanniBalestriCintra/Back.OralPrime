using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class TipoPedido
    {

        private int _id;
        private string _nome;

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }



        public TipoPedido(string nome)
        {
            _nome = nome;
        }

        public TipoPedido(int id, string nome)
        {
            _id = id;
            _nome = nome;
        }


        // Inserir TipoPedido
        public static int Inserir(TipoPedido tipoPedido)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirTipoPedido(@Nome)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", tipoPedido.Nome);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar TipoPedido
        public static int Deletar(TipoPedido tipoPedido)
        {
            // String SQL
            string sql = "CALL sp_DeletarTipoPedido(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", tipoPedido.Id);

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



        // Listar todos os Tipos de Pedido
        public static List<TipoPedido> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosTipoPedido()";

            List<TipoPedido> listaTiposPedido = new();

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
                        TipoPedido tipoPedido = new TipoPedido(id, nome);

                        // Adiciona este tipo de pedido à nossa lista
                        listaTiposPedido.Add(tipoPedido);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaTiposPedido;

            }

        } //Fim ListarTodos



        // Listar tipo de pedido por ID
        public static TipoPedido? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdTipoPedido(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                TipoPedido tipoPedido = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    string nome = ValidarCampo(reader["Nome"]);

                    // Cria o objeto TipoPedido con os dados extraidos
                    tipoPedido = new TipoPedido(idListado, nome);
                }

                return tipoPedido;
            }
        } // Fim ListarPorId



        // Atualizar TipoPedido
        public static int Atualizar(TipoPedido tipoPedido)
        {
            string sql = "CALL sp_AtualizarTipoPedido(@Nome)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", tipoPedido.Nome);

                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", tipoPedido.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar



    } // Classe

} // Namespace
