
using MySqlConnector;
using OralPrime.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class Boleto
    {
        // Variáveis
        private int _id;
        private DateTime _data;
        private int _clienteId;
        private string _numDocumento;
        private DateTime _dtVencimento;
        private decimal _valor;
        private DateTime _dtPagamento;
        private decimal _valorPago;
        private string _observacao;
        private bool _final;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public int ClienteId { get => _clienteId; set => _clienteId = value; }
        public string NumDocumento { get => _numDocumento; set => _numDocumento = value; }
        public DateTime DtVencimento { get => _dtVencimento; set => _dtVencimento = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public DateTime DtPagamento { get => _dtPagamento; set => _dtPagamento = value; }
        public decimal ValorPago { get => _valorPago; set => _valorPago = value; }
        public string Observacao { get => _observacao; set => _observacao = value; }
        public bool Final { get => _final; set => _final = value; }

        // Construtor com ID
        public Boleto(int id, DateTime data, int clienteId, string numDocumento,
            DateTime dtVencimento, decimal valor, DateTime dtPagamento,
            decimal valorPago, string observacao, bool final)
        {
            _id = id;
            _data = data;
            _clienteId = clienteId;
            _numDocumento = numDocumento;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _observacao = observacao;
            _final = final;
        }

        // Construtor sem ID
        public Boleto(DateTime data, int clienteId, string numDocumento,
            DateTime dtVencimento, decimal valor, DateTime dtPagamento,
            decimal valorPago, string observacao, bool final)
        {
            _data = data;
            _clienteId = clienteId;
            _numDocumento = numDocumento;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _observacao = observacao;
            _final = final;
        }

        // Inserir
        public static int Inserir(Boleto boleto)
        {
            string sql = "CALL sp_InserirBoleto(@Data, @ClienteId, @NumDocumento, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Observacao, @Final)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Data", boleto.Data);
                cmd.Parameters.AddWithValue("@ClienteId", boleto.ClienteId);
                cmd.Parameters.AddWithValue("@NumDocumento", boleto.NumDocumento);
                cmd.Parameters.AddWithValue("@DtVencimento", boleto.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", boleto.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", boleto.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", boleto.ValorPago);
                cmd.Parameters.AddWithValue("@Observacao", boleto.Observacao);
                cmd.Parameters.AddWithValue("@Final", boleto.Final);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(Boleto boleto)
        {
            string sql = "CALL sp_DeletarBoleto(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", boleto.Id);

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
        public static List<Boleto> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosBoletos()";

            List<Boleto> listaBoletos = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        int clienteId = reader.GetInt32("ClienteId");
                        string numDocumento = ValidarCampo(reader["NumDocumento"]);
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string observacao = ValidarCampo(reader["Observacao"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        Boleto boleto = new Boleto(
                            id, data, clienteId, numDocumento,
                            dtVencimento, valor, dtPagamento,
                            valorPago, observacao, final
                        );

                        listaBoletos.Add(boleto);
                    }
                }

                return listaBoletos;
            }
        }

        // Listar por ID
        public static Boleto? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdBoleto(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    Boleto boleto = null;

                    while (reader.Read())
                    {
                        int idLista = reader.GetInt32("id");

                        DateTime data = (DateTime)reader["Data"];
                        int clienteId = reader.GetInt32("ClienteId");
                        string numDocumento = ValidarCampo(reader["NumDocumento"]);
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string observacao = ValidarCampo(reader["Observacao"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        boleto = new Boleto(
                            idLista, data, clienteId, numDocumento,
                            dtVencimento, valor, dtPagamento,
                            valorPago, observacao, final
                        );
                    }

                    return boleto;
                }
            }
        }

        // Atualizar
        public static int Atualizar(Boleto boleto)
        {
            string sql = "CALL sp_AtualizarBoleto(@Id, @Data, @ClienteId, @NumDocumento, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Observacao, @Final)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", boleto.Id);
                cmd.Parameters.AddWithValue("@Data", boleto.Data);
                cmd.Parameters.AddWithValue("@ClienteId", boleto.ClienteId);
                cmd.Parameters.AddWithValue("@NumDocumento", boleto.NumDocumento);
                cmd.Parameters.AddWithValue("@DtVencimento", boleto.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", boleto.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", boleto.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", boleto.ValorPago);
                cmd.Parameters.AddWithValue("@Observacao", boleto.Observacao);
                cmd.Parameters.AddWithValue("@Final", boleto.Final);

                return Banco.ExecutarComando(cmd);
            }
        }

    }
}
