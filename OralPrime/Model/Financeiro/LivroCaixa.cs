using MySqlConnector;
using System;
using System.Collections.Generic;

namespace OralPrime.Model.Financeiro
{
    public enum TipoMovimento
    {
        Entrada,
        Saida
    }

    internal class LivroCaixa
    {
        // Variáveis
        private int _id;
        private DateTime _data;
        private string _historico;
        private TipoMovimento _entradaSaida;
        private decimal _valor;
        private decimal _saldo;

        // Propriedades
        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public TipoMovimento EntradaSaida { get => _entradaSaida; set => _entradaSaida = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public decimal Saldo { get => _saldo; set => _saldo = value; }

        // Construtor sem ID
        public LivroCaixa(DateTime data, string historico, TipoMovimento entradaSaida, decimal valor, decimal saldo)
        {
            _data = data;
            _historico = historico;
            _entradaSaida = entradaSaida;
            _valor = valor;
            _saldo = saldo;
        }

        // Construtor com ID
        public LivroCaixa(int id, DateTime data, string historico, TipoMovimento entradaSaida, decimal valor, decimal saldo)
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
            string sql = @"CALL sp_InserirLivroCaixa(@Data, @Historico, @EntradaSaida, @Valor, @Saldo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Data", livroCaixa.Data);
                cmd.Parameters.AddWithValue("@Historico", livroCaixa.Historico);
                cmd.Parameters.AddWithValue("@EntradaSaida", livroCaixa.EntradaSaida.ToString());
                cmd.Parameters.AddWithValue("@Valor", livroCaixa.Valor);
                cmd.Parameters.AddWithValue("@Saldo", livroCaixa.Saldo);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Atualizar
        public static int Atualizar(LivroCaixa livroCaixa)
        {
            string sql = @"CALL sp_AtualizarLivroCaixa(@Id, @Data, @Historico, @EntradaSaida, @Valor, @Saldo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", livroCaixa.Id);
                cmd.Parameters.AddWithValue("@Data", livroCaixa.Data);
                cmd.Parameters.AddWithValue("@Historico", livroCaixa.Historico);
                cmd.Parameters.AddWithValue("@EntradaSaida", livroCaixa.EntradaSaida.ToString());
                cmd.Parameters.AddWithValue("@Valor", livroCaixa.Valor);
                cmd.Parameters.AddWithValue("@Saldo", livroCaixa.Saldo);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(LivroCaixa livroCaixa)
        {
            string sql = "CALL sp_DeletarLivroCaixa(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", livroCaixa.Id);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Validação String
        private static string ValidarCampoString(object campo)
        {
            if (campo == DBNull.Value || campo == null)
                return "";

            return campo.ToString();
        }

        // Conversão para enum
        private static TipoMovimento ValidarTipoMovimento(object campo)
        {
            if (campo == DBNull.Value || campo == null)
                return TipoMovimento.Entrada;

            return Enum.Parse<TipoMovimento>(campo.ToString());
        }

        private static bool CampoPreenchido(object campo)
        {
            return campo != DBNull.Value && campo != null;
        }

        // Listar Todos
        public static List<LivroCaixa> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosLivroCaixa()";

            List<LivroCaixa> listaLivroCaixa = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        DateTime data = reader.GetDateTime("Data");
                        string historico = ValidarCampoString(reader["Historico"]);

                        TipoMovimento entradaSaida =
                            ValidarTipoMovimento(reader["EntradaSaida"]);

                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        LivroCaixa livroCaixa = new LivroCaixa(id, data, historico, entradaSaida, valor, saldo);

                        listaLivroCaixa.Add(livroCaixa);
                    }
                }
            }

            return listaLivroCaixa;
        }

        // Listar por ID
        public static LivroCaixa? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdLivroCaixa(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    LivroCaixa? livroCaixa = null;

                    while (reader.Read())
                    {
                        int idLista = reader.GetInt32("Id");
                        DateTime data = reader.GetDateTime("Data");
                        string historico = ValidarCampoString(reader["Historico"]);

                        TipoMovimento entradaSaida =
                            ValidarTipoMovimento(reader["EntradaSaida"]);

                        decimal valor = reader.GetDecimal("Valor");
                        decimal saldo = reader.GetDecimal("Saldo");

                        livroCaixa = new LivroCaixa(idLista, data, historico, entradaSaida, valor, saldo);
                    }

                    return livroCaixa;
                }
            }
        }
    }
}