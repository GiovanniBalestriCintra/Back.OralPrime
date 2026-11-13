
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace OralPrime.Model.Producao
{
    internal class RoteiroBoy
    {
        public enum TipoRoteitro
        {
            Entrada,
            Saida
        }

        // Variáveis
        private int _id;
        private int? _funcionarioId;
        private int _clienteId;
        private DateTime _data;
        private string _nome;
        private DateTime _DtVisita;
        private TimeSpan _hora;
        private string _historico;
        private DateTime? _finalizado;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _cep;

        // Propriedades
        public int Id { get => _id; set => _id = value; }
        public int? FuncionarioId { get => _funcionarioId; set => _funcionarioId = value; }
        public int ClienteId { get => _clienteId; set => _clienteId = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public DateTime DtVisita { get => _DtVisita; set => _DtVisita = value; }
        public TimeSpan Hora { get => _hora; set => _hora = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public DateTime? Finalizado { get => _finalizado; set => _finalizado = value; }
        public string Bairro { get => _bairro; set => _bairro = value; }
        public string Cidade { get => _cidade; set => _cidade = value; }
        public string Estado { get => _estado; set => _estado = value; }
        public string Cep { get => _cep; set => _cep = value; }


        // Construtor sem ID
        public RoteiroBoy(
            int? funcionarioId,
            int clienteId,
            DateTime data,
            string nome,
            DateTime dtVisita,
            TimeSpan hora,
            string historico,
            DateTime? finalizado,
            string bairro,
            string cidade,
            string estado,
            string cep)
        {
            _funcionarioId = funcionarioId;
            _clienteId = clienteId;
            _data = data;
            _nome = nome;
            _DtVisita = dtVisita;
            _hora = hora;
            _historico = historico;
            _finalizado = finalizado;
            _bairro = bairro;
            _cidade = cidade;
            _estado = estado;
            _cep = cep;
        }


        // Construtor com ID
        public RoteiroBoy(
            int id,
            int? funcionarioId,
            int clienteId,
            DateTime data,
            string nome,
            DateTime dtVisita,
            TimeSpan hora,
            string historico,
            DateTime? finalizado,
            string bairro,
            string cidade,
            string estado,
            string cep)
        {
            _id = id;
            _funcionarioId = funcionarioId;
            _clienteId = clienteId;
            _data = data;
            _nome = nome;
            _DtVisita = dtVisita;
            _hora = hora;
            _historico = historico;
            _finalizado = finalizado;
            _bairro = bairro;
            _cidade = cidade;
            _estado = estado;
            _cep = cep;
        }


        // Inserir
        public static int Inserir(RoteiroBoy roteiroBoy)
        {
            string sql = @"CALL sp_InserirRoteiroBoy(
                @FuncionarioId, @ClienteId,
                @Data, @Nome, @DtVisita, @Hora, @Historico,
                @Finalizado, @Bairro, @Cidade, @Estado, @CEP)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@FuncionarioId",
                    (object?)roteiroBoy.FuncionarioId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ClienteId", roteiroBoy.ClienteId);
                cmd.Parameters.AddWithValue("@Data", roteiroBoy.Data);
                cmd.Parameters.AddWithValue("@Nome", roteiroBoy.Nome);
                cmd.Parameters.AddWithValue("@DtVisita", roteiroBoy.DtVisita);
                cmd.Parameters.AddWithValue("@Hora", roteiroBoy.Hora);
                cmd.Parameters.AddWithValue("@Historico", roteiroBoy.Historico);

                cmd.Parameters.AddWithValue("@Finalizado",
                    (object?)roteiroBoy.Finalizado ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Bairro", roteiroBoy.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", roteiroBoy.Cidade);
                cmd.Parameters.AddWithValue("@Estado", roteiroBoy.Estado);
                cmd.Parameters.AddWithValue("@CEP", roteiroBoy.Cep);

                return Banco.ExecutarComando(cmd);
            }
        }


        // Deletar
        public static int Deletar(RoteiroBoy roteiroBoy)
        {
            string sql = "CALL sp_DeletarRoteiroBoy(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", roteiroBoy.Id);

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

            return campo.ToString();
        }


        // Listar todos
        public static List<RoteiroBoy> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosRoteiroBoy()";

            List<RoteiroBoy> listaRoteiros = new();

            using (var cmd = new MySqlCommand(sql))
            {
                using (MySqlDataReader reader =
                    Banco.ExecutarComRetornoDeDados(cmd))
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32("id");

                        int? funcionarioId = reader["FuncionarioId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["FuncionarioId"]);

                        int clienteId = reader.GetInt32("ClienteId");

                        DateTime data = Convert.ToDateTime(reader["Data"]);
                        string nome = ValidarCampo(reader["Nome"]);
                        DateTime dtVisita = Convert.ToDateTime(reader["DtVisita"]);
                        TimeSpan hora = (TimeSpan)reader["Hora"];
                        string historico = ValidarCampo(reader["Historico"]);

                        DateTime? finalizado = reader["Finalizado"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Finalizado"]);

                        string bairro = ValidarCampo(reader["Bairro"]);
                        string cidade = ValidarCampo(reader["Cidade"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        string cep = ValidarCampo(reader["Cep"]);

                        RoteiroBoy roteiroBoy = new RoteiroBoy(
                            id,
                            funcionarioId,
                            clienteId,
                            data,
                            nome,
                            dtVisita,
                            hora,
                            historico,
                            finalizado,
                            bairro,
                            cidade,
                            estado,
                            cep
                        );

                        listaRoteiros.Add(roteiroBoy);
                    }
                }

                return listaRoteiros;
            }
        }


        // Listar por ID
        public static RoteiroBoy? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdRoteiroBoy(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                using (MySqlDataReader reader =
                    Banco.ExecutarComRetornoDeDados(cmd))
                {
                    RoteiroBoy? roteiroBoy = null;

                    while (reader.Read())
                    {
                        int idListado = reader.GetInt32("id");

                        int? funcionarioId = reader["FuncionarioId"] == DBNull.Value
                            ? null : Convert.ToInt32(reader["FuncionarioId"]);

                        int clienteId = reader.GetInt32("ClienteId");

                        DateTime data = Convert.ToDateTime(reader["Data"]);
                        string nome = ValidarCampo(reader["Nome"]);
                        DateTime dtVisita = Convert.ToDateTime(reader["DtVisita"]);
                        TimeSpan hora = (TimeSpan)reader["Hora"];
                        string historico = ValidarCampo(reader["Historico"]);

                        DateTime? finalizado = reader["Finalizado"] == DBNull.Value
                            ? null : Convert.ToDateTime(reader["Finalizado"]);

                        string bairro = ValidarCampo(reader["Bairro"]);
                        string cidade = ValidarCampo(reader["Cidade"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        string cep = ValidarCampo(reader["Cep"]);

                        roteiroBoy = new RoteiroBoy(
                            idListado,
                            funcionarioId,
                            clienteId,
                            data,
                            nome,
                            dtVisita,
                            hora,
                            historico,
                            finalizado,
                            bairro,
                            cidade,
                            estado,
                            cep
                        );
                    }

                    return roteiroBoy;
                }
            }
        }


        // Atualizar
        public static int Atualizar(RoteiroBoy roteiroBoy)
        {
            string sql = @"CALL sp_AtualizarRoteiroBoy(
                @Id, @FuncionarioId, @ClienteId,
                @Data, @Nome, @DtVisita, @Hora, @Historico,
                @Finalizado, @Bairro, @Cidade, @Estado, @CEP)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", roteiroBoy.Id);

                cmd.Parameters.AddWithValue("@FuncionarioId",
                    (object?)roteiroBoy.FuncionarioId ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ClienteId", roteiroBoy.ClienteId);
                cmd.Parameters.AddWithValue("@Data", roteiroBoy.Data);
                cmd.Parameters.AddWithValue("@Nome", roteiroBoy.Nome);
                cmd.Parameters.AddWithValue("@DtVisita", roteiroBoy.DtVisita);
                cmd.Parameters.AddWithValue("@Hora", roteiroBoy.Hora);
                cmd.Parameters.AddWithValue("@Historico", roteiroBoy.Historico);

                cmd.Parameters.AddWithValue("@Finalizado",
                    (object?)roteiroBoy.Finalizado ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Bairro", roteiroBoy.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", roteiroBoy.Cidade);
                cmd.Parameters.AddWithValue("@Estado", roteiroBoy.Estado);
                cmd.Parameters.AddWithValue("@CEP", roteiroBoy.Cep);

                return Banco.ExecutarComando(cmd);
            }
        }
    }
}
