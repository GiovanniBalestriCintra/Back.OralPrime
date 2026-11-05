using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OralPrime.Model.Producao
{
    internal class Cliente
    {

        // Variáveis
        private int _id;
        private string _nome;
        private string _contato;
        private string _ddd;
        private string _telefone;
        private string _endereco;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _cep;
        private string _email;

       

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Contato { get => _contato; set => _contato = value; }
        public string Ddd { get => _ddd; set => _ddd = value; }
        public string Telefone { get => _telefone; set => _telefone = value; }
        public string Endereco { get => _endereco; set => _endereco = value; }
        public string Bairro { get => _bairro; set => _bairro = value; }
        public string Cidade { get => _cidade; set => _cidade = value; }
        public string Estado { get => _estado; set => _estado = value; }
        public string Cep { get => _cep; set => _cep = value; }
        public string Email { get => __email; set => __email = value; }


        // Método construtor sem ID.
        public Cliente(string nome, string contato, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep, string email)
        {
            _nome = nome;
            _contato = contato;
            _ddd = ddd;
            _telefone = telefone;
            _endereco = endereco;
            _bairro = bairro;
            _cidade = cidade;
            _estado = estado;
            _cep = cep;
            __email = email;
        }

        // Método construtor com ID.
        public Cliente(int id, string nome, string contato, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep, string email)
        {
            _id = id;
            _nome = nome;
            _contato = contato;
            _ddd = ddd;
            _telefone = telefone;
            _endereco = endereco;
            _bairro = bairro;
            _cidade = cidade;
            _estado = estado;
            _cep = cep;
            __email = email;
        }


        // Inserir Cliente
        public static int Inserir(Cliente cliente)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_Cliente` (`nome`, `contato`, `DDD`, `telefone`, `endereco`, `bairro`, `cidade`, `estado`, `CEP`, `email`) VALUES (@Nome, @Contato, @DDD, @Telefone, @Endereco, @Bairro, @Cidade, @Estado, @CEP, @Email)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Contato", cliente.Contato);
                cmd.Parameters.AddWithValue("@DDD", cliente.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", cliente.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                cmd.Parameters.AddWithValue("@CEP", cliente.Cep);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir




        // Deletar Cliente
        public static int Deletar(Cliente cliente)
        {
            // String SQL
            string sql = "DELETE FROM `tb_Cliente` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", cliente.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar



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




        public static List<Cliente> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_Cliente`";

            List<Cliente> listaClientes = new();

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

                        string nome = ValidarCampo(reader["Nome"]);
                        string contato = ValidarCampo(reader["Contato"]);
                        string ddd = ValidarCampo(reader["DDD"]);
                        string telefone = ValidarCampo(reader["Telefone"]);
                        string endereco = ValidarCampo(reader["Endereco"]);
                        string bairro = ValidarCampo(reader["Bairro"]);
                        string cidade = ValidarCampo(reader["Cidade"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        string cep = ValidarCampo(reader["Cep"]);
                        string email = ValidarCampo(reader["Email"]);


                        // Cria o objeto Cliente con os dados extraidos
                        Cliente cliente = new Cliente(id, nome, contato, ddd, telefone, endereco, bairro, cidade, estado, cep, email);

                        // Adiciona este cliente à nossa lista
                        listaClientes.Add(cliente);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaClientes;

            }

        } //Fim ListarTodos



        public static Cliente? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_Cliente` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Cliente cliente = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    string nome = ValidarCampo(reader["Nome"]);
                    string contato = ValidarCampo(reader["Contato"]);
                    string ddd = ValidarCampo(reader["DDD"]);
                    string telefone = ValidarCampo(reader["Telefone"]);
                    string endereco = ValidarCampo(reader["Endereco"]);
                    string bairro = ValidarCampo(reader["Bairro"]);
                    string cidade = ValidarCampo(reader["Cidade"]);
                    string estado = ValidarCampo(reader["Estado"]);
                    string cep = ValidarCampo(reader["Cep"]);
                    string email = ValidarCampo(reader["Email"]);

                    // Cria o objeto Cliente con os dados extraidos
                    cliente = new Cliente(idListado, nome, contato, ddd, telefone, endereco, bairro, cidade, estado, cep, email);
                }

                return cliente;
            }
        } // Fim ListarPorId



        public static int Atualizar(Cliente cliente)
        {
            string sql = "UPDATE `tb_Cliente` SET `Nome` = @Nome, `contato` = @Contato, `DDD` = @DDD, `telefone` = @Telefone, `endereco` = @Endereco, `bairro` = @Bairro, `cidade` = @Cidade, `estado` = @Estado, `CEP` = @CEP, `email` = @Email WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                cmd.Parameters.AddWithValue("@Contato", cliente.Contato);
                cmd.Parameters.AddWithValue("@DDD", cliente.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", cliente.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", cliente.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", cliente.Cidade);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                cmd.Parameters.AddWithValue("@CEP", cliente.Cep);
                cmd.Parameters.AddWithValue("@Email", cliente.Email);

                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", cliente.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    }// Classe

}// NameSpace
