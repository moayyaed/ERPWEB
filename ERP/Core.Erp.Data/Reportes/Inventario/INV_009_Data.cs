using Core.Erp.Info.Reportes.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Erp.Data.Reportes.Base;
namespace Core.Erp.Data.Reportes.Inventario
{
    public class INV_009_Data
    {
        public List<INV_009_Info> GetList(int IdEmpresa, string IdUsuario, int IdSucursal, int IdBodega, int IdCategoria, int IdLinea, int IdGrupo, int IdSubgrupo, bool considerarSinAprobar, bool mostrarSinMovimiento, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                int IdSucursalIni = IdSucursal;
                int IdSucursalFin = IdSucursal == 0 ? 9999 : IdSucursal;

                int IdBodegaIni = IdBodega;
                int IdBodegaFin = IdBodega == 0 ? 9999 : IdBodega;


                int IdLineaIni = IdLinea;
                int IdLineaFin = IdLinea == 0 ? 9999 : IdLinea;


                int IdCategoriaIni = IdCategoria;
                int IdCategoriaFin = IdCategoria == 0 ? 9999 : IdCategoria;


                int IdGrupoIni = IdGrupo;
                int IdGrupoFin = IdGrupo == 0 ? 9999 : IdGrupo;


                int IdSubgrupoIni = IdSubgrupo;
                int IdSubgrupoFin = IdSubgrupo == 0 ? 9999 : IdSubgrupo;


                List<INV_009_Info> Lista;
                using (Entities_reportes Context = new Entities_reportes())
                {
                    Lista = Context.SPINV_009(IdEmpresa, IdUsuario, IdSucursalIni, IdSucursalFin, IdBodegaIni, IdBodegaFin, IdCategoriaIni, IdCategoriaFin, IdLineaIni, IdLineaFin, IdGrupoIni, IdGrupoFin, IdSubgrupoIni, IdSubgrupoFin, considerarSinAprobar, mostrarSinMovimiento, fecha_ini, fecha_fin).Select(q => new INV_009_Info
                    {
                        CantidadEgreso = q.CantidadEgreso,
                        CantidadFinal = q.CantidadFinal,
                        CantidadIngreso = q.CantidadIngreso,
                        CantidadInicial = q.CantidadInicial,
                        CostoEgreso = q.CostoEgreso,
                        CostoFinal = q.CostoFinal,
                        CostoIngreso = q.CostoIngreso,
                        CostoInicial = q.CostoInicial,
                        IdBodega = q.IdBodega,
                        IdCategoria = q.IdCategoria,
                        IdEmpresa = q.IdEmpresa,
                        IdGrupo = q.IdGrupo,
                        IdLinea = q.IdLinea,
                        IdProducto = q.IdProducto,
                        IdSucursal = q.IdSucursal,
                        IdUnidadMedida = q.IdUnidadMedida,
                        IdUsuario = q.IdUsuario,
                        NomCategoria = q.NomCategoria,
                        NomGrupo = q.NomGrupo,
                        NomLinea = q.NomLinea,
                        NomSubGrupo = q.NomSubGrupo,
                        NomUnidadMedida = q.NomUnidadMedida,
                        pr_codigo = q.pr_codigo,
                        pr_descripcion = q.pr_descripcion,
                        IdSubGrupo = q.IdSubGrupo



                    }).ToList();
                }
                return Lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
