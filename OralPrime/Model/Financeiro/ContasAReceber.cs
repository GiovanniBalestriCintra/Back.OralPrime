
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
        private int _clienteId;
        private int _pedidoId;
        private DateTime _data;
        private string _historico;
        private decimal _valor;
        private decimal _saldo;

        public int Id { get => _id; set => _id = value; }
        public int ClienteId { get => _clienteId; set => _clienteId = value; }
        public int PedidoId { get => _pedidoId; set => _pedidoId = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public decimal Saldo { get => _saldo; set => _saldo = value; }

        // Método construtor sem ID
        public ContasAReceber(int clienteId, int pedidoId, DateTime data, string historico, decimal valor, decimal saldo)
        {
            _clienteId = clienteId;
            _pedidoId = pedidoId;
            _data = data;
            _historico = historico;
            _valor = valor;
            _saldo = saldo;
        }

        // Método construtor com ID
        public ContasAReceber(int id, int clienteId, int pedidoId, DateTime data, string historico, decimal valor, decimal saldo)
        {
            _id = id;
            _clienteId = clienteId;
            _pedidoId = pedidoId;
            _data = data;
            _historico = historico;
            _valor = valor;
            _saldo = saldo;
        }

        // Inserir
        public static int Inserir(ContasAReceber contasAReceber)
        {
            string sql = "CALL sp_InserirContasAReceber(@ClienteId, @PedidoId, @Data, @Historico, @Valor, @Saldo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@ClienteId", contasAReceber.ClienteId);
                cmd.Parameters.AddWithValue("@PedidoId", contasAReceber.PedidoId);
                cmd.Parameters.AddWithValue("@Data", contasAReceber.Data);
                cmd.Parameters.AddWithValue("@Historico", contasAReceber.Historico);
                cmd.Parameters.AddWithValue("@Valor", contasAReceber.Valor);
                cmd.Parameters.AddWithValue("@Saldo", contasAReceber.Saldo);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(ContasAReceber contasAReceber)
        {
            string sql = "CALL sp_DeletarContasAReceber(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAReceber.Id);

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
        public static List<ContasAReceber> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosContasAReceber()";

            List<ContasAReceber> listaContasAReceber = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");
                        int clienteId = reader.GetInt32("ClienteId");
                        int pedidoId = reader.GetInt32("PedidoId");

                        DateTime data = (DateTime)reader["Data"];
                        string historico = ValidarCampo(reader["Historico"]);
                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        ContasAReceber contasAReceber = new ContasAReceber(
                            id, clienteId, pedidoId, data, historico, valor, saldo
                        );

                        listaContasAReceber.Add(contasAReceber);
                    }
                }

                return listaContasAReceber;
            }
        }

        // Listar por ID
        public static ContasAReceber? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdContasAReceber(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    ContasAReceber? contasAReceber = null;

                    while (reader.Read())
                    {
                        int idListado = reader.GetInt32("id");
                        int clienteId = reader.GetInt32("ClienteId");
                        int pedidoId = reader.GetInt32("PedidoId");

                        DateTime data = (DateTime)reader["Data"];
                        string historico = ValidarCampo(reader["Historico"]);
                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        contasAReceber = new ContasAReceber(
                            idListado, clienteId, pedidoId, data, historico, valor, saldo
                        );
                    }

                    return contasAReceber;
                }
            }
        }

        // Atualizar
        public static int Atualizar(ContasAReceber contasAReceber)
        {
            string sql = "CALL sp_AtualizarContaAReceber(@Id, @ClienteId, @PedidoId, @Data, @Historico, @Valor, @Saldo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", contasAReceber.Id);
                cmd.Parameters.AddWithValue("@ClienteId", contasAReceber.ClienteId);
                cmd.Parameters.AddWithValue("@PedidoId", contasAReceber.PedidoId);
                cmd.Parameters.AddWithValue("@Data", contasAReceber.Data);
                cmd.Parameters.AddWithValue("@Historico", contasAReceber.Historico);
                cmd.Parameters.AddWithValue("@Valor", contasAReceber.Valor);
                cmd.Parameters.AddWithValue("@Saldo", contasAReceber.Saldo);

                return Banco.ExecutarComando(cmd);
            }
        }

    }
}
