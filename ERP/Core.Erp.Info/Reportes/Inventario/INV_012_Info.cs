using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Inventario
{
    public class INV_012_Info
    {
        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdMovi_inven_tipo { get; set; }
        public decimal IdNumMovi { get; set; }
        public int Secuencia { get; set; }
        public int IdBodega { get; set; }
        public decimal IdProducto { get; set; }
        public string Su_Descripcion { get; set; }
        public string bo_Descripcion { get; set; }
        public string pr_codigo { get; set; }
        public string pr_descripcion { get; set; }
        public string tm_descripcion { get; set; }
        public System.DateTime cm_fecha { get; set; }
        public string cm_observacion { get; set; }
        public string NomUnidad { get; set; }
        public double dm_cantidad_sinConversion { get; set; }
        public Nullable<double> mv_costo_sinConversion { get; set; }
        public Nullable<double> CostoTotal { get; set; }
        public string cm_tipo_movi { get; set; }
        public string IdEstadoAproba { get; set; }
        public string CodMoviInven { get; set; }
        public string MotivoCabecera { get; set; }
        public string MotivoDetalle { get; set; }
        public string cc_Descripcion { get; set; }
    }
}
