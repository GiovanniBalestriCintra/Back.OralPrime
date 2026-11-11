using MySqlConnector;
using OralPrime.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class Fornecedor
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


        // Método construtor sem ID
        public Fornecedor(string nome, string contato, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep)
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
        }

        // Método construtor com ID
        public Fornecedor(int id, string nome, string contato, string ddd, string telefone, string endereco, string bairro, string cidade, string estado, string cep)
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
        }




        // Inserir
        public static int Inserir(Fornecedor fornecedor)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirFornecedor(@Nome, @Contato, @DDD, @Telefone, @Endereco, @Bairro, @Cidade, @Estado, @CEP)";
            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", fornecedor.Nome);
                cmd.Parameters.AddWithValue("@Contato", fornecedor.Contato);
                cmd.Parameters.AddWithValue("@DDD", fornecedor.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", fornecedor.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", fornecedor.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", fornecedor.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", fornecedor.Cidade);
                cmd.Parameters.AddWithValue("@Estado", fornecedor.Estado);
                cmd.Parameters.AddWithValue("@CEP", fornecedor.Cep);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar
        public static int Deletar(Fornecedor fornecedor)
        {
            // String SQL
            string sql = "CALL sp_DeletarFornecedor(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", fornecedor.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar


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
        } // Fim ValidarCampo



        // Listar todos
        public static List<Fornecedor> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosFornecedor()";

            List<Fornecedor> listaFornecedor = new();

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


                        // Cria o objeto Cliente con os dados extraidos
                        Fornecedor fornecedor = new Fornecedor(id, nome, contato, ddd, telefone, endereco, bairro, cidade, estado, cep);

                        // Adiciona este cliente à nossa lista
                        listaFornecedor.Add(fornecedor);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaFornecedor;

            }

        } //Fim ListarTodos



        // Listar por ID
        public static Fornecedor? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarFornecedorPorId(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Fornecedor fornecedor = null;

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

                    // Cria o objeto Cliente con os dados extraidos
                    fornecedor = new Fornecedor(idListado, nome, contato, ddd, telefone, endereco, bairro, cidade, estado, cep);
                }

                return fornecedor;
            }
        } // Fim ListarPorId


        // Atualizar
        public static int Atualizar(Fornecedor fornecedor)
        {

            string sql = "CALL sp_AtualizarFornecedor(@Id, @Nome, @Contato, @DDD, @Telefone, @Endereco, @Bairro, @Cidade, @Estado, @CEP)";
            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", fornecedor.Nome);
                cmd.Parameters.AddWithValue("@Contato", fornecedor.Contato);
                cmd.Parameters.AddWithValue("@DDD", fornecedor.Ddd);
                cmd.Parameters.AddWithValue("@Telefone", fornecedor.Telefone);
                cmd.Parameters.AddWithValue("@Endereco", fornecedor.Endereco);
                cmd.Parameters.AddWithValue("@Bairro", fornecedor.Bairro);
                cmd.Parameters.AddWithValue("@Cidade", fornecedor.Cidade);
                cmd.Parameters.AddWithValue("@Estado", fornecedor.Estado);
                cmd.Parameters.AddWithValue("@CEP", fornecedor.Cep);

                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", fornecedor.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    } // Classe
} // namespace
