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
        private string __email;

       

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


    }// Classe

}// NameSpace
