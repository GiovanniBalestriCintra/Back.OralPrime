using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class Pedido
    {

        // Variáveis
        private int _ficha;
        private string _nome;
        private string _paciente;
        private DateTime _entrada;
        private DateTime? _prevista;
        private DateTime? _finalizado;
        private string _escala;
        private string _cor;
        private decimal _total;


        public int Ficha { get => _ficha; set => _ficha = value; }
        public string Nome { get => _nome; set => _nome = value; }
        public string Paciente { get => _paciente; set => _paciente = value; }
        public DateTime Entrada { get => _entrada; set => _entrada = value; }
        public DateTime? Prevista { get => _prevista; set => _prevista = value; }
        public DateTime? Finalizado { get => _finalizado; set => _finalizado = value; }
        public string Escala { get => _escala; set => _escala = value; }
        public string Cor { get => _cor; set => _cor = value; }
        public decimal Total { get => _total; set => _total = value; }




        // Método construtor sem ID
        public Pedido(string nome, string paciente, DateTime entrada, DateTime? prevista, DateTime? finalizado, string escala, string cor, decimal total)
        {
            this._nome = nome;
            this._paciente = paciente;
            this._entrada = entrada;
            this._prevista = prevista;
            this._finalizado = finalizado;
            this._escala = escala;
            this._cor = cor;
            this._total = total;
        }



        // Método construtor com ID
        public Pedido(int ficha, string nome, string paciente, DateTime entrada, DateTime? prevista, DateTime? finalizado, string escala, string cor, decimal total)
        {
            this._ficha = ficha;
            this._nome = nome;
            this._paciente = paciente;
            this._entrada = entrada;
            this._prevista = prevista;
            this._finalizado = finalizado;
            this._escala = escala;
            this._cor = cor;
            this._total = total;
        }




        // Inserir Funcionário
        public static int Inserir(Pedido pedido)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_pedido` (`nome`, `paciente`, `entrada`, `prevista`, `finalizado`, `escala`, `cor`, `total`) VALUES (@Nome, @Paciente, @entrada, @Prevista, @Finalizado, @Escala, @Cor, @Total)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", pedido.Nome);
                cmd.Parameters.AddWithValue("@Paciente", pedido.Paciente);
                cmd.Parameters.AddWithValue("@Entrada", pedido.Entrada);
                cmd.Parameters.AddWithValue("@Prevista", pedido.Prevista);
                cmd.Parameters.AddWithValue("@Finalizado", pedido.Finalizado);
                cmd.Parameters.AddWithValue("@Escala", pedido.Escala);
                cmd.Parameters.AddWithValue("@Cor", pedido.Cor);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir




        // Deletar Funcionário
        public static int Deletar(Pedido pedido)
        {
            // String SQL
            string sql = "DELETE FROM `tb_Pedido` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", pedido.Ficha);

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
        public static List<Pedido> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_Pedido`";

            List<Pedido> listaPedido = new();

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
                        int fichaLista = reader.GetInt32("id");

                        string nome = ValidarCampo(reader["Nome"]);
                        string paciente = ValidarCampo(reader["Paciente"]);
                        DateTime entrada = (DateTime)(reader["Entrada"]);
                        DateTime prevista = (DateTime)(reader["Prevista"]);
                        DateTime finalizado = (DateTime)(reader["Finalizado"]);
                        string escala = ValidarCampo(reader["Escala"]);
                        string cor = ValidarCampo(reader["Cor"]);
                        decimal total = reader.GetDecimal("Total");


                        // Cria o objeto Cliente com os dados extraidos
                        Pedido pedido = new Pedido(fichaLista, nome, paciente, entrada, prevista, finalizado, escala, cor, total);

                        // Adiciona este cliente à nossa lista
                        listaPedido.Add(pedido);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaPedido;

            }

        } //Fim ListarTodos



        // Listar Por ID
        public static Pedido? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_Pedido` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Pedido pedido = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int fichaLista = reader.GetInt32("id");

                    string nome = ValidarCampo(reader["Nome"]);
                    string paciente = ValidarCampo(reader["Paciente"]);
                    DateTime entrada = (DateTime)(reader["Entrada"]);
                    DateTime prevista = (DateTime)(reader["Prevista"]);
                    DateTime finalizado = (DateTime)(reader["Finalizado"]);
                    string escala = ValidarCampo(reader["Escala"]);
                    string cor = ValidarCampo(reader["Cor"]);
                    decimal total = reader.GetDecimal("Total");

                    // Cria o objeto Cliente con os dados extraidos
                    pedido = new Pedido(fichaLista, nome, paciente, entrada, prevista, finalizado, escala, cor, total);
                }

                return pedido;
            }
        } // Fim ListarPorId



        // Atualizar funcionario
        public static int Atualizar(Pedido pedido)
        {
            string sql = "UPDATE `tb_Pedido` SET `nome` = @Nome, `paciente` = @Paciente, `entrada` = @Entrada, `prevista` = @Prevista, `finalizado` = @Finalizado, `escala` = @Escala, `cor` = @Cor, `total` = @Total WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", pedido.Nome);
                cmd.Parameters.AddWithValue("@Paciente", pedido.Paciente);
                cmd.Parameters.AddWithValue("@Entrada", pedido.Entrada);
                cmd.Parameters.AddWithValue("@Prevista", pedido.Prevista);
                cmd.Parameters.AddWithValue("@Finalizado", pedido.Finalizado);
                cmd.Parameters.AddWithValue("@Escala", pedido.Escala);
                cmd.Parameters.AddWithValue("@Cor", pedido.Cor);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);


                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", pedido.Ficha);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    }
}
