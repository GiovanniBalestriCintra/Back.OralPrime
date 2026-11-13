
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace OralPrime.Model.Producao
{
    internal class Pedido
    {
        // Variáveis
        private int _ficha;
        private int _clienteId;
        private int? _listaPrecoId;
        private int? _tipoPedidoId;
        private string _nome;
        private string _paciente;
        private DateTime _entrada;
        private DateTime? _prevista;
        private DateTime? _finalizado;
        private string _escala;
        private string _cor;
        private decimal _total;

        // Propriedades
        public int Ficha { get => _ficha; set => _ficha = value; }
        public int ClienteId { get => _clienteId; set => _clienteId = value; }
        public int? ListaPrecoId { get => _listaPrecoId; set => _listaPrecoId = value; }
        public int? TipoPedidoId { get => _tipoPedidoId; set => _tipoPedidoId = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Paciente { get => _paciente; set => _paciente = value; }
        public DateTime Entrada { get => _entrada; set => _entrada = value; }
        public DateTime? Prevista { get => _prevista; set => _prevista = value; }
        public DateTime? Finalizado { get => _finalizado; set => _finalizado = value; }
        public string Escala { get => _escala; set => _escala = value; }
        public string Cor { get => _cor; set => _cor = value; }
        public decimal Total { get => _total; set => _total = value; }


        // Construtor sem ID
        public Pedido(
            int clienteId,
            int? listaPrecoId,
            int? tipoPedidoId,
            string nome,
            string paciente,
            DateTime entrada,
            DateTime? prevista,
            DateTime? finalizado,
            string escala,
            string cor,
            decimal total)
        {
            _clienteId = clienteId;
            _listaPrecoId = listaPrecoId;
            _tipoPedidoId = tipoPedidoId;
            _nome = nome;
            _paciente = paciente;
            _entrada = entrada;
            _prevista = prevista;
            _finalizado = finalizado;
            _escala = escala;
            _cor = cor;
            _total = total;
        }


        // Construtor com ID
        public Pedido(
            int ficha,
            int clienteId,
            int? listaPrecoId,
            int? tipoPedidoId,
            string nome,
            string paciente,
            DateTime entrada,
            DateTime? prevista,
            DateTime? finalizado,
            string escala,
            string cor,
            decimal total)
        {
            _ficha = ficha;
            _clienteId = clienteId;
            _listaPrecoId = listaPrecoId;
            _tipoPedidoId = tipoPedidoId;
            _nome = nome;
            _paciente = paciente;
            _entrada = entrada;
            _prevista = prevista;
            _finalizado = finalizado;
            _escala = escala;
            _cor = cor;
            _total = total;
        }


        // Inserir
        public static int Inserir(Pedido pedido)
        {
            string sql = @"CALL sp_InserirPedido(
                @ClienteId, @ListaPrecoId, @TipoPedidoId,
                @Nome, @Paciente, @Entrada, @Prevista,
                @Finalizado, @Escala, @Cor, @Total)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);

                cmd.Parameters.AddWithValue("@ListaPrecoId",
                    (object?)pedido.ListaPrecoId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@TipoPedidoId",
                    (object?)pedido.TipoPedidoId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Nome", pedido.Nome);
                cmd.Parameters.AddWithValue("@Paciente", pedido.Paciente);
                cmd.Parameters.AddWithValue("@Entrada", pedido.Entrada);

                cmd.Parameters.AddWithValue("@Prevista",
                    (object?)pedido.Prevista ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Finalizado",
                    (object?)pedido.Finalizado ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Escala", pedido.Escala);
                cmd.Parameters.AddWithValue("@Cor", pedido.Cor);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);

                return Banco.ExecutarComando(cmd);
            }
        }


        // Deletar
        public static int Deletar(Pedido pedido)
        {
            string sql = "CALL sp_DeletarPedido(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", pedido.Ficha);

                return Banco.ExecutarComando(cmd);
            }
        }


        // Validar Campo
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


        // Listar todos
        public static List<Pedido> ListarTodos()
        {
            string sql = "CALL sp_listarTodosPedido()";

            List<Pedido> listaPedido = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader =
                    Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int fichaLista = reader.GetInt32("id");

                        int clienteId = reader.GetInt32("ClienteId");

                        int? listaPrecoId = reader["ListaPrecoId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["ListaPrecoId"]);

                        int? tipoPedidoId = reader["TipoPedidoId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["TipoPedidoId"]);

                        string nome = ValidarCampo(reader["Nome"]);
                        string paciente = ValidarCampo(reader["Paciente"]);

                        DateTime entrada = Convert.ToDateTime(reader["Entrada"]);

                        DateTime? prevista = reader["Prevista"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Prevista"]);

                        DateTime? finalizado = reader["Finalizado"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Finalizado"]);

                        string escala = ValidarCampo(reader["Escala"]);
                        string cor = ValidarCampo(reader["Cor"]);
                        decimal total = reader.GetDecimal("Total");

                        Pedido pedido = new Pedido(
                            fichaLista,
                            clienteId,
                            listaPrecoId,
                            tipoPedidoId,
                            nome,
                            paciente,
                            entrada,
                            prevista,
                            finalizado,
                            escala,
                            cor,
                            total
                        );

                        listaPedido.Add(pedido);
                    }
                }

                return listaPedido;
            }
        }


        // Listar por ID
        public static Pedido? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdPedido(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader =
                    Banco.ExecutarComRetornoDeDados(cmd))
                {
                    Pedido? pedido = null;

                    while (reader.Read())
                    {
                        int fichaLista = reader.GetInt32("id");

                        int clienteId = reader.GetInt32("ClienteId");

                        int? listaPrecoId = reader["ListaPrecoId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["ListaPrecoId"]);

                        int? tipoPedidoId = reader["TipoPedidoId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["TipoPedidoId"]);

                        string nome = ValidarCampo(reader["Nome"]);
                        string paciente = ValidarCampo(reader["Paciente"]);

                        DateTime entrada = Convert.ToDateTime(reader["Entrada"]);

                        DateTime? prevista = reader["Prevista"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Prevista"]);

                        DateTime? finalizado = reader["Finalizado"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Finalizado"]);

                        string escala = ValidarCampo(reader["Escala"]);
                        string cor = ValidarCampo(reader["Cor"]);
                        decimal total = reader.GetDecimal("Total");

                        pedido = new Pedido(
                            fichaLista,
                            clienteId,
                            listaPrecoId,
                            tipoPedidoId,
                            nome,
                            paciente,
                            entrada,
                            prevista,
                            finalizado,
                            escala,
                            cor,
                            total
                        );
                    }

                    return pedido;
                }
            }
        }


        // Atualizar
        public static int Atualizar(Pedido pedido)
        {
            string sql = @"CALL sp_AtualizarPedido(
                @Id, @ClienteId, @ListaPrecoId, @TipoPedidoId,
                @Nome, @Paciente, @Entrada, @Prevista,
                @Finalizado, @Escala, @Cor, @Total)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", pedido.Ficha);

                cmd.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);

                cmd.Parameters.AddWithValue("@ListaPrecoId",
                    (object?)pedido.ListaPrecoId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@TipoPedidoId",
                    (object?)pedido.TipoPedidoId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Nome", pedido.Nome);
                cmd.Parameters.AddWithValue("@Paciente", pedido.Paciente);
                cmd.Parameters.AddWithValue("@Entrada", pedido.Entrada);

                cmd.Parameters.AddWithValue("@Prevista",
                    (object?)pedido.Prevista ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Finalizado",
                    (object?)pedido.Finalizado ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Escala", pedido.Escala);
                cmd.Parameters.AddWithValue("@Cor", pedido.Cor);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);

                return Banco.ExecutarComando(cmd);
            }
        }
    }
}
