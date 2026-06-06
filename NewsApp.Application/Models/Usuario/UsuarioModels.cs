namespace NewsApp.Application.Models.Usuario
{
    public class LoginResponseModel
    {
        public string Nome { get; set; }
        public string Token { get; set; }
        public string TipoUsuario { get; set; }
        public int IdUsuario { get; set; }
    }

    public class UsuarioResponseModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
    }

    public class AlterarSenhaResponseModel { }
    public class EsqueceuSenhaResponseModel { }
    public class UsuarioListResponseModel { }
    public class EsqueceuSenhaRequestModel { }
    public class CriarUsuarioRequestModel { }
    public class AtualizarUsuarioRequestModel { }
    public class CriarUsuarioPacienteRequestModel { }
}
