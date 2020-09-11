using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Facturacion
{
    public class fa_cliente_tipo_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int Idtipo_cliente { get; set; }
        [StringLength(10, MinimumLength = 0, ErrorMessage = "el campo código debe tener máximo 10")]
        public string Cod_cliente_tipo { get; set; }
        [Required(ErrorMessage = "El campo descripción es obligatorio")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 50")]
        public string Descripcion_tip_cliente { get; set; }
        public string IdCtaCble_CXC_Cred { get; set; }
        public string IdCtaCble_Anticipo { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string MotivoAnula { get; set; }
        public string Estado { get; set; }
        public bool EstadoBool { get; set; }


        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
    }
}
