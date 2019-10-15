using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Facturacion
{
    public class fa_ProbabilidadCobroDet_Info
    {
        public int IdEmpresa { get; set; }
        public int IdProbabilidad { get; set; }
        public int Secuencia { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public decimal IdCbteVta { get; set; }
        public string vt_tipoDoc { get; set; }

        #region Campos que no existen en la tabla
        public string vt_NumFactura { get; set; }
        public DateTime vt_fecha { get; set; }
        public int DiasVencido { get; set; }
        public double Saldo { get; set; }
        public decimal Total { get; set; }
        public string vt_Observacion { get; set; }
        public string IdString { get; set; }
        public string pe_nombreCompleto { get; set; }
        #endregion
    }
}
