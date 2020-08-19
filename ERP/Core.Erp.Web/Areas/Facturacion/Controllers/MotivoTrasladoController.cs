using Core.Erp.Bus.Facturacion;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.Facturacion;
using Core.Erp.Info.SeguridadAcceso;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class MotivoTrasladoController : Controller
    {
        #region Variables
        fa_MotivoTraslado_List Lista_MotivoTraslado = new fa_MotivoTraslado_List();
        fa_MotivoTraslado_Bus bus_motivo = new fa_MotivoTraslado_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
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
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "MotivoTraslado", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            fa_MotivoTraslado_Info model = new fa_MotivoTraslado_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_motivo.get_list(model.IdEmpresa, true);
            Lista_MotivoTraslado.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }
        public ActionResult GridViewPartial_Motivo_traslado(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_motivo.get_list(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_MotivoTraslado.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_Motivo_traslado", model);
        }

        #endregion
        #region Acciones
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "MotivoTraslado", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion

            fa_MotivoTraslado_Info model = new fa_MotivoTraslado_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(fa_MotivoTraslado_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_motivo.GuardarDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMotivoTraslado = model.IdMotivoTraslado, Exito = true });

        }
        public ActionResult Consultar(int IdEmpresa = 0, int IdMotivoTraslado = 0, bool Exito=false)
        {
            fa_MotivoTraslado_Info model = bus_motivo.GetInfo(IdEmpresa, IdMotivoTraslado);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "MotivoTraslado", "Index");
            if (model.Estado == false)
            {
                info.Modificar = false;
                info.Anular = false;
            }
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            if (Exito)
                ViewBag.MensajeSuccess = MensajeSuccess;

            return View(model);
        }
        public ActionResult Modificar(int IdEmpresa = 0,int IdMotivoTraslado = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "MotivoTraslado", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            fa_MotivoTraslado_Info model = bus_motivo.GetInfo(IdEmpresa, IdMotivoTraslado);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(fa_MotivoTraslado_Info model)
        {
            model.IdUsuarioModificacion = SessionFixed.IdUsuario;
            if (!bus_motivo.ModificarDB(model))

            {
                return View(model);

            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdMotivoTraslado = model.IdMotivoTraslado, Exito = true });

        }
        public ActionResult Anular(int IdEmpresa = 0, int IdMotivoTraslado = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Facturacion", "MotivoTraslado", "Index");
            if (!info.Anular)
                return RedirectToAction("Index");
            #endregion

            fa_MotivoTraslado_Info model = bus_motivo.GetInfo(IdEmpresa, IdMotivoTraslado);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(fa_MotivoTraslado_Info model)
        {
            model.IdUsuarioAnulacion = SessionFixed.IdUsuario;
            if (!bus_motivo.AnularDB(model))
            {
                return View(model);

            }
            return RedirectToAction("Index");

        }
#endregion
    }

    public class fa_MotivoTraslado_List
    {
        string Variable = "fa_MotivoTraslado_Info";
        public List<fa_MotivoTraslado_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_MotivoTraslado_Info> list = new List<fa_MotivoTraslado_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_MotivoTraslado_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_MotivoTraslado_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}