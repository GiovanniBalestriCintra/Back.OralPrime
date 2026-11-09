using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class RoteiroBoy
    {

        public enum TipoRoteitro
        {
            Entrada,
            Saida
        }

        // Váriaveis
        private int _id;
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

        public int Id { get => _id; set => _id = value; }
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


        public RoteiroBoy(int id, DateTime data, string nome, DateTime dtVisita, TimeSpan hora, string historico, DateTime? finalizado, string bairro, string cidade, string estado, string cep)
        {
            _id = id;
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

        public RoteiroBoy(DateTime data, string nome, DateTime dtVisita, TimeSpan hora, string historico, DateTime? finalizado, string bairro, string cidade, string estado, string cep)
        {
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



        // Inserir RoteiroBoy
        public static int Inserir(RoteiroBoy roteiroBoy)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_RoteiroBoy` (`data`, `nome`, `dtVisita`, `hora`, `historico`, `finalizado`, `bairro`, `cidade`, `estado`, `cep`) VALUES (@Data, @Nome, @DtVisita, @Hora, @Historico, @Finalizado, @Bairro, @Cidade, @Estado, @CEP)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", roteiroBoy.Data);
                cmd.Parameters.AddWithValue("@Nome", roteiroBoy.Nome);
                cmd.Parameters.AddWithValue("@DtVisita", roteiroBoy.DtVisita);
                cmd.Parameters.AddWithValue("@Hora", roteiroBoy.Hora);
                cmd.Parameters.AddWithValue("@Historico", roteiroBoy.Historico);
                cmd.Parameters.AddWithValue("@Finalizado", roteiroBoy.Finalizado);
                cmd.Parameters.AddWithValue("@Bairro", roteiroBoy.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", roteiroBoy.Cidade);
                cmd.Parameters.AddWithValue("@Estado", roteiroBoy.Estado);
                cmd.Parameters.AddWithValue("@CEP", roteiroBoy.Cep);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir




        // Deletar RoteioBoy
        public static int Deletar(RoteiroBoy roteiroBoy)
        {
            // String SQL
            string sql = "DELETE FROM `tb_RoteiroBoy` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", roteiroBoy.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar




        // ValidarCampo: Verifica se o campo e nulo ou DBNull e retorna uma string vazia, caso contrario retorna o valor do campo convertido para string.
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


        } // Fim ValidarCampo


        // Listar todos os RoteirosBoy
        public static List<RoteiroBoy> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_RoteiroBoy`";

            List<RoteiroBoy> listaRoteiros = new();

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
                        string nome = ValidarCampo(reader["Nome"]);
                        DateTime dtVisita = Convert.ToDateTime(reader["DtVisita"]);
                        TimeSpan hora = (TimeSpan)reader["Hora"];
                        string historico = ValidarCampo(reader["Historico"]);
                        DateTime finalizado = Convert.ToDateTime(reader["Finalizado"]);
                        string bairro = ValidarCampo(reader["Bairro"]);
                        string cidade = ValidarCampo(reader["Cidade"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        string cep = ValidarCampo(reader["Cep"]);


                        // Cria o objeto Cliente con os dados extraidos
                        RoteiroBoy roteiroBoy = new RoteiroBoy(id, data, nome, dtVisita, hora, historico, finalizado, bairro, cidade, estado, cep);

                        // Adiciona este cliente à nossa lista
                        listaRoteiros.Add(roteiroBoy);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaRoteiros;

            }

        } //Fim ListarTodos




        // Listar RoteiroBoy por ID
        public static RoteiroBoy? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_RoteiroBoy` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                RoteiroBoy roteiroBoy = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    string nome = ValidarCampo(reader["Nome"]);
                    DateTime dtVisita = Convert.ToDateTime(reader["DtVisita"]);
                    TimeSpan hora = (TimeSpan)reader["Hora"];
                    string historico = ValidarCampo(reader["Historico"]);
                    DateTime finalizado = Convert.ToDateTime(reader["Finalizado"]);
                    string bairro = ValidarCampo(reader["Bairro"]);
                    string cidade = ValidarCampo(reader["Cidade"]);
                    string estado = ValidarCampo(reader["Estado"]);
                    string cep = ValidarCampo(reader["Cep"]);
                   

                    // Cria o objeto RoteiroBoy com os dados extraídos
                    roteiroBoy = new RoteiroBoy(idListado, data, nome, dtVisita, hora, historico, finalizado, bairro, cidade, estado, cep);
                }

                return roteiroBoy;
            }
        } // Fim ListarPorId



        // Atualizar RoteiroBoy
        public static int Atualizar(RoteiroBoy roteiroBoy)
        {
            string sql = "UPDATE `tb_RoteiroBoy` SET `Data` = @Data, `Nome` = @Nome, `DtVisita` = @DtVisita, `Hora` = @Hora, `Historico` = @Historico, `Finalizado` = @Finalizado, `Bairro` = @Bairro, `Cidade` = @Cidade, `Estado` = @Estado, `Cep` = @Cep WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", roteiroBoy.Data);
                cmd.Parameters.AddWithValue("@Nome", roteiroBoy.Nome);
                cmd.Parameters.AddWithValue("@DtVisita", roteiroBoy.DtVisita);
                cmd.Parameters.AddWithValue("@Hora", roteiroBoy.Hora);
                cmd.Parameters.AddWithValue("@Historico", roteiroBoy.Historico);
                cmd.Parameters.AddWithValue("@Finalizado", roteiroBoy.Finalizado);
                cmd.Parameters.AddWithValue("@Bairro", roteiroBoy.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", roteiroBoy.Cidade);
                cmd.Parameters.AddWithValue("@Estado", roteiroBoy.Estado);
                cmd.Parameters.AddWithValue("@Cep", roteiroBoy.Cep);
                

                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", roteiroBoy.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar





    } // Classe

} // Namespace
