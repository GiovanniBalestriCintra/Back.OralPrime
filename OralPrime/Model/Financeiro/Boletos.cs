using MySqlConnector;
using OralPrime.Model.Producao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Financeiro
{
    internal class Boleto
    {

        private int _id;
        private DateTime _data;
        private string _numDocumento;
        private DateTime _dtVencimento;
        private decimal _valor;
        private DateTime _dtPagamento;
        private decimal _valorPago;
        private string _observação;
        private bool _final;

        public int Id { get => _id; set => _id = value; }
        public DateTime Data { get => _data; set => _data = value; }
        public string NumDocumento { get => _numDocumento; set => _numDocumento = value; }
        public DateTime DtVencimento { get => _dtVencimento; set => _dtVencimento = value; }
        public decimal Valor { get => _valor; set => _valor = value; }
        public DateTime DtPagamento { get => _dtPagamento; set => _dtPagamento = value; }
        public decimal ValorPago { get => _valorPago; set => _valorPago = value; }
        public string Observação { get => _observação; set => _observação = value; }
        public bool Final { get => _final; set => _final = value; }

        // Método construtor com ID
        public Boleto(int id, DateTime data, string numDocumento, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string observação, bool final)
        {
            _id = id;
            _data = data;
            _numDocumento = numDocumento;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _observação = observação;
            _final = final;
        }

        // Método construtor sem ID
        public Boleto(DateTime data, string numDocumento, DateTime dtVencimento, decimal valor, DateTime dtPagamento, decimal valorPago, string observação, bool final)
        {
            _data = data;
            _numDocumento = numDocumento;
            _dtVencimento = dtVencimento;
            _valor = valor;
            _dtPagamento = dtPagamento;
            _valorPago = valorPago;
            _observação = observação;
            _final = final;
        }


        // Inserir Boleto
        public static int Inserir(Boleto boleto)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_Boletos` (`data`, `numDocumento`, `dtVencimento`, `valor`, `dtPagamento`, `valorPago`, `observação`, `final`) VALUES (@Data, @NumDocumento, @DtVencimento, @Valor, @DtPagamento, @ValorPago, @Observação, @Final)";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Data", boleto.Data);
                cmd.Parameters.AddWithValue("@NumDocumento", boleto.NumDocumento);
                cmd.Parameters.AddWithValue("@DtVencimento", boleto.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", boleto.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", boleto.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", boleto.ValorPago);
                cmd.Parameters.AddWithValue("@Observação", boleto.Observação);
                cmd.Parameters.AddWithValue("@Final", boleto.Final);

                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar Boleto
        public static int Deletar(Boleto boleto)
        {
            // String SQL
            string sql = "DELETE FROM `tb_Boletos` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", boleto.Id);

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
        public static List<Boleto> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_Boletos`";

            List<Boleto> listaBoletos = new();

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
                        string numDocumento = ValidarCampo(reader["NumDocumento"]);
                        DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                        decimal valor = reader.GetDecimal("Valor");
                        DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                        decimal valorPago = reader.GetDecimal("ValorPago");
                        string observacao = ValidarCampo(reader["Observacao"]);
                        string estado = ValidarCampo(reader["Estado"]);
                        bool final = CampoPreenchido(reader["Final"]);

                        // Cria o objeto Cliente con os dados extraidos
                        Boleto boleto = new Boleto(id, data, numDocumento, dtVencimento, valor, dtPagamento, valorPago, observacao, final);

                        // Adiciona este boleto à nossa lista
                        listaBoletos.Add(boleto);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listaBoletos;

            }

        } //Fim ListarTodos



        // Listar Boleto por ID
        public static Boleto? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_Boletos` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                Boleto boleto = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idListado = reader.GetInt32("id");

                    DateTime data = (DateTime)reader["Data"];
                    string numDocumento = ValidarCampo(reader["NumDocumento"]);
                    DateTime dtVencimento = (DateTime)reader["DtVencimento"];
                    decimal valor = reader.GetDecimal("Valor");
                    DateTime dtPagamento = (DateTime)reader["DtPagamento"];
                    decimal valorPago = reader.GetDecimal("ValorPago");
                    string observacao = ValidarCampo(reader["Observacao"]);
                    string estado = ValidarCampo(reader["Estado"]);
                    bool final = CampoPreenchido(reader["Final"]);

                    // Cria o objeto Cliente con os dados extraidos
                    boleto = new Boleto(idListado, data, numDocumento, dtVencimento, valor, dtPagamento, valorPago, observacao, final);
                }

                return boleto;
            }
        } // Fim ListarPorId



        // Atualizar boleto
        public static int Atualizar(ContasAPagar conta)
        {
            string sql = "UPDATE `tb_ContasAPagar` SET `Data` = @Data, `DtVencimento` = @DtVencimento, `Valor` = @Valor, `DtPagamento` = @DtPagamento, `ValorPago` = @ValorPago, `Historico` = @Historico, `Final` = @Final WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Data", conta.Data);
                cmd.Parameters.AddWithValue("@DtVencimento", conta.DtVencimento);
                cmd.Parameters.AddWithValue("@Valor", conta.Valor);
                cmd.Parameters.AddWithValue("@DtPagamento", conta.DtPagamento);
                cmd.Parameters.AddWithValue("@ValorPago", conta.ValorPago);
                cmd.Parameters.AddWithValue("@Historico", conta.Historico);
                cmd.Parameters.AddWithValue("@Final", conta.Final);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    } // Classe

} // Namespace
