using MySqlConnector;
using OralPrime.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class ContasAPagar
    {

        // Variáveis
        private int _id;
        private DateTime _data;
        private DateTime _dtVencimento;
        private decimal _valor;
        private DateTime _dtPagamento;
        private decimal _valorPago;
        private string _historico;
        private bool _final;


        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public DateTime DtVencimento { get => _dtVencimento; set => _dtVencimento = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public DateTime DtPagamento { get => _dtPagamento; set => _dtPagamento = value; }
        public decimal ValorPago { get => _valorPago; set => _valorPago = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public bool Final { get => _final; set => _final = value; }


        // Método construtor sem ID
        public ContasAPagar(DateTime data, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _data = data;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _historico = historico;
            _final = final;
        }


        // Método construtor com ID
        public ContasAPagar(int id, DateTime data, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _id = id;
            _data = data;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _historico = historico;
            _final = final;
        }



        // Inserir
        public static int Inserir(ContasAPagar contasAPagar)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // O uso de marcações com '@' (ex: @Data) é uma prática de segurança crucial chamada de "Parametrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que é comando e o que é apenas texto do usuário.

            string sql = "CALL sp_InserirContasAPagar(@Data, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Historico, @Final)";

            // Gerenciamento de recursos: Cria o objeto de comando já passando a instrução SQL.
            // O 'using' garante que os recursos de memória ocupados pelo comando sejam liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (contasAPagar.Data, contasAPagar.Valor, etc.)
                // e substitui de forma segura as marcações '@' da string SQL com os valores reais.

                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);

                // Execução: Como o comando de INSERT não retorna tabelas (apenas executa uma ação),
                // ele envia o comando configurado para o método genérico 'ExecutarComando' da classe 'Banco'.
                // O método retorna 1 (se inserido com sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar
        public static int Deletar(ContasAPagar contasAPagar)
        {
            // String SQL
            string sql = "CALL sp_DeleteContasAPagar(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAPagar.Id);

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
        public static List<ContasAPagar> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosContasAPagar()";

            List<ContasAPagar> listaContasAPagar = new();

            using (var cmd = new MySqlCommand(sql))
            {

                // Executa a leitura. O bloco 'using' aqui é FUNDAMENTAL para garantir que o
                // reader seja fechado no final, o que por sua vez fecha a conexão com o banco.
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {

                    // O while vai rodar uma vez para CADA linha que existir na tabela
                    while (reader.Read())
                    {

                        // Extrai os dados da linha atual.
                        int id = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string historico = ValidarCampo(reader["Historico"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        // Cria o objeto ContasAPagar com os dados extraídos
                        ContasAPagar contasAPagar = new ContasAPagar(id, data, dtVencimento, valor, dtPagamento, valorPago, historico, final);

                        // Adiciona esta conta à nossa lista
                        listaContasAPagar.Add(contasAPagar);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista com 0 itens)
                return listaContasAPagar;

            }

        } // Fim ListarTodos



        // Listar por ID
        public static ContasAPagar? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdContasAPagar(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    ContasAPagar? contasAPagar = null;

                    while (reader.Read())
                    {

                        // Extrai os dados da linha atual.
                        int idListado = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string historico = ValidarCampo(reader["Historico"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        // Cria o objeto ContasAPagar com os dados extraídos
                        contasAPagar = new ContasAPagar(idListado, data, dtVencimento, valor, dtPagamento, valorPago, historico, final);
                    }

                    return contasAPagar;
                }
            }
        } // Fim ListarPorId



        // Atualizar
        public static int Atualizar(ContasAPagar contasAPagar)
        {
            string sql = "CALL sp_AtualizarContasAPagar(@Data, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Historico, @Final)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculação de parâmetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Id", contasAPagar.Id);
                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);

                // Execução: O UPDATE também é uma instrução de ação (como INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou com sucesso) ou 0 (se o ID não existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    } // Classe

} // Namespace