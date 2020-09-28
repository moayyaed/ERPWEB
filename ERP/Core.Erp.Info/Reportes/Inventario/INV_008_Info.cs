using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Inventario
{
    public class INV_008_Info
    {
        public string ca_Categoria { get; set; }

        public int IdEmpresa { get; set; }
        public int IdSucursal { get; set; }
        public int IdMovi_inven_tipo { get; set; }
        public decimal IdNumMovi { get; set; }
        public int Secuencia { get; set; }
        public string Su_Descripcion { get; set; }
        public string bo_Descripcion { get; set; }
        public string cm_tipo_movi { get; set; }
        public string tm_descripcion { get; set; }
        public string Desc_mov_inv { get; set; }
        public string pe_nombreCompleto { get; set; }
        public string pr_codigo { get; set; }
        public string pr_descripcion { get; set; }
        public string IdEstadoAproba { get; set; }
        public double dm_cantidad { get; set; }
        public double mv_costo { get; set; }
        public Nullable<decimal> IdOrdenCompra { get; set; }
        public string IdCentroCosto { get; set; }
        public Nullable<int> IdMotivo_Inv { get; set; }
        public System.DateTime cm_fecha { get; set; }
        public string Estado { get; set; }
        public int IdBodega { get; set; }
        public decimal IdProducto { get; set; }
        public string cc_Descripcion { get; set; }
        public string tp_descripcion { get; set; }
        public string IdCategoria { get; set; }
        public int IdLinea { get; set; }
        public int IdSubGrupo { get; set; }
        public string nom_linea { get; set; }
        public string nom_grupo { get; set; }
        public string nom_subgrupo { get; set; }
        public string Tipo { get; set; }
        public int IdGrupo { get; set; }
    }
}
