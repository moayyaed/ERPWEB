using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Bus.Inventario;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.General;
using Core.Erp.Info.General;
using Core.Erp.Bus.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    public class AprobacionDespachoController : Controller
    {
        #region Variables
        in_Ing_Egr_Inven_Bus bus_inv = new in_Ing_Egr_Inven_Bus();
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        tb_ColaImpresionDirecta_Bus bus_ColaImpresion = new tb_ColaImpresionDirecta_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        in_Ing_Egr_Inven_AprobacionDespacho_List Lista_AprobacionDespacho = new in_Ing_Egr_Inven_AprobacionDespacho_List();
        #endregion
        #region Index
        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            cl_filtros_Info model = new cl_filtros_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal),
                IdBodega = 0,
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                fecha_ini = DateTime.Now.Date.AddMonths(-1),
                fecha_fin = DateTime.Now.Date,
            };
            CargarCombosConsulta(model.IdEmpresa);
            var lst = bus_inv.GetListPorDespachar(model.IdEmpresa, model.IdSucursal, model.IdBodega);
            Lista_AprobacionDespacho.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(cl_filtros_Info model)
        {
            SessionFixed.IdTransaccionSessionActual = model.IdTransaccionSession.ToString();
            var lst = bus_inv.GetListPorDespachar(model.IdEmpresa, model.IdSucursal, model.IdBodega);
            Lista_AprobacionDespacho.set_list(lst, model.IdTransaccionSession);
            CargarCombosConsulta(model.IdEmpresa);
            return View(model);
        }

        private void CargarCombosConsulta(int IdEmpresa)
        {
            tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
            var lst_sucursal = bus_sucursal.GetList(IdEmpresa, SessionFixed.IdUsuario, true);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_bodega = bus_bodega.get_list(IdEmpresa, false);
            ViewBag.lst_bodega = lst_bodega;
        }
        #endregion
        #region INV
        [ValidateInput(false)]
        public ActionResult GridViewPartial_aprobacion_despacho()
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //ViewBag.IdEmpresa = IdEmpresa;
            //ViewBag.IdSucursal = IdSucursal;
            //ViewBag.IdBodega = IdBodega;
            //List<in_Ing_Egr_Inven_Info> model = bus_inv.GetListPorDespachar(IdEmpresa, IdSucursal, IdBodega);
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_AprobacionDespacho.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_aprobacion_despacho", model);
        }
        #endregion
        #region Json
        public JsonResult DespacharMovimiento(string SecuencialID = "")
        {
            string resultado = string.Empty;
            int IdEmpresa = Convert.ToInt32(SecuencialID.Substring(0, 2));
            int IdSucursal = Convert.ToInt32(SecuencialID.Substring(2, 2));
            int IdMovi_inven_tipo = Convert.ToInt32(SecuencialID.Substring(4, 2));
            int IdNumMovi = Convert.ToInt32(SecuencialID.Substring(6, 8));

            seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
            var usuario = bus_usuario.get_info(SessionFixed.IdUsuario);

            var model = bus_inv.get_info(IdEmpresa, IdSucursal, IdMovi_inven_tipo, IdNumMovi);
            model.IdUsuarioDespacho = SessionFixed.IdUsuario;

            if (model != null)
            {
                if (bus_inv.DespacharDB(model))
                {
                    resultado = "Despacho exitoso";

                    bus_ColaImpresion.GuardarDB(new tb_ColaImpresionDirecta_Info
                    {
                        IdEmpresa = IdEmpresa,
                        CodReporte = "INV_020",
                        IPImpresora = usuario.IPImpresora,
                        IPUsuario = usuario.IPMaquina,
                        NombreEmpresa = SessionFixed.NomEmpresa,
                        Usuario = SessionFixed.IdUsuario,
                        //Nunca enviar IdEmpresa en Parametros
                        Parametros = IdSucursal + "," + IdMovi_inven_tipo + "," + IdNumMovi,
                        NumCopias = 2
                    });
                }                    
            }
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class in_Ing_Egr_Inven_AprobacionDespacho_List
    {
        string variable = "in_Ing_Egr_Inven_AprobacionDespacho_Info";
        public List<in_Ing_Egr_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Ing_Egr_Inven_Info> list = new List<in_Ing_Egr_Inven_Info>();

                HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Ing_Egr_Inven_Info>)HttpContext.Current.Session[variable + IdTransaccionSession.ToString()];
        }
        public void set_list(List<in_Ing_Egr_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[variable + IdTransaccionSession.ToString()] = list;
        }
    }
}