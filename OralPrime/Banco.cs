using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace OralPrime
{
    public static class Banco
    {

        private static string stringConexao = "Server=10.91.45.33;Port=3306;Database=padaria_bd;Uid=admin;Pwd=12345678";


        public static void ConnectAndTest()
        {
            using (var connection = new MySqlConnection(stringConexao))
            {

                // Tenta fazer
                try
                {

                    Console.WriteLine("Tentando conectar ao banco de dados");

                    connection.Open();

                    Console.WriteLine("Conexão bem sucedida");

                    using (var command = new MySqlCommand("SELECT VERSION();", connection))
                    {
                        string? version = command.ExecuteScalar()?.ToString();
                        Console.WriteLine($"Versão do MariaDB {version}");
                    }

                    // Captura o erro
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Erro ao conectar no banco de dados");
                    Console.WriteLine(ex.Message);

                }

            }
        }



        /* Declaração do método: público e estatico, retorna um número inteiro (linhas afetadas)
        e recebe um objeto MysqlCommand com a intrução SQL já confirmada como parâmetro. */
        public static int ExecutarComando(MySqlCommand command)
        {
            /* O bloco 'using' garante que a conexao sera automaticanente fechada e Liberada
            da memória ao final do bloco, mesmo que ocorram erros no meio do processo. 

            Cria a conexão utilizando a variável de ambiente
            */

            using (var conexao = new MySqlConnection(stringConexao))
            {
                // Vincular: Diz ao comando SQL em qual conexão ele deve ser executada.
                command.Connection = conexao;

                // tratamento de erros: Inicia um bloco de tentativa para proteger o programa de falhas abruptas.
                try
                {
                    // Abertura da conexão: Estabelece efetivamente a comunicação com o servidor MSQL.
                    conexao.Open();


                    // Execução: 0 ExecuteNonQuery dispara comandos de ação (INSERT, UPDATE, DELETE).
                    // Ele nao retorna registros, mas sim o numero inteiro de linhas que foram alteradas,
                    // e o 'return' devolve esse nunero para quem chanou o metodo.
                    return command.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro de Banco de Dados {ex.Message}");

                    return -1;
                }
            }
        }



        // Declaração do metodo: publico e estatico, prjetado especificamente para executar comandos
        // (geralmente INSERTs seguidos de SELECT LAST_INSERT_ID()) e retornar o ID gerado pelo banco.

        public static int ExecutarComandoComRetornoId(MySqlCommand command)
        {
            using (var conexao = new MySqlConnection(stringConexao))
            {
                command.Connection = conexao;

                try
                {
                    conexao.Open();

                    /* O ExecuteScalar executa o comando e retorna APENAS a primeira coluna da primeira
                     linha do resultado. É perfeito para pegar um único valor (como um ID recém-gerado).
                     Ele retorna um tipo 'object' porque nao sabe de antemao qual o tipo do dado no banco.*/
                    object objRetorno = command.ExecuteScalar();

                    int retorno = 0;

                    /* Verificação de segurança: Checa se o banco realmente retornou algo.
                    Verifica tanto se o objeto C# não é nulo (null) quanto se
                    não é um nulo do próprio banco de dados (DBNull.Value).
                    */


                    if (objRetorno != null && objRetorno != DBNull.Value)
                    {

                        /* Se passou na verificação, converte o objeto (que veio como texto/objeto)
                        para um numero inteiro (int). */

                        retorno = int.Parse(objRetorno.ToString());

                    }

                    return retorno;

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro de Banco de Dados{ex.Message}");
                    return -1;
                }
            }
        } // Metodo RetornoComId




        // Metodo central para executar comandos de consulta (SELECT) no banco de dados.
        // Ele retorna um MySqLDataReader, que e uma especie de "ponteiro" para ler os dados linha por linha.
        public static MySqlDataReader ExecutarComRetornoDeDados(MySqlCommand command)
        {

            // Cria a conexão com o banco.
            // ATENÇÃO: Aqui NÃO usamos o bloco 'using' para a conexão. Se usássemos, a conexão
            // seria fechada antes de retornar os dados, e o programa quebraria ao tentar ler a tabela.
            var conexao = new MySqlConnection(stringConexao);

            command.Connection = conexao;

            try
            {
                // Abre a porta de comunicação com o MySql
                conexao.Open();

                // Executa a consulta e retorna o "leitor" de dados.
                // 0 CommandBehavior.CloseConnection e a peca-chave deste metodo: ele avisa ao leitor
                // que assin que ele (o leitor) for fechado ou descartado la na tela/classe que o chanou,
                // a conexao com o banco deve ser fechada automaticamente junto con ele,
                return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (MySqlException ex)
            {
                // O Dispose() já executa o Close() e libera a memória.
                conexao.Dispose();

                Console.WriteLine($"Erro na leitura do Banco de Dados: {ex.Message}");

                // Ao inves de retornar null, relança o erro (throw) para que o metodo que
                // chamou este ExecuteReader saiba que a pesquisa falhou e possa tratar a tela do usuário.
                throw;
            }

        }




    } // Classe

} // NameSpace
