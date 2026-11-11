using MySqlConnector;
using OralPrime.Model.Financeiro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Material
{
    internal class Compra
    {

        // variáveis
        private int _id;
        private DateTime _data;
        private string _controle;
        private string _nome;
        private bool _finalizado;


        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Controle { get => _controle; set => _controle = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public bool Finalizado { get => _finalizado; set => _finalizado = value; }


        // Método construtor sem ID
        public Compra(DateTime data, string controle, string nome, bool finalizado)
        {
            _data = data;
            _controle = controle;
            _nome = nome;
            _finalizado = finalizado;
        }

        // Método construtor com ID
        public Compra(int id, DateTime data, string controle, string nome, bool finalizado)
        {
            _id = id;
            _data = data;
            _controle = controle;
            _nome = nome;
            _finalizado = finalizado;
        }



        // Inserir
        public static int Inserir(Compra compra)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirCompra(@Data, @Controle, @nome, @Finalizado)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", compra.Data);
                cmd.Parameters.AddWithValue("@NumDocumento", compra.Controle);
                cmd.Parameters.AddWithValue("@DtVencimento", compra.Nome);
                cmd.Parameters.AddWithValue("@Valor", compra.Finalizado);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir


        // Deletar
        public static int Deletar(Compra compra)
        {
            // String SQL
            string sql = "CALL sp_DeletarCompra(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", compra.Id);

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


        }

        private static bool CampoPreenchido(object campo)
        {
            return campo != DBNull.Value && campo != null;
        } // Fim ValidarCampo



        // Listar todos
        public static List<Compra> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosCompra()";

            List<Compra> listaCompra = new();

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

                        DateTime data = (DateTime)reader["Data"];
                        string controle = ValidarCampo(reader["Controle"]);
                        string nome = ValidarCampo(reader["Nome"]);
                        bool finalizado = CampoPreenchido(reader["Finalizado"]);

                        // Cria o objeto Cliente con os dados extraidos
                        Compra compra = new Compra(id, data, controle, nome, finalizado);

                        // Adiciona este boleto à nossa lista
                        listaCompra.Add(compra);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaCompra;

            }

        } //Fim ListarTodos



        // Listar por ID
        public static Compra? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdCompra(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Compra compra = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idLista = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    string controle = ValidarCampo(reader["Controle"]);
                    string nome = ValidarCampo(reader["Nome"]);
                    bool finalizado = CampoPreenchido(reader["Finalizado"]);

                    // Cria o objeto Cliente con os dados extraidos
                    compra = new Compra(idLista, data, controle, nome, finalizado);
                }

                return compra;
            }
        } // Fim ListarPorId



        // Atualizar
        public static int Atualizar(Compra compra)
        {
            string sql = "CALL sp_AtualizarCompra(@Data, @Controle, @nome, @Finalizado)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", compra.Data);
                cmd.Parameters.AddWithValue("@NumDocumento", compra.Controle);
                cmd.Parameters.AddWithValue("@DtVencimento", compra.Nome);
                cmd.Parameters.AddWithValue("@Valor", compra.Finalizado);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar



    } // Classe

} // Namespace
