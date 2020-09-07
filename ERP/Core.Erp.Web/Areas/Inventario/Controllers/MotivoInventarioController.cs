using Core.Erp.Bus.Contabilidad;
using Core.Erp.Bus.Inventario;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Info.Helps;
using Core.Erp.Info.Inventario;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Inventario.Controllers
{
    [SessionTimeout]
    public class MotivoInventarioController : Controller
    {
        ct_plancta_Bus bus_plancta = new ct_plancta_Bus();

        #region Index /  Metodos

        in_Motivo_Inven_Bus bus_motivo = new Bus.Inventario.in_Motivo_Inven_Bus();
        in_Motivo_Inven_List Lista_MotivoInv = new in_Motivo_Inven_List();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";

        public ActionResult Index()
        {
            #region Validar Session
            if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                return RedirectToAction("Login", new { Area = "", Controller = "Account" });
            SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
            SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
            #endregion

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "MotivoInventario", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            in_Motivo_Inven_Info model = new in_Motivo_Inven_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
            };

            var lst = bus_motivo.get_list(model.IdEmpresa, true);
            Lista_MotivoInv.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_motivoinven(bool Nuevo = false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_MotivoInv.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_motivoinven", model);
        }

        private void cargar_combos()
        {
            in_Catalogo_Bus bus_catalogo = new in_Catalogo_Bus();
            var lst_tipo = bus_catalogo.get_list(Convert.ToInt32(cl_enumeradores.eTipoCatalogoInventario.ING_EGR), false);
            ViewBag.lst_tipos = lst_tipo;
        }
        #endregion

        #region Metodos ComboBox bajo demanda
        public ActionResult CmbCtaCble()
        {
            in_Motivo_Inven_Info model = new in_Motivo_Inven_Info();
            return PartialView("_CmbCtaCble", model);
        }
        public List<ct_plancta_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_plancta.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), false);
        }
        public ct_plancta_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_plancta.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa));
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo(int IdEmpresa = 0 )
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "MotivoInventario", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            in_Motivo_Inven_Info model = new in_Motivo_Inven_Info
            {
                IdEmpresa = IdEmpresa,
                Genera_Movi_Inven_bool = true
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(in_Motivo_Inven_Info model)
        {
            if (!bus_motivo.guardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMotivo_Inv = model.IdMotivo_Inv, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdMotivo_Inv = 0, bool Exito=false)
        {
            in_Motivo_Inven_Info model = bus_motivo.get_info(IdEmpresa, IdMotivo_Inv);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "MotivoInventario", "Index");
            if (model.estado == "I")
            {
                info.Modificar = false;
                info.Anular = false;
            }
            model.Nuevo = (info.Nuevo == true ? 1 : 0);
            model.Modificar = (info.Modificar == true ? 1 : 0);
            model.Anular = (info.Anular == true ? 1 : 0);
            #endregion
            cargar_combos();
            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0 , int IdMotivo_Inv = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "MotivoInventario", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion
            in_Motivo_Inven_Info model = bus_motivo.get_info(IdEmpresa, IdMotivo_Inv);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(in_Motivo_Inven_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario.ToString();
            if (!bus_motivo.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMotivo_Inv = model.IdMotivo_Inv, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0 , int IdMotivo_Inv = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Inventario", "MotivoInventario", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            in_Motivo_Inven_Info model = bus_motivo.get_info(IdEmpresa, IdMotivo_Inv);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(in_Motivo_Inven_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario.ToString();
            if (!bus_motivo.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class in_Motivo_Inven_List
    {
        string Variable = "in_Motivo_Inven_Info";
        public List<in_Motivo_Inven_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<in_Motivo_Inven_Info> list = new List<in_Motivo_Inven_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<in_Motivo_Inven_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<in_Motivo_Inven_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}