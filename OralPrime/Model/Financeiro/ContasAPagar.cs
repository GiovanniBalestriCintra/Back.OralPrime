
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
        private int _fornecedorId;
        private DateTime _dtVencimento;
        private decimal _valor;
        private DateTime _dtPagamento;
        private decimal _valorPago;
        private string _historico;
        private bool _final;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public int FornecedorId { get => _fornecedorId; set => _fornecedorId = value; }
        public DateTime DtVencimento { get => _dtVencimento; set => _dtVencimento = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public DateTime DtPagamento { get => _dtPagamento; set => _dtPagamento = value; }
        public decimal ValorPago { get => _valorPago; set => _valorPago = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public bool Final { get => _final; set => _final = value; }

        // Método construtor sem ID
        public ContasAPagar(DateTime data, int fornecedorId, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _data = data;
            _fornecedorId = fornecedorId;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _historico = historico;
            _final = final;
        }

        // Método construtor com ID
        public ContasAPagar(int id, DateTime data, int fornecedorId, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _id = id;
            _data = data;
            _fornecedorId = fornecedorId;
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
            string sql = "CALL sp_InserirContasAPagar(@Data, @FornecedorId, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Historico, @Final)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@FornecedorId", contasAPagar.FornecedorId);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(ContasAPagar contasAPagar)
        {
            string sql = "CALL sp_DeleteContasAPagar(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAPagar.Id);

                return Banco.ExecutarComando(cmd);
            }
        }

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
        public static List<ContasAPagar> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosContasAPagar()";

            List<ContasAPagar> listaContasAPagar = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        int fornecedorId = reader.GetInt32("FornecedorId");
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string historico = ValidarCampo(reader["Historico"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        ContasAPagar contasAPagar = new ContasAPagar(
                            id, data, fornecedorId, dtVencimento,
                            valor, dtPagamento, valorPago, historico, final
                        );

                        listaContasAPagar.Add(contasAPagar);
                    }
                }

                return listaContasAPagar;
            }
        }

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
                        int idListado = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        int fornecedorId = reader.GetInt32("FornecedorId");
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string historico = ValidarCampo(reader["Historico"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        contasAPagar = new ContasAPagar(
                            idListado, data, fornecedorId, dtVencimento,
                            valor, dtPagamento, valorPago, historico, final
                        );
                    }

                    return contasAPagar;
                }
            }
        }

        // Atualizar
        public static int Atualizar(ContasAPagar contasAPagar)
        {
            string sql = "CALL sp_AtualizarContasAPagar(@Id, @Data, @FornecedorId, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Historico, @Final)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAPagar.Id);
                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@FornecedorId", contasAPagar.FornecedorId);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);

                return Banco.ExecutarComando(cmd);
            }
        }

    }
}
