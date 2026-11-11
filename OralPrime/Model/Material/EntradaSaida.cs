using MySqlConnector;
using System;
using System.Collections.Generic;

namespace OralPrime.Model.Material
{
    public enum TipoMovimento
    {
        Entrada,
        Saida
    }

    internal class EntradaSaida
    {
        // Variáveis
        private int _id;
        private DateTime _data;
        private string _descricao;
        private int _unidade;
        private int _quantidade;
        private decimal _precoUnitario;
        private TipoMovimento _entradaSaida;

        // Propriedades
        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Descricao { get => _descricao; set => _descricao = value; }
        public int Unidade { get => _unidade; set => _unidade = value; }
        public int Quantidade { get => _quantidade; set => _quantidade = value; }
        public decimal PrecoUnitario { get => _precoUnitario; set => _precoUnitario = value; }
        public TipoMovimento EntradaSaidaTipo { get => _entradaSaida; set => _entradaSaida = value; }

        // Construtor sem ID
        public EntradaSaida(DateTime data, string descricao, int unidade, int quantidade, decimal precoUnitario, TipoMovimento entradaSaida)
        {
            _data = data;
            _descricao = descricao;
            _unidade = unidade;
            _quantidade = quantidade;
            _precoUnitario = precoUnitario;
            _entradaSaida = entradaSaida;
        }

        // Construtor com ID
        public EntradaSaida(int id, DateTime data, string descricao, int unidade, int quantidade, decimal precoUnitario, TipoMovimento entradaSaida)
        {
            _id = id;
            _data = data;
            _descricao = descricao;
            _unidade = unidade;
            _quantidade = quantidade;
            _precoUnitario = precoUnitario;
            _entradaSaida = entradaSaida;
        }

        // Inserir
        public static int Inserir(EntradaSaida entradaSaida)
        {
            string sql = @"CALL sp_InserirEntradaSaida( @Data, @Descricao, @Unidade, @Quantidade, @PrecoUnitario, @EntradaSaida)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Data", entradaSaida.Data);
                cmd.Parameters.AddWithValue("@Descricao", entradaSaida.Descricao);
                cmd.Parameters.AddWithValue("@Unidade", entradaSaida.Unidade);
                cmd.Parameters.AddWithValue("@Quantidade", entradaSaida.Quantidade);
                cmd.Parameters.AddWithValue("@PrecoUnitario", entradaSaida.PrecoUnitario);
                cmd.Parameters.AddWithValue("@EntradaSaida", entradaSaida.EntradaSaidaTipo.ToString());

                return Banco.ExecutarComando(cmd);
            }
        }

        // Atualizar
        public static int Atualizar(EntradaSaida entradaSaida)
        {
            string sql = @"CALL sp_AtualizarEntradaSaida( @Id, @Data, @Descricao, @Unidade, @Quantidade, @PrecoUnitario, @EntradaSaida)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", entradaSaida.Id);
                cmd.Parameters.AddWithValue("@Data", entradaSaida.Data);
                cmd.Parameters.AddWithValue("@Descricao", entradaSaida.Descricao);
                cmd.Parameters.AddWithValue("@Unidade", entradaSaida.Unidade);
                cmd.Parameters.AddWithValue("@Quantidade", entradaSaida.Quantidade);
                cmd.Parameters.AddWithValue("@PrecoUnitario", entradaSaida.PrecoUnitario);
                cmd.Parameters.AddWithValue("@EntradaSaida", entradaSaida.EntradaSaidaTipo.ToString());

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(EntradaSaida entradaSaida)
        {
            string sql = "CALL sp_DeletarEntradaSaida(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", entradaSaida.Id);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Validar String
        private static string ValidarCampoString(object campo)
        {
            if (campo == DBNull.Value || campo == null)
                return "";

            return campo.ToString();
        }

        // Validar Enum
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
        public static List<EntradaSaida> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosEntradaSaida()";

            List<EntradaSaida> listaEntradaSaida = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("Id");
                        DateTime data = reader.GetDateTime("Data");
                        string descricao = ValidarCampoString(reader["Descricao"]);
                        int unidade = reader.GetInt32("Unidade");
                        int quantidade = reader.GetInt32("Quantidade");
                        decimal precoUnitario = reader.GetDecimal("PrecoUnitario");

                        TipoMovimento entradaSaida =
                            ValidarTipoMovimento(reader["EntradaSaida"]);

                        EntradaSaida movimento = new EntradaSaida(id, data, descricao, unidade, quantidade, precoUnitario, entradaSaida);

                        listaEntradaSaida.Add(movimento);
                    }
                }
            }

            return listaEntradaSaida;
        }

        // Listar por ID
        public static EntradaSaida? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdEntradaSaida(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    EntradaSaida? entradaSaida = null;

                    while (reader.Read())
                    {
                        int idLista = reader.GetInt32("Id");
                        DateTime data = reader.GetDateTime("Data");
                        string descricao = ValidarCampoString(reader["Descricao"]);
                        int unidade = reader.GetInt32("Unidade");
                        int quantidade = reader.GetInt32("Quantidade");
                        decimal precoUnitario = reader.GetDecimal("PrecoUnitario");

                        TipoMovimento tipoMovimento =
                            ValidarTipoMovimento(reader["EntradaSaida"]);

                        entradaSaida = new EntradaSaida( idLista, data, descricao, unidade, quantidade, precoUnitario, tipoMovimento);
                    }

                    return entradaSaida;
                }
            }
        }
    }
}