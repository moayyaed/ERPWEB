using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.ActivoFijo
{
    public class Af_Parametros_Info
    {
        public int IdEmpresa { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El campo tipo de comprobante es obligatorio")]
        public int IdTipoCbte { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El campo tipo de comprobante para mejora es obligatorio")]
        public int IdTipoCbteMejora { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El campo tipo de comprobante para baja es obligatorio")]
        public int IdTipoCbteBaja { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El campo tipo de comprobante para venta es obligatorio")]
        public int IdTipoCbteVenta { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "El campo tipo de comprobante para retiro es obligatorio")]
        public int IdTipoCbteRetiro { get; set; }
        [Required(ErrorMessage = "El campo dias de transacción a futuro es obligatorio")]
        public int DiasTransaccionesAFuturo { get; set; }
        public bool ContabilizaDepreciacionPorActivo { get; set; }
    }
}
