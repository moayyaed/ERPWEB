using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.General;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class TipoMovimientoController : Controller
    {
        #region Variables

        in_movi_inven_tipo_Bus bus_tipo_movimiento = new in_movi_inven_tipo_Bus();
        ct_cbtecble_tipo_Bus bus_tipo_comprobante = new ct_cbtecble_tipo_Bus();
        in_Catalogo_Bus bus_catalogo = new in_Catalogo_Bus();
        in_movi_inven_tipo_List Lista_TipoMovimiento = new in_movi_inven_tipo_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        #endregion
        #region Index/Metodos

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "TipoMovimiento", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            in_movi_inven_tipo_Info model = new in_movi_inven_tipo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_tipo_movimiento.get_list(model.IdEmpresa,"", true);
            Lista_TipoMovimiento.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_tipo_movimiento(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_TipoMovimiento.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_tipo_movimiento", model);
        }

        private void cargar_combos(in_movi_inven_tipo_Info model)
        {
            Dictionary<string, string> lst_signo = new Dictionary<string, string>();
            lst_signo.Add("+", "+");
            lst_signo.Add("-", "-");
            ViewBag.lst_signo = lst_signo;
            
            var lst_tipo_comprobante = bus_tipo_comprobante.get_list(model.IdEmpresa, false);
            ViewBag.lst_tipo_comprobante = lst_tipo_comprobante;

            var lst_CatalogoAprobacion = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoInventario.EST_APROB),false);
            ViewBag.lst_CatalogoAprobacion = lst_CatalogoAprobacion;
        }
        #endregion
        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "TipoMovimiento", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            in_movi_inven_tipo_Info model = new in_movi_inven_tipo_Info
            {
                IdEmpresa = IdEmpresa
            };
            cargar_combos(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_movi_inven_tipo_Info model)
        {
            if (!bus_tipo_movimiento.guardarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            /*
            model.lst_tipo_mov_x_bodega.ForEach(q => { q.IdEmpresa = model.IdEmpresa; q.IdMovi_inven_tipo = model.IdMovi_inven_tipo; });
            bus_tipo_movimiento_x_bodega.guardarDB(model.lst_tipo_mov_x_bodega.Where(q => q.seleccionado == true).ToList());
            */
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMovi_inven_tipo = model.IdMovi_inven_tipo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdMovi_inven_tipo = 0, bool Exito=false)
        {
            in_movi_inven_tipo_Info model = bus_tipo_movimiento.get_info(IdEmpresa, IdMovi_inven_tipo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "TipoMovimiento", "Index");
            if (model.Estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion
            cargar_combos(model);
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0 , int IdMovi_inven_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "TipoMovimiento", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            in_movi_inven_tipo_Info model = bus_tipo_movimiento.get_info(IdEmpresa,IdMovi_inven_tipo);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(in_movi_inven_tipo_Info model)
        {
            if (!bus_tipo_movimiento.modificarDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            /*
            bus_tipo_movimiento_x_bodega.eliminarDB(model.IdEmpresa, model.IdMovi_inven_tipo);
            model.lst_tipo_mov_x_bodega.ForEach(q => { q.IdEmpresa = model.IdEmpresa; q.IdMovi_inven_tipo = model.IdMovi_inven_tipo; });
            bus_tipo_movimiento_x_bodega.guardarDB(model.lst_tipo_mov_x_bodega.Where(q=>q.seleccionado == true).ToList());
            */
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMovi_inven_tipo = model.IdMovi_inven_tipo, Exito = true });
        }

        public ActionResult Anular(int IdEmpresa = 0 , int IdMovi_inven_tipo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "TipoMovimiento", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            in_movi_inven_tipo_Info model = bus_tipo_movimiento.get_info(IdEmpresa, IdMovi_inven_tipo);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(in_movi_inven_tipo_Info model)
        {
            if (!bus_tipo_movimiento.anularDB(model))
            {
                cargar_combos(model);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class in_movi_inven_tipo_List
    {
        string Variable = "in_movi_inven_tipo_Info";
        public List<in_movi_inven_tipo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_movi_inven_tipo_Info> list = new List<in_movi_inven_tipo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_movi_inven_tipo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_movi_inven_tipo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}