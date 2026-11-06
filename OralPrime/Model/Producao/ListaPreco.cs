using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OralPrime.Model.Producao
{
    internal class ListaPreco
    {

        // Variáveis
        private int _id;
        private string _descricao;
        private decimal _lista_1;
        private decimal _lista_2;
        private decimal _lista_3;
        private decimal _lista_4;
        private decimal _lista_5;
        private decimal _lista_6;
        private decimal _lista_7;
        private decimal _lista_8;
        private decimal _lista_9;
        private decimal _lista_10;
        private decimal _lista_11;
        private decimal _lista_12;
        private decimal _lista_13;
        private decimal _lista_14;
        private decimal _lista_15;
        private decimal _lista_16;
        private decimal _lista_17;
        private decimal _lista_18;
        private decimal _lista_19;
        private decimal _lista_20;

        public int Id { get => _id; set => _id = value; }
        public string Descricao { get => _descricao; set => _descricao = value; }
        public decimal Lista_1 { get => _lista_1; set => _lista_1 = value; }
        public decimal Lista_2 { get => _lista_2; set => _lista_2 = value; }
        public decimal Lista_3 { get => _lista_3; set => _lista_3 = value; }
        public decimal Lista_4 { get => _lista_4; set => _lista_4 = value; }
        public decimal Lista_5 { get => _lista_5; set => _lista_5 = value; }
        public decimal Lista_6 { get => _lista_6; set => _lista_6 = value; }
        public decimal Lista_7 { get => _lista_7; set => _lista_7 = value; }
        public decimal Lista_8 { get => _lista_8; set => _lista_8 = value; }
        public decimal Lista_9 { get => _lista_9; set => _lista_9 = value; }
        public decimal Lista_10 { get => _lista_10; set => _lista_10 = value; }
        public decimal Lista_11 { get => _lista_11; set => _lista_11 = value; }
        public decimal Lista_12 { get => _lista_12; set => _lista_12 = value; }
        public decimal Lista_13 { get => _lista_13; set => _lista_13 = value; }
        public decimal Lista_14 { get => _lista_14; set => _lista_14 = value; }
        public decimal Lista_15 { get => _lista_15; set => _lista_15 = value; }
        public decimal Lista_16 { get => _lista_16; set => _lista_16 = value; }
        public decimal Lista_17 { get => _lista_17; set => _lista_17 = value; }
        public decimal Lista_18 { get => _lista_18; set => _lista_18 = value; }
        public decimal Lista_19 { get => _lista_19; set => _lista_19 = value; }
        public decimal Lista_20 { get => _lista_20; set => _lista_20 = value; }


        // Método construtor com Id
        ListaPreco (int id, string descricao, decimal lista_1, decimal lista_2, decimal lista_3, decimal lista_4, decimal lista_5, decimal lista_6, decimal lista_7, decimal lista_8, decimal lista_9, decimal lista_10, decimal lista_11, decimal lista_12, decimal lista_13, decimal lista_14, decimal lista_15, decimal lista_16, decimal lista_17, decimal lista_18, decimal lista_19, decimal lista_20)
        {
            this._id = id;
            this._descricao = descricao;
            this._lista_1 = lista_1;
            this._lista_2 = lista_2;
            this._lista_3 = lista_3;
            this._lista_4 = lista_4;
            this._lista_5 = lista_5;
            this._lista_6 = lista_6;
            this._lista_7 = lista_7;
            this._lista_8 = lista_8;
            this._lista_9 = lista_9;
            this._lista_10 = lista_10;
            this._lista_11 = lista_11;
            this._lista_11 = lista_11;
            this._lista_12  = lista_12;
            this._lista_13 = lista_13;
            this._lista_14 = lista_14;
            this._lista_15 = lista_15;
            this._lista_16 = lista_16;
            this._lista_17 = lista_17;
            this._lista_18 = lista_18;
            this._lista_19 = lista_19;
            this._lista_20 = lista_20;
        }

        // Método construtor sem ID
        ListaPreco(string descricao, decimal lista_1, decimal lista_2, decimal lista_3, decimal lista_4, decimal lista_5, decimal lista_6, decimal lista_7, decimal lista_8, decimal lista_9, decimal lista_10, decimal lista_11, decimal lista_12, decimal lista_13, decimal lista_14, decimal lista_15, decimal lista_16, decimal lista_17, decimal lista_18, decimal lista_19, decimal lista_20)
        {
            this._descricao = descricao;
            this._lista_1 = lista_1;
            this._lista_2 = lista_2;
            this._lista_3 = lista_3;
            this._lista_4 = lista_4;
            this._lista_5 = lista_5;
            this._lista_6 = lista_6;
            this._lista_7 = lista_7;
            this._lista_8 = lista_8;
            this._lista_9 = lista_9;
            this._lista_10 = lista_10;
            this._lista_11 = lista_11;
            this._lista_11 = lista_11;
            this._lista_12 = lista_12;
            this._lista_13 = lista_13;
            this._lista_14 = lista_14;
            this._lista_15 = lista_15;
            this._lista_16 = lista_16;
            this._lista_17 = lista_17;
            this._lista_18 = lista_18;
            this._lista_19 = lista_19;
            this._lista_20 = lista_20;
        }




        // Inserir ListaPreço
        public static int Inserir(ListaPreco listaPreco)
        {
            // Instrução SQL: Define o comando de inserção (INSERT).
            // 0 uso de marcações com '0' (ex: ONone) é una pratica de segurança crucial chanada de "Paranetrização".
            // Ela impede ataques de SQL Injection, pois o banco entende o que e comando e o que e apenas texto do usuário.

            string sql = "INSERT INTO `tb_ListaPreco` (`descricao`, `lista_1`, `lista_2`, `lista_3`, `lista_4`, `lista_5`, `lista_6`, `lista_7`, `lista_8`, `lista_9`, `lista_10`, `lista_11`, `lista_12`, `lista_13`, `lista_14`, `lista_15`, `lista_16`, `lista_17`, `lista_18`, `lista_19`, `lista_20`,) VALUES (@Descricao, @Lista_1, @Lista_2, @Lista_3, @Lista_4, @Lista_5, @Lista_6, @Lista_7, @Lista_8, @Lista_9, @Lista_10, @Lista_11, @Lista_12, @Lista_13, @Lista_14, @Lista_15, @Lista_16, @Lista_17, @Lista_18, @Lista_19, @Lista_20, )";

            // Gerenciamento de recursos: Cria o objeto de comando ja passando a instrução SQL.
            // 0 'using' garante que os recursos de nemoria ocupados pelo comando sejam Liberados ao final do bloco.
            using (var cmd = new MySqlCommand(sql))
            {
                // Vinculação de Dados: Pega as propriedades do objeto C# (cliente.Nome, cliente.cpf, etc.)
                // e substitui de forna segura as narcacões '0' da string SQL com os valores reais,

                cmd.Parameters.AddWithValue("@Nome", listaPreco.Descricao);
                cmd.Parameters.AddWithValue("@Lista_1", listaPreco.Lista_1);
                cmd.Parameters.AddWithValue("@Lista_2", listaPreco.Lista_2);
                cmd.Parameters.AddWithValue("@Lista_3", listaPreco.Lista_3);
                cmd.Parameters.AddWithValue("@Lista_4", listaPreco.Lista_4);
                cmd.Parameters.AddWithValue("@Lista_5", listaPreco.Lista_5);
                cmd.Parameters.AddWithValue("@Lista_6", listaPreco.Lista_6);
                cmd.Parameters.AddWithValue("@Lista_7", listaPreco.Lista_7);
                cmd.Parameters.AddWithValue("@Lista_8", listaPreco.Lista_8);
                cmd.Parameters.AddWithValue("@Lista_9", listaPreco.Lista_9);
                cmd.Parameters.AddWithValue("@Lista_10", listaPreco.Lista_10);
                cmd.Parameters.AddWithValue("@Lista_11", listaPreco.Lista_11);
                cmd.Parameters.AddWithValue("@Lista_12", listaPreco.Lista_12);
                cmd.Parameters.AddWithValue("@Lista_13", listaPreco.Lista_13);
                cmd.Parameters.AddWithValue("@Lista_14", listaPreco.Lista_14);
                cmd.Parameters.AddWithValue("@Lista_15", listaPreco.Lista_15);
                cmd.Parameters.AddWithValue("@Lista_16", listaPreco.Lista_16);
                cmd.Parameters.AddWithValue("@Lista_17", listaPreco.Lista_17);
                cmd.Parameters.AddWithValue("@Lista_18", listaPreco.Lista_18);
                cmd.Parameters.AddWithValue("@Lista_19", listaPreco.Lista_19);
                cmd.Parameters.AddWithValue("@Lista_20", listaPreco.Lista_20);
                // Execucão: Como o comando de INSERT nao retorna tabelas (apenas executa una ação),
                // ele envia o comando configurado para o metodo generico 'executarComando' da classe 'Banco'.
                // O metodo retorna 1 (se o usuario for inserido con sucesso) ou -1 (se houver erro no banco).

                return Banco.ExecutarComando(cmd);
            }
        } // Fim Inserir



        // Deletar ListaPreço
        public static int Deletar(ListaPreco listaPreco)
        {
            // String SQL
            string sql = "DELETE FROM `tb_ListaPreco` WHERE `ID` = @id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@id", listaPreco.Id);

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



        // Listar todas as ListasPreço
        public static List<ListaPreco> ListarTodos()
        {
            string sql = "SELECT * FROM `tb_listaPreco`";

            List<ListaPreco> listarPrecos = new();

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

                        string descricao = ValidarCampo(reader["Descricao"]);
                        decimal lista_1 = reader.GetDecimal("Lista_1");
                        decimal lista_2 = reader.GetDecimal("Lista_2");
                        decimal lista_3 = reader.GetDecimal("Lista_3");
                        decimal lista_4 = reader.GetDecimal("Lista_4");
                        decimal lista_5 = reader.GetDecimal("Lista_5");
                        decimal lista_6 = reader.GetDecimal("Lista_6");
                        decimal lista_7 = reader.GetDecimal("Lista_7");
                        decimal lista_8 = reader.GetDecimal("Lista_8");
                        decimal lista_9 = reader.GetDecimal("Lista_9");
                        decimal lista_10 = reader.GetDecimal("Lista_10");
                        decimal lista_11 = reader.GetDecimal("Lista_11");
                        decimal lista_12 = reader.GetDecimal("Lista_12");
                        decimal lista_13 = reader.GetDecimal("Lista_13");
                        decimal lista_14 = reader.GetDecimal("Lista_14");
                        decimal lista_15 = reader.GetDecimal("Lista_15");
                        decimal lista_16 = reader.GetDecimal("Lista_16");
                        decimal lista_17 = reader.GetDecimal("Lista_17");
                        decimal lista_18 = reader.GetDecimal("Lista_18");
                        decimal lista_19 = reader.GetDecimal("Lista_19");
                        decimal lista_20 = reader.GetDecimal("Lista_20");



                        // Cria o objeto Cliente com os dados extraidos
                        ListaPreco listaPreco = new ListaPreco(idLista, descricao, lista_1, lista_2, lista_3, lista_4, lista_5, lista_6, lista_7, lista_8, lista_9, lista_10, lista_11, lista_12, lista_13, lista_14, lista_15, lista_16, lista_17, lista_18, lista_19, lista_10);

                        // Adiciona este cliente à nossa lista
                        listarPrecos.Add(listaPreco);

                    }

                }

                // Retorna a lista completa (se a tabela estiver vazia, retorna uma lista con 0 itens)
                return listarPrecos;

            }
        } // Fim ListarTodos



        // Listar Por ID
        public static ListaPreco? ListarPorId(int id)
        {
            string sql = "SELECT * FROM `tb_Listar` WHERE `id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataReader reader = Banco.ExecutarComRetornoDeDados(cmd);

                ListaPreco listarPreco = null;

                while (reader.Read())
                {

                    // Extrai os dados da linha atual.
                    int idLista = reader.GetInt32("id");

                    string descricao = ValidarCampo(reader["Descricao"]);
                    decimal lista_1 = reader.GetDecimal("Lista_1");
                    decimal lista_2 = reader.GetDecimal("Lista_2");
                    decimal lista_3 = reader.GetDecimal("Lista_3");
                    decimal lista_4 = reader.GetDecimal("Lista_4");
                    decimal lista_5 = reader.GetDecimal("Lista_5");
                    decimal lista_6 = reader.GetDecimal("Lista_6");
                    decimal lista_7 = reader.GetDecimal("Lista_7");
                    decimal lista_8 = reader.GetDecimal("Lista_8");
                    decimal lista_9 = reader.GetDecimal("Lista_9");
                    decimal lista_10 = reader.GetDecimal("Lista_10");
                    decimal lista_11 = reader.GetDecimal("Lista_11");
                    decimal lista_12 = reader.GetDecimal("Lista_12");
                    decimal lista_13 = reader.GetDecimal("Lista_13");
                    decimal lista_14 = reader.GetDecimal("Lista_14");
                    decimal lista_15 = reader.GetDecimal("Lista_15");
                    decimal lista_16 = reader.GetDecimal("Lista_16");
                    decimal lista_17 = reader.GetDecimal("Lista_17");
                    decimal lista_18 = reader.GetDecimal("Lista_18");
                    decimal lista_19 = reader.GetDecimal("Lista_19");
                    decimal lista_20 = reader.GetDecimal("Lista_20");

                    // Cria o objeto Cliente con os dados extraidos
                    ListaPreco listaPreco = new ListaPreco(idLista, descricao, lista_1, lista_2, lista_3, lista_4, lista_5, lista_6, lista_7, lista_8, lista_9, lista_10, lista_11, lista_12, lista_13, lista_14, lista_15, lista_16, lista_17, lista_18, lista_19, lista_10);
                }

                return listarPrecos;
            }
        } // Fim ListarPorId



        // Atualizar funcionario
        public static int Atualizar(ListaPreco listaPreco)
        {
            string sql = "UPDATE `tb_ListarPreco` SET `descricao` = @Descricao, `lista_1` = @Lista_1, `lista_2` = @Lista_2, `lista_3` = @Lista_3, `lista_4` = @Lista_4, `lista_5` = @Lista_5, `lista_6` = @Lista_6, `lista_7` = @Lista_7, `lista_8` = @Lista_8, `lista_9` = @Lista_9, `lista_10` = @Lista_10, `lista_11` = @Lista_11, `lista_12` = @Lista_12, `lista_13` = @Lista_13, `lista_14` = @Lista_14, `lista_15` = @Lista_15, `lista_16` = @Lista_16, `lista_17` = @Lista_17, `lista_18` = @Lista_18, `lista_19` = @Lista_19, `lista_20` = @Lista_20 WHERE `Id` = @Id";

            using (var cmd = new MySqlCommand(sql))
            {

                // Vinculacão de paranetros: Protege contra SQL Injection e organiza os dados
                cmd.Parameters.AddWithValue("@Nome", listaPreco.Descricao);
                cmd.Parameters.AddWithValue("@Lista_1", listaPreco.Lista_1);
                cmd.Parameters.AddWithValue("@Lista_2", listaPreco.Lista_2);
                cmd.Parameters.AddWithValue("@Lista_3", listaPreco.Lista_3);
                cmd.Parameters.AddWithValue("@Lista_4", listaPreco.Lista_4);
                cmd.Parameters.AddWithValue("@Lista_5", listaPreco.Lista_5);
                cmd.Parameters.AddWithValue("@Lista_6", listaPreco.Lista_6);
                cmd.Parameters.AddWithValue("@Lista_7", listaPreco.Lista_7);
                cmd.Parameters.AddWithValue("@Lista_8", listaPreco.Lista_8);
                cmd.Parameters.AddWithValue("@Lista_9", listaPreco.Lista_9);
                cmd.Parameters.AddWithValue("@Lista_10", listaPreco.Lista_10);
                cmd.Parameters.AddWithValue("@Lista_11", listaPreco.Lista_11);
                cmd.Parameters.AddWithValue("@Lista_12", listaPreco.Lista_12);
                cmd.Parameters.AddWithValue("@Lista_13", listaPreco.Lista_13);
                cmd.Parameters.AddWithValue("@Lista_14", listaPreco.Lista_14);
                cmd.Parameters.AddWithValue("@Lista_15", listaPreco.Lista_15);
                cmd.Parameters.AddWithValue("@Lista_16", listaPreco.Lista_16);
                cmd.Parameters.AddWithValue("@Lista_17", listaPreco.Lista_17);
                cmd.Parameters.AddWithValue("@Lista_18", listaPreco.Lista_18);
                cmd.Parameters.AddWithValue("@Lista_19", listaPreco.Lista_19);
                cmd.Parameters.AddWithValue("@Lista_20", listaPreco.Lista_20);


                // A chave de busca: Quem sera atualizado?
                cmd.Parameters.AddWithValue("@Id", listaPreco.Id);

                // Execução: 0 UPOATE tambén é uma instrução de ação (cono INSERT e DELETE).
                // Ele vai retornar 1 (se encontrou o ID e atualizou con sucesso) ou 0 (se o ID nao existia).
                return Banco.ExecutarComando(cmd);

            }
        } // Fim Atualizar


    }// Classe

} // Namespace
