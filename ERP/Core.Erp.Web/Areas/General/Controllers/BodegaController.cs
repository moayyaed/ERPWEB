using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.General;
using Core.Erp.Info.General;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.General.Controllers
{
    [SessionTimeout]
    public class BodegaController : Controller
    {
        #region Variables
        tb_bodega_Bus bus_bodega = new tb_bodega_Bus();
        tb_sucursal_Bus bus_sucursal = new tb_sucursal_Bus();
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();
        tb_bodega_List Lista_Bodega = new tb_bodega_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

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

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Sucursal", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            tb_bodega_Info model = new tb_bodega_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdSucursal = Convert.ToInt32(SessionFixed.IdSucursal)
            };

            var lst = bus_bodega.get_list(model.IdEmpresa, model.IdSucursal, true);
            Lista_Bodega.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_bodega(int IdSucursal = 0, bool Nuevo=false)
        {
            ViewBag.IdSucursal = IdSucursal;
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Bodega.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_bodega", model);
        }

        private void cargar_combos(int IdEmpresa)
        {
            var lst_sucursal = bus_sucursal.get_list(IdEmpresa, false);
            ViewBag.lst_sucursal = lst_sucursal;

            var lst_cuentas = bus_plancta.get_list(IdEmpresa, false, true);
            ViewBag.lst_cuentas = lst_cuentas;

        }

        #endregion

        #region Acciones
        public ActionResult Nuevo(int  IdEmpresa = 0 , int IdSucursal = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Sucursal", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            tb_bodega_Info model = new tb_bodega_Info {
                IdEmpresa = IdEmpresa,
                IdSucursal = IdSucursal
            };
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            cargar_combos(IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(tb_bodega_Info model)
        {            
            if (!bus_bodega.guardarDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdSucursal = model.IdSucursal;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal=model.IdSucursal, IdBodega=model.IdBodega, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdSucursal = 0, int IdBodega = 0, bool Exito=false)
        {
            tb_bodega_Info model = bus_bodega.get_info(IdEmpresa, IdSucursal, IdBodega);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Sucursal", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            cargar_combos(IdEmpresa);
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , int IdSucursal = 0, int IdBodega = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Sucursal", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            tb_bodega_Info model = bus_bodega.get_info(IdEmpresa, IdSucursal, IdBodega);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdSucursal = IdSucursal });
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(tb_bodega_Info model)
        {
            if (!bus_bodega.modificarDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdSucursal = model.IdSucursal;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal, IdBodega = model.IdBodega, Exito = true });
        }

        public ActionResult Anular(int  IdEmpresa = 0 , int IdSucursal = 0, int IdBodega = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "General", "Sucursal", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            tb_bodega_Info model = bus_bodega.get_info(IdEmpresa, IdSucursal, IdBodega);
            if (model == null)
                return RedirectToAction("Index", new { IdEmpresa = IdEmpresa, IdSucursal = IdSucursal });
            ViewBag.IdEmpresa = IdEmpresa;
            ViewBag.IdSucursal = IdSucursal;
            cargar_combos(IdEmpresa);
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(tb_bodega_Info model)
        {
            if (!bus_bodega.anularDB(model))
            {
                ViewBag.IdEmpresa = model.IdEmpresa;
                ViewBag.IdSucursal = model.IdSucursal;
                cargar_combos(model.IdEmpresa);
                return View(model);
            }

            return RedirectToAction("Index", new { IdEmpresa = model.IdEmpresa, IdSucursal = model.IdSucursal });
        }

        #endregion
    }
    public class tb_bodega_List
    {
        string Variable = "tb_bodega_Info";
        public List<tb_bodega_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<tb_bodega_Info> list = new List<tb_bodega_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<tb_bodega_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<tb_bodega_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

}