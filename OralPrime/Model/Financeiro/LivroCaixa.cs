using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class LivroCaixa
    {

        // Váriaveis
        private int _id;
        private DateTime _data;
        private string _historico;
        private char _entradaSaida;
        private decimal _valor;
        private decimal _saldo;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public char EntradaSaida { get => _entradaSaida; set => _entradaSaida = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public decimal Saldo { get => _saldo; set => _saldo = value; }


        // Método construtor sem ID
        public LivroCaixa(DateTime data, string historico, char entradaSaida, decimal valor, decimal saldo)
        {
            _data = data;
            _historico = historico;
            _entradaSaida = entradaSaida;
            _valor = valor;
            _saldo = saldo;
        }

        // Método construtor com ID
        public LivroCaixa(int id, DateTime data, string historico, char entradaSaida, decimal valor, decimal saldo)
        {
            _id = id;
            _data = data;
            _historico = historico;
            _entradaSaida = entradaSaida;
            _valor = valor;
            _saldo = saldo;
        }

        // Inserir
        public static int Inserir(LivroCaixa livroCaixa)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirLivroCaixa(@Data, @Historico, @EntradaSaida, @Valor, @Saldo)";
            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", livroCaixa.Data);
                cmd.Parameters.AddWithValue("@NumDocumento", livroCaixa.Historico);
                cmd.Parameters.AddWithValue("@DtVencimento", livroCaixa.EntradaSaida);
                cmd.Parameters.AddWithValue("@Valor", livroCaixa.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", livroCaixa.Saldo);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar
        public static int Deletar(LivroCaixa livroCaixa)
        {
            // String SQL
            string sql = "CALL sp_DeletarLivroCaixa(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", livroCaixa.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar



        // ValidarCampo string
        private static string ValidarCampoString(object campo)
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


        // ValidarCampo string char
        private static char ValidarCampoChar(object campo)
        {
            if (campo == DBNull.Value || campo == null)
            {
                return '\0'; // caractere nulo
            }
            else
            {
                return Convert.ToChar(campo);
            }
        }


        private static bool CampoPreenchido(object campo)
        {
            return campo != DBNull.Value && campo != null;
        } // Fim ValidarCampo



        // Listar todos
        public static List<LivroCaixa> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosLivroCaixa()";

            List<LivroCaixa> listaLivroCaixa = new();

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
                        string historico = ValidarCampoString(reader["Historico"]);
                        char entradaSaida = ValidarCampoChar(reader["EntradaSaida"]);
                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        // Cria o objeto Cliente con os dados extraidos
                        LivroCaixa livroCaixa = new LivroCaixa(id, data, historico, entradaSaida, valor, saldo);

                        // Adiciona este boleto à nossa lista
                        listaLivroCaixa.Add(livroCaixa);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaLivroCaixa;

            }

        } //Fim ListarTodos



        // Listar por ID
        public static LivroCaixa? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdLivroCaixa(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                LivroCaixa livroCaixa = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idLista = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    string historico = ValidarCampoString(reader["Historico"]);
                    char entradaSaida = ValidarCampoChar(reader["EntradaSaida"]);
                    decimal valor = reader.GetDecimal("Valor");
                    decimal saldo = reader.GetDecimal("Saldo");

                    // Cria o objeto Cliente con os dados extraidos
                    livroCaixa = new LivroCaixa(idLista, data, historico, entradaSaida, valor, saldo);
                }

                return livroCaixa;
            }
        } // Fim ListarPorId



        // Atualizar
        public static int Atualizar(LivroCaixa livroCaixa)
        {
            string sql = "CALL sp_AtualizarLivroCaixa(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", livroCaixa.Data);
                cmd.Parameters.AddWithValue("@NumDocumento", livroCaixa.Historico);
                cmd.Parameters.AddWithValue("@DtVencimento", livroCaixa.EntradaSaida);
                cmd.Parameters.AddWithValue("@Valor", livroCaixa.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", livroCaixa.Saldo);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    } // Classe

} // Namespace
