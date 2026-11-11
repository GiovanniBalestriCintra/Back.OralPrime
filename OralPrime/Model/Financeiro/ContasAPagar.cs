using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class ContasAPagar
    {

        private int _id;
        private DateTime _data;
        private DateTime dtVencimento;
        private decimal _valor;
        private DateTime _dtPagamento;
        private decimal _valorPago;
        private string _historico;
        private bool _final;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public DateTime DtVencimento { get => dtVencimento; set => dtVencimento = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public DateTime DtPagamento { get => _dtPagamento; set => _dtPagamento = value; }
        public decimal ValorPago { get => _valorPago; set => _valorPago = value; }
        public string Historico { get => _historico; set => _historico = value; }
        public bool Final { get => _final; set => _final = value; }


        // Método construtor com ID
        public ContasAPagar(int id, DateTime data, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _id = id;
            _data = data;
            this.dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _historico = historico;
            _final = final;
        }


        // Método contrutor sem ID
        public ContasAPagar(DateTime data, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string historico, bool final)
        {
            _data = data;
            this.dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _historico = historico;
            _final = final;
        }



        // Inserir Contas a Pagar
        public static int Inserir(ContasAPagar contasAPagar)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_ContasAPagar` (`data`, `dtVencimento`, `valor`, `dtPagamento`, `valorPago`, `historico`, `final`) VALUES (@Data, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Historico, @Final)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar Contas a Pagar
        public static int Deletar(ContasAPagar contasAPagar)
        {
            // String SQL
            string sql = "DELETE FROM `tb_ContasAPagar` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", contasAPagar.Id);

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



        // Listar todos os Boletos
        public static List<ContasAPagar> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_ContasAPagar`";

            List<ContasAPagar> listaContasAPagar = new();

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
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string historico = ValidarCampo(reader["Historico"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        // Cria o objeto Cliente con os dados extraidos
                        ContasAPagar contasAPagar = new ContasAPagar(id, data, dtVencimento, valor, dtPagamento, valorPago, historico, final);

                        // Adiciona este boleto à nossa lista
                        listaContasAPagar.Add(contasAPagar);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaContasAPagar;

            }

        } //Fim ListarTodos



        // Listar Boleto por ID
        public static ContasAPagar? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_ContasAPagar` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                ContasAPagar contasAPagar = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                    decimal valor = reader.GetDecimal("Valor");
                    DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                    decimal valorPago = reader.GetDecimal("ValorPago");
                    string historico = ValidarCampo(reader["Historico"]);
                    bool final = CampoPreenchido(reader["Final"]);

                    // Cria o objeto Cliente con os dados extraidos
                    contasAPagar = new ContasAPagar(idListado, data, dtVencimento, valor, dtPagamento, valorPago, historico, final);
                }

                return contasAPagar;
            }
        } // Fim ListarPorId



        // Atualizar Contas a pagar
        public static int Atualizar(ContasAPagar contasAPagar)
        {
            string sql = "UPDATE `tb_ContasAPagar` SET `Data` = @Data, `DtVencimento` = @DtVencimento, `Valor` = @Valor, `DtPagamento` = @DtPagamento, `ValorPago` = @ValorPago, `Historico` = @Historico, `Final` = @Final WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", contasAPagar.Data);
                cmd.Parameters.AddWithValue("@DtVencimento", contasAPagar.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", contasAPagar.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", contasAPagar.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", contasAPagar.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", contasAPagar.Historico);
                cmd.Parameters.AddWithValue("@Final", contasAPagar.Final);


                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar








    } // Classe
} // Namespace

