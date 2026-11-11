using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class ContasAReceber
    {

        // Variáveis
        private int _id;
        private DateTime _data;
        private string _historico;
        private decimal _valor;
        private decimal _saldo;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public decimal Saldo { get => _saldo; set => _saldo = value; }


        // Método construtor sme ID
        public ContasAReceber(DateTime data, string historico, decimal valor, decimal saldo)
        {
            _data = data;
            _historico = historico;
            _valor = valor;
            _saldo = saldo;
        }

        // Método construtor com ID
        public ContasAReceber(int id, DateTime data, string historico, decimal valor, decimal saldo)
        {
            _id = id;
            _data = data;
            _historico = historico;
            _valor = valor;
            _saldo = saldo;
        }



        // Inserir
        public static int Inserir(ContasAReceber contasAReceber)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirContasAReceber(@Data, @Historico, @Valor, @Saldo)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", contasAReceber.Data);
                cmd.Parameters.AddWithValue("@Historico", contasAReceber.Historico);
                cmd.Parameters.AddWithValue("@Valor", contasAReceber.Valor);
                cmd.Parameters.AddWithValue("@Saldo", contasAReceber.Saldo);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar
        public static int Deletar(ContasAReceber contasAReceber)
        {
            // String SQL
            string sql = "CALL sp_DeletarContasAReceber(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAReceber.Id);

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

        }



        // Listar todos
        public static List<ContasAReceber> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosContasAReceber()";

            List<ContasAReceber> listaContasAReceber = new();

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
                        string historico = ValidarCampo(reader["Historico"]);
                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        // Cria o objeto Cliente con os dados extraidos
                        ContasAReceber contasAReceber = new ContasAReceber(id, data, historico, valor, saldo);

                        // Adiciona este boleto à nossa lista
                        listaContasAReceber.Add(contasAReceber);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaContasAReceber;

            }

        } //Fim ListarTodos



        // Listar por ID
        public static ContasAReceber? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdContasAReceber(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                ContasAReceber contasAReceber = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    string historico = ValidarCampo(reader["Historico"]);
                    decimal valor = reader.GetDecimal("Valor");
                    decimal saldo = reader.GetDecimal("Saldo");

                    // Cria o objeto Cliente con os dados extraidos
                    contasAReceber = new ContasAReceber(idListado, data, historico, valor, saldo);
                }

                return contasAReceber;
            }
        } // Fim ListarPorId



        // Atualizar
        public static int Atualizar(ContasAReceber contasAReceber)
        {
            string sql = "CALL sp_AtualizarContaAReceber(@Data, @Historico, @Valor, @Saldo)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", contasAReceber.Data);
                cmd.Parameters.AddWithValue("@Historico", contasAReceber.Historico);
                cmd.Parameters.AddWithValue("@Valor", contasAReceber.Valor);
                cmd.Parameters.AddWithValue("@Saldo", contasAReceber.Saldo);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar



    } // Classe

} // Namespace
