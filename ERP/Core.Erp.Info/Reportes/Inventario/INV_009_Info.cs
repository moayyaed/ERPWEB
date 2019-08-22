using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Erp.Info.Reportes.Inventario
{
    public class INV_009_Info
    {
        public int IdEmpresa { get; set; }
        public string IdUsuario { get; set; }
        public decimal IdProducto { get; set; }
        public int IdSucursal { get; set; }
        public int IdBodega { get; set; }
        public int IdCategoria { get; set; }
        public int IdLinea { get; set; }
        public int IdGrupo { get; set; }
        public int IdSubGrupo { get; set; }
        public string pr_codigo { get; set; }
        public string pr_descripcion { get; set; }
        public string IdUnidadMedida { get; set; }
        public string NomUnidadMedida { get; set; }
        public string NomCategoria { get; set; }
        public string NomLinea { get; set; }
        public string NomGrupo { get; set; }
        public string NomSubGrupo { get; set; }
        public double CantidadInicial { get; set; }
        public double CostoInicial { get; set; }
        public double CantidadIngreso { get; set; }
        public double CostoIngreso { get; set; }
        public double CantidadEgreso { get; set; }
        public double CostoEgreso { get; set; }
        public double CantidadFinal { get; set; }
        public double CostoFinal { get; set; }
        public string Su_Descripcion { get; set; }
        public string bo_Bodega { get; set; }
    }
}
