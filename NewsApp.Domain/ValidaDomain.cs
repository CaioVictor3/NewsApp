using NewsApp.Domain.Handle;
using System.Text.RegularExpressions;

namespace NewsApp.Domain
{
    public static class ValidaDomain
    {
        public static string ValidaCampoNulo(string nome, string valor)
        {
            if (string.IsNullOrEmpty(valor))
                throw new DomainException("O campo \"" + nome + "\" é obrigatório");
            return valor;
        }

        public static string ValidaString(string valor)
        {
            if (valor is string)
            {
                return valor;
            }
            throw new DomainException("Não é uma string");
        }

        public static DateTime ValidaData(DateTime Data)
        {
            if (Data.Day > 31 || Data.Month > 12 || Data.Year > 2030)
                throw new DomainException("Data Inválida");
            if (Data == null)
                throw new DomainException("Data Inválida");
            if (Data.Day == 1 && Data.Month == 1 && Data.Year == 1)
                throw new DomainException("Data Inválida");
            return Data;
        }

        public static string ValidaEmail(string email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");
            if (string.IsNullOrEmpty(email) || !rg.IsMatch(email))
                throw new DomainException("Email Inválido!");
            return email;
        }

        public static decimal ValidaDecimal(string nome, decimal number)
        {
            //Regex rg = new Regex(@"\d{1,12}\.\d\d");
            //if (!rg.IsMatch(number.ToString()))
            //{
            //    throw new Exception(nome + " não é um número decimal");
            //}
            if (number != null)
            {
                return number;
            }
            throw new DomainException(nome + " não é um número válido");
        }

        public static string ValidaCpfCnpj(string cpfcnpj)
        {
            Regex rg = new Regex(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})");
            if (string.IsNullOrEmpty(cpfcnpj) || !rg.IsMatch(cpfcnpj))
                throw new DomainException("CPF/CPNJ inválido!");
            return cpfcnpj;
        }

        public static string ValidaTelefone(string telefone)
        {
            /*
            Regex rg = new Regex(@"^\([1-9]{2}\)(?:[2-8]|9[1-9])[0-9]{3}\-[0-9]{4}$"); //(xx)xxxxx-xxxx
            if (string.IsNullOrEmpty(telefone) || !rg.IsMatch(telefone))
                throw new Exception("Telefone inválido!");
                */
            return telefone;
        }

        public static string ValidaTipoEndereco(string tipoEndereco)
        {
            /*
            if (tipoEndereco != "Principal" && tipoEndereco != "Secundário")
                throw new Exception("Tipo de endereço inválido");
            return tipoEndereco;
            */
            return tipoEndereco;
        }

        public static string ValidaCEP(string cep)
        {
            /*
            Regex rg = new Regex(@"^\d{5}-\d{3}$"); //xxxxx-xx
            if (string.IsNullOrEmpty(cep) || !rg.IsMatch(cep))
                throw new Exception("Formato de CEP inválido!");
                */
            return cep;
        }

        public static T ValidaEntidadeNull<T>(string nome, T obj)
        {
            if (obj != null)
                return obj;
            else
                throw new DomainException("Entidade " + nome + " inválida ");
        }

        public static int ValidaNumero(string nome, int number)
        {
            if (number != null)  //Consertar essa validação
            {
                return number;
            }
            else
            {
                throw new DomainException("Número" + nome + " inválido");
            }
        }
    }
}
