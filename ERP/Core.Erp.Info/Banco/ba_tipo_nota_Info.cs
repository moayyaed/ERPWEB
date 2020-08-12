using System.ComponentModel.DataAnnotations;

namespace Core.Erp.Info.Banco
{
    public class ba_tipo_nota_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoNota { get; set; }
        [Required(ErrorMessage = ("el campo Tipo es obligatorio"))]
        public string Tipo { get; set; }
        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 50")]
        public string Descripcion { get; set; }
        public string IdCtaCble { get; set; }
        public string Estado { get; set; }
        public bool EstadoBool { get; set; }
    }
}
