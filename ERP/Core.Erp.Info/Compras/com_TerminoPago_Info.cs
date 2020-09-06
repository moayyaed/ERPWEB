using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Erp.Info.Compras
{
    public class com_TerminoPago_Info
    {
        public decimal IdTransaccionSession { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTerminoPago { get; set; }
        [Required(ErrorMessage = ("el campo descripción es obligatorio"))]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "el campo descripción debe tener mínimo 1 caracter y máximo 500")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = ("el campo días es obligatorio"))]
        public int Dias { get; set; }
        public string Estado { get; set; }
        public bool EstadoBool { get; set; }
        public string IdUsuario { get; set; }
        public string nom_pc { get; set; }
        public string ip { get; set; }
        public string IdUsuarioUltMod { get; set; }
        public Nullable<System.DateTime> FechaUltMod { get; set; }
        public string IdUsuarioUltAnu { get; set; }
        public Nullable<System.DateTime> Fecha_UltAnu { get; set; }
        public string MotiAnula { get; set; }


        public int Nuevo { get; set; }
        public int Modificar { get; set; }
        public int Anular { get; set; }
    }
}
