using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;


namespace OralPrime.Model.Producao
{
    internal class Funcionario
    {

        // Variáveis
        private int _id;
        private string _nome;
        private string _ddd;
        private string _telefone;
        private string _endereco;
        private string _bairro;
        private string _cidade;
        private string _estado;
        private string _cep;
        private string _cpf;

        public int Id { get => _id; set => _id = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Ddd { get => _ddd; set => _ddd = value; }
        public string Telefone { get => _telefone; set => _telefone = value; }
        public string Endereco { get => _endereco; set => _endereco = value; }
        public string Bairro { get => _bairro; set => _bairro = value; }
        public string Cidade { get => _cidade; set => _cidade = value; }
        public string Estado { get => _estado; set => _estado = value; }
        public string Cep { get => _cep; set => _cep = value; }
        public string Cpf { get => _cpf; set => _cpf = value; }


        // Método construtor sem ID
        public Funcionario (string nome, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep, string cpf)
        {
            this._nome = nome;
            this._ddd = ddd;
            this._telefone = telefone;
            this._endereco = endereco;
            this._bairro = bairro;
            this._cidade = cidade;
            this._estado = estado;
            this._cep = cep;
            this._cpf = cpf;
        }

        // Método construtor com ID

        public Funcionario(int id, string nome, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep, string cpf)
        {
            this._id = id;
            this._nome = nome;
            this._ddd = ddd;
            this._telefone = telefone;
            this._endereco = endereco;
            this._bairro = bairro;
            this._cidade = cidade;
            this._estado = estado;
            this._cep = cep;
            this._cpf = cpf;
        }




        // Inserir Funcionário
        public static int Inserir(Funcionario funcionario)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_Funcionario` (`nome`, `DDD`, `telefone`, `endereco`, `bairro`, `cidade`, `estado`, `CEP`, `CPF`) VALUES (@Nome, @DDD, @Telefone, @Endereco, @Bairro, @Cidade, @Estado, @CEP, @CPF)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@DDD", funcionario.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", funcionario.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", funcionario.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", funcionario.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", funcionario.Cidade);
                cmd.Parameters.AddWithValue("@Estado", funcionario.Estado);
                cmd.Parameters.AddWithValue("@CEP", funcionario.Cep);
                cmd.Parameters.AddWithValue("@CPF", funcionario.Cpf);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir




        // Deletar Funcionário
        public static int Deletar(Funcionario funcionario)
        {
            // String SQL
            string sql = "DELETE FROM `tb_Funcionario` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", funcionario.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar


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


        } // Fim ValidarCampo




        // Listar todos os funcionários
        public static List<Funcionario> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_Funcionario`";

            List<Funcionario> listaFuncionario = new();

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
                        int idLista = reader.GetInt32("id");

                        string nome = ValidarCampo(reader["Nome"]);
                        string ddd = ValidarCampo(reader["DDD"]);
                        string telefone = ValidarCampo(reader["Telefone"]);
                        string endereco = ValidarCampo(reader["Endereco"]);
                        string bairro = ValidarCampo(reader["Bairro"]);
                        string cidade = ValidarCampo(reader["Cidade"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        string cep = ValidarCampo(reader["Cep"]);
                        string cpf = ValidarCampo(reader["Cpf"]);


                        // Cria o objeto Cliente com os dados extraidos
                        Funcionario funcionario = new Funcionario(idLista, nome, ddd, telefone, endereco, bairro, cidade, estado, cep, cpf);

                        // Adiciona este cliente à nossa lista
                        listaFuncionario.Add(funcionario);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaFuncionario;

            }

        } //Fim ListarTodos



        public static Funcionario? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_Funcionario` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Funcionario funcionario = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idLista = reader.GetInt32("id");

                    string nome = ValidarCampo(reader["Nome"]);
                    string ddd = ValidarCampo(reader["DDD"]);
                    string telefone = ValidarCampo(reader["Telefone"]);
                    string endereco = ValidarCampo(reader["Endereco"]);
                    string bairro = ValidarCampo(reader["Bairro"]);
                    string cidade = ValidarCampo(reader["Cidade"]);
                    string estado = ValidarCampo(reader["Estado"]);
                    string cep = ValidarCampo(reader["Cep"]);
                    string cpf = ValidarCampo(reader["Cpf"]);

                    // Cria o objeto Cliente con os dados extraidos
                    funcionario = new Funcionario(idLista, nome, ddd, telefone, endereco, bairro, cidade, estado, cep, cpf);
                }

                return funcionario;
            }
        } // Fim ListarPorId



        // Atualizar funcionario
        public static int Atualizar(Funcionario funcionario)
        {
            string sql = "UPDATE `tb_Funcionario` SET `Nome` = @Nome, `DDD` = @DDD, `telefone` = @Telefone, `endereco` = @Endereco, `bairro` = @Bairro, `cidade` = @Cidade, `estado` = @Estado, `CEP` = @CEP, `CPF` = @CPF WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", funcionario.Nome);
                cmd.Parameters.AddWithValue("@DDD", funcionario.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", funcionario.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", funcionario.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", funcionario.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", funcionario.Cidade);
                cmd.Parameters.AddWithValue("@Estado", funcionario.Estado);
                cmd.Parameters.AddWithValue("@CEP", funcionario.Cep);
                cmd.Parameters.AddWithValue("@CPF", funcionario.Cpf);


                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", funcionario.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar

    }
}
