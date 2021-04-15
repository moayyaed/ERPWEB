using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Info.General
{
    public class tb_TarjetaCredito_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTarjeta { get; set; }
        [StringLength(500, MinimumLength = 1, ErrorMessage = "el campo tarjeta de credito debe tener mínimo 1 caracter y máximo 500")]
        [Required(ErrorMessage = "El campo religión es obligatorio")]
        public string NombreTarjeta { get; set; }
        public bool Estado { get; set; }
        public Nullable<int> IdBanco { get; set; }
        public string IdUsuario { get; set; }
        public Nullable<System.DateTime> Fecha_Transac { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> Fecha_UltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
    }
}
