using Core.Erp.Bus.Compras;
using Core.Erp.Bus.General;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Compras;
using Core.Erp.Info.General;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Compras.Controllers
{
    [SessionTimeout]
    public class CompradorController : Controller
    {
        #region Variables
        com_comprador_Bus bus_comprador = new com_comprador_Bus();
        seg_usuario_Bus bus_usuario = new seg_usuario_Bus();
        tb_persona_Bus bus_persona = new tb_persona_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        com_comprador_List Lista_Comprador = new com_comprador_List();

        #endregion
        #region Combo box bajo demanda
        public ActionResult CmbPersona_Compras()
        {
            SessionFixed.TipoPersona = Request.Params["IdTipoPersona"] != null ? Request.Params["IdTipoPersona"].ToString() : "PERSONA";
            decimal model = new decimal();
            return PartialView("_CmbPersona_Compras", model);
        }
        public List<tb_persona_Info> get_list_bajo_demanda(ListEditItemsRequestedByFilterConditionEventArgs args)
        {
            return bus_persona.get_list_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.TipoPersona);
        }
        public tb_persona_Info get_info_bajo_demanda(ListEditItemRequestedByValueEventArgs args)
        {
            return bus_persona.get_info_bajo_demanda(args, Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.TipoPersona);
        }

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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "Comprador", "Index");
            ViewBag.Nuevo = info.Nuevo;
            #endregion

            com_comprador_Info model = new com_comprador_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };

            var lst = bus_comprador.get_list(model.IdEmpresa, true);
            Lista_Comprador.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_comprador(bool Nuevo=false)
        {
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_Comprador.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_comprador", model);
        }

        private void cargar_combos()
        {
            var lst_usuario = bus_usuario.get_list(false);
            ViewBag.lst_usuario = lst_usuario;
            
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "Comprador", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            com_comprador_Info model = new com_comprador_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            cargar_combos();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(com_comprador_Info model)
        {
            model.IdUsuario = SessionFixed.IdUsuario;
            if (!bus_comprador.guardarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdComprador = model.IdComprador, Exito = true });
        }
        public ActionResult Consultar(int IdEmpresa = 0, decimal IdComprador = 0, bool Exito=false)
        {
            com_comprador_Info model = bus_comprador.get_info(IdEmpresa, IdComprador);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "Comprador", "Index");
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

            cargar_combos();
            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0, decimal IdComprador = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "Comprador", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            com_comprador_Info model = bus_comprador.get_info(IdEmpresa, IdComprador);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(com_comprador_Info model)
        {
            model.IdUsuarioUltMod = SessionFixed.IdUsuario;

            if (!bus_comprador.modificarDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdComprador=model.IdComprador, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, decimal IdComprador = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Compras", "Comprador", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion
            com_comprador_Info model = bus_comprador.get_info(IdEmpresa, IdComprador);
            if (model == null)
                return RedirectToAction("Index");
            cargar_combos();
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(com_comprador_Info model)
        {
            model.IdUsuarioUltAnu = SessionFixed.IdUsuario;

            if (!bus_comprador.anularDB(model))
            {
                cargar_combos();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        #endregion
    }

    public class com_comprador_List
    {
        string Variable = "com_comprador_Info";
        public List<com_comprador_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<com_comprador_Info> list = new List<com_comprador_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<com_comprador_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<com_comprador_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}