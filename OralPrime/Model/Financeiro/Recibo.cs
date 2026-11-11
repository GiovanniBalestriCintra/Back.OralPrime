using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class Recibo
    {

        // Variáveis
        private int _id;
        private string _numero;
        private DateTime _emissao;
        private decimal _valor;
        private string _referente;
        private string _observacao;

        public int Id { get => _id; set => _id = value; }
        public string Numero { get => _numero; set => _numero = value; }
        public DateTime Emissao { get => _emissao; set => _emissao = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public string Referente { get => _referente; set => _referente = value; }
        public string Observacao { get => _observacao; set => _observacao = value; }



        // Método construtor sem ID
        public Recibo(string numero, DateTime emissao, decimal valor, string referente, string observacao)
        {
            _numero = numero;
            _emissao = emissao;
            _valor = valor;
            _referente = referente;
            _observacao = observacao;
        }

        // Método construtor com ID
        public Recibo(int id, string numero, DateTime emissao, decimal valor, string referente, string observacao)
        {
            _id = id;
            _numero = numero;
            _emissao = emissao;
            _valor = valor;
            _referente = referente;
            _observacao = observacao;
        }


        // Inserir
        public static int Inserir(Recibo recibo)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "CALL sp_InserirRecibo(@Numero, @Emissao, @Valor, @Referente, @Observacao)";
            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", recibo.Numero);
                cmd.Parameters.AddWithValue("@NumDocumento", recibo.Emissao);
                cmd.Parameters.AddWithValue("@DtVencimento", recibo.Valor);
                cmd.Parameters.AddWithValue("@Valor", recibo.Referente);
                cmd.Parameters.AddWithValue("@DtPagamento", recibo.Observacao);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar
        public static int Deletar(Recibo recibo)
        {
            // String SQL
            string sql = "CALL sp_DeletarRecibo(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", recibo.Id);

                return Banco.ExecutarComando(cmd);
            }

        } // Fim Deletar



        // ValidarCampo string
        private static string ValidarCampoString(object campo)
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


        // ValidarCampo string char
        private static char ValidarCampoChar(object campo)
        {
            if (campo == DBNull.Value || campo == null)
            {
                return '\0'; // caractere nulo
            }
            else
            {
                return Convert.ToChar(campo);
            }
        }


        private static bool CampoPreenchido(object campo)
        {
            return campo != DBNull.Value && campo != null;
        } // Fim ValidarCampo



        // Listar todos
        public static List<Recibo> ListarTodos()
        {
            string sql = "CALL sp_ListarTodosRecibo()";

            List<Recibo> listaRecibo = new();

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

                        string numero = ValidarCampoString(reader["Numero"]);
                        DateTime emissao = (DateTime)reader["Emissao"];
                        decimal valor = reader.GetDecimal("Valor");
                        string referente = ValidarCampoString(reader["Referente"]);
                        string observacao = ValidarCampoString(reader["Observacao"]);

                        // Cria o objeto Cliente con os dados extraidos
                        Recibo recibo = new Recibo(id, numero, emissao, valor, referente, observacao);

                        // Adiciona este boleto à nossa lista
                        listaRecibo.Add(recibo);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaRecibo;

            }

        } //Fim ListarTodos



        // Listar por ID
        public static Recibo? ListarPorId(int id)
        {
            string sql = "CALL sp_ListarPorIdRecibo(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Recibo recibo = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int id = reader.GetInt32("id");

                    string numero = ValidarCampoString(reader["Numero"]);
                    DateTime emissao = (DateTime)reader["Emissao"];
                    decimal valor = reader.GetDecimal("Valor");
                    string referente = ValidarCampoString(reader["Referente"]);
                    string observacao = ValidarCampoString(reader["Observacao"]);

                    // Cria o objeto Cliente con os dados extraidos
                    recibo = new Recibo(id, numero, emissao, valor, referente, observacao);
                }

                return recibo;
            }
        } // Fim ListarPorId


        // Atualizar
        public static int Atualizar(Recibo recibo)
        {
            string sql = "CALL sp_AtualizarRecibo(@Id)";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", recibo.Numero);
                cmd.Parameters.AddWithValue("@NumDocumento", recibo.Emissao);
                cmd.Parameters.AddWithValue("@DtVencimento", recibo.Valor);
                cmd.Parameters.AddWithValue("@Valor", recibo.Referente);
                cmd.Parameters.AddWithValue("@DtPagamento", recibo.Observacao);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar






    } // Classe

} // Namespace
