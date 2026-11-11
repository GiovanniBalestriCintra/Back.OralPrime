using MySqlConnector;
using System;
using System.Collections.Generic;

namespace OralPrime.Model.Material
{
    internal class Estoque
    {
        // Variáveis
        private int _codigo;
        private string _descricao;
        private int _unidade;
        private int _quantidade;
        private int _minimo;
        private int _saldo;
        private decimal _precoUnitario;
        private decimal _precoVenda;

        // Propriedades
        public int Codigo { get => _codigo; set => _codigo = value; }
        public string Descricao { get => _descricao; set => _descricao = value; }
        public int Unidade { get => _unidade; set => _unidade = value; }
        public int Quantidade { get => _quantidade; set => _quantidade = value; }
        public int Minimo { get => _minimo; set => _minimo = value; }
        public int Saldo { get => _saldo; set => _saldo = value; }
        public decimal PrecoUnitario { get => _precoUnitario; set => _precoUnitario = value; }
        public decimal PrecoVenda { get => _precoVenda; set => _precoVenda = value; }


        // Construtor sem código
        public Estoque(
            string descricao,
            int unidade,
            int quantidade,
            int minimo,
            int saldo,
            decimal precoUnitario,
            decimal precoVenda)
        {
            _descricao = descricao;
            _unidade = unidade;
            _quantidade = quantidade;
            _minimo = minimo;
            _saldo = saldo;
            _precoUnitario = precoUnitario;
            _precoVenda = precoVenda;
        }

        // Construtor com código
        public Estoque(
            int codigo,
            string descricao,
            int unidade,
            int quantidade,
            int minimo,
            int saldo,
            decimal precoUnitario,
            decimal precoVenda)
        {
            _codigo = codigo;
            _descricao = descricao;
            _unidade = unidade;
            _quantidade = quantidade;
            _minimo = minimo;
            _saldo = saldo;
            _precoUnitario = precoUnitario;
            _precoVenda = precoVenda;
        }

        // Inserir
        public static int Inserir(Estoque estoque)
        {
            string sql = @"CALL sp_InserirEstoque(@Descricao, @Unidade, @Quantidade, @Minimo, @Saldo, @PrecoUnitario, @PrecoVenda)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Descricao", estoque.Descricao);
                cmd.Parameters.AddWithValue("@Unidade", estoque.Unidade);
                cmd.Parameters.AddWithValue("@Quantidade", estoque.Quantidade);
                cmd.Parameters.AddWithValue("@Minimo", estoque.Minimo);
                cmd.Parameters.AddWithValue("@Saldo", estoque.Saldo);
                cmd.Parameters.AddWithValue("@PrecoUnitario", estoque.PrecoUnitario);
                cmd.Parameters.AddWithValue("@PrecoVenda", estoque.PrecoVenda);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Atualizar
        public static int Atualizar(Estoque estoque)
        {
            string sql = @"CALL sp_AtualizarEstoque(@Codigo, @Descricao, @Unidade, @Quantidade, @Minimo, @Saldo, @PrecoUnitario, @PrecoVenda)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Codigo", estoque.Codigo);
                cmd.Parameters.AddWithValue("@Descricao", estoque.Descricao);
                cmd.Parameters.AddWithValue("@Unidade", estoque.Unidade);
                cmd.Parameters.AddWithValue("@Quantidade", estoque.Quantidade);
                cmd.Parameters.AddWithValue("@Minimo", estoque.Minimo);
                cmd.Parameters.AddWithValue("@Saldo", estoque.Saldo);
                cmd.Parameters.AddWithValue("@PrecoUnitario", estoque.PrecoUnitario);
                cmd.Parameters.AddWithValue("@PrecoVenda", estoque.PrecoVenda);

                return Banco.ExecutarComando(cmd);
            }
        }

        // Deletar
        public static int Deletar(Estoque estoque)
        {
            string sql = "CALL sp_DeletarEstoque(@Codigo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Codigo", estoque.Codigo);

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

        private static bool CampoPreenchido(object campo)
        {
            return campo != DBNull.Value && campo != null;
        }

        // Listar Todos
        public static List<Estoque> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosEstoque()";

            List<Estoque> listaEstoque = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int codigo = reader.GetInt32("Codigo");
                        string descricao = ValidarCampoString(reader["Descricao"]);
                        int unidade = reader.GetInt32("Unidade");
                        int quantidade = reader.GetInt32("Quantidade");
                        int minimo = reader.GetInt32("Minimo");
                        int saldo = reader.GetInt32("Saldo");
                        decimal precoUnitario = reader.GetDecimal("PrecoUnitario");
                        decimal precoVenda = reader.GetDecimal("PrecoVenda");

                        Estoque estoque = new Estoque(codigo, descricao, unidade, quantidade, minimo, saldo, precoUnitario, precoVenda);

                        listaEstoque.Add(estoque);
                    }
                }
            }

            return listaEstoque;
        }

        // Listar por Código
        public static Estoque? ListarPorCodigo(int codigo)
        {
            string sql = "CALL sp_ListarPorCodigoEstoque(@Codigo)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Codigo", codigo);

                using (MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd))
                {
                    Estoque? estoque = null;

                    while (reader.Read())
                    {
                        int codigoLista = reader.GetInt32("Codigo");
                        string descricao = ValidarCampoString(reader["Descricao"]);
                        int unidade = reader.GetInt32("Unidade");
                        int quantidade = reader.GetInt32("Quantidade");
                        int minimo = reader.GetInt32("Minimo");
                        int saldo = reader.GetInt32("Saldo");
                        decimal precoUnitario = reader.GetDecimal("PrecoUnitario");
                        decimal precoVenda = reader.GetDecimal("PrecoVenda");

                        estoque = new Estoque(codigoLista, descricao, unidade, quantidade, minimo, saldo, precoUnitario, precoVenda);
                    }

                    return estoque;
                }
            }
        }
    }
}