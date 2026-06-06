using Proclin.NotificationPartner;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proclin.Models
{
    public abstract class BaseModel
    {
        public string UsuarioInclusao { get; set; }
        public string UsuarioAlteracao { get; set; }
        public string UsuarioExclusao { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public DateTime? DataExclusao { get; set; }

        [NotMapped]
        public ICollection<Notification> Notification { get; set; }

        [NotMapped]
        public bool Valid { get; set; }
        public BaseModel()
        {
            this.Valid = true;
        }

        public void AddNotification(Notification n)
        {
            if (this.Notification == null)
                this.Notification = new Collection<Notification>();
            else if (this.Notification.Contains(n))
                return;

            this.Notification.Add(n);

            this.Valid = false;
        }

        //  public abstract void Validar();

        public string Situacao { get; set; }

        public void SetUsuarioInclusao(string usuarioInclusao)
        {
            this.Situacao = "Ativo";
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
        }
        public void SetUsuarioAlteracao(string usuarioAlteracao)
        {
            this.UsuarioAlteracao = usuarioAlteracao;
            this.DataAlteracao = DateTime.Now;
        }
        public void SetUsuarioExclusao(string usuarioExclusao)
        {
            this.Situacao = "Excluido";
            this.UsuarioExclusao = usuarioExclusao;
            this.DataExclusao = DateTime.Now;
        }

    }
}
