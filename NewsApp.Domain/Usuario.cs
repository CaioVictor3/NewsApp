namespace NewsApp.Domain
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string? Login { get; set; }
        public string? Nome { get; set; }
        public string? Senha { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public string? CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? TipoUsuario { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? Situacao { get; set; }

        public Usuario() { }

        public Usuario(string login, string nome, string email, string senha, string cpf, DateTime? dataNascimento, string endereco)
        {
            this.Login = login;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.CPF = cpf;
            this.DataNascimento = dataNascimento;
            this.Endereco = endereco;
            this.TipoUsuario = "Mobile";
            this.DataInclusao = DateTime.Now;
            this.Situacao = "Ativo";
        }
    }
}
