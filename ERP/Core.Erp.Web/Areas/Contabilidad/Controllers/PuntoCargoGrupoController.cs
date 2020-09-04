using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Erp.Bus.Contabilidad;
using Core.Erp.Info.Contabilidad;
using Core.Erp.Web.Helps;
using Core.Erp.Bus.SeguridadAcceso;
using Core.Erp.Info.SeguridadAcceso;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class PuntoCargoGrupoController : Controller
    {
        #region Index
        ct_punto_cargo_grupo_Bus bus_punto = new ct_punto_cargo_grupo_Bus();
        seg_Menu_x_Empresa_x_Usuario_Bus bus_permisos = new seg_Menu_x_Empresa_x_Usuario_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        ct_punto_cargo_grupo_List Lista_PuntoCargoGrupo = new ct_punto_cargo_grupo_List();

        public ActionResult Index()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            ViewBag.Nuevo = info.Nuevo;
            ViewBag.Modificar = info.Modificar;
            ViewBag.Anular = info.Anular;
            #endregion

            ct_punto_cargo_grupo_Info model = new ct_punto_cargo_grupo_Info
            {
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSession),
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
            };

            var lst = bus_punto.GetList(model.IdEmpresa, true);
            Lista_PuntoCargoGrupo.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_punto_cargo_grupo(bool Nuevo=false)
        {
            //int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            //var model = bus_punto.GetList(IdEmpresa, true);
            ViewBag.Nuevo = Nuevo;
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;
            var model = Lista_PuntoCargoGrupo.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            return PartialView("_GridViewPartial_punto_cargo_grupo", model);
        }

        #endregion
        #region Acciones
        
        public ActionResult Nuevo()
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Nuevo)
                return RedirectToAction("Index");
            #endregion
            ct_punto_cargo_grupo_Info model = new ct_punto_cargo_grupo_Info
            {
                IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa)
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(ct_punto_cargo_grupo_Info model)
        {
            model.IdUsuarioCreacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto.GuardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdPunto_cargo_grupo = model.IdPunto_cargo_grupo, Exito = true });
        }

        public ActionResult Consultar(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0, bool Exito = false)
        {
            ct_punto_cargo_grupo_Info model = bus_punto.GetInfo(IdEmpresa, IdPunto_cargo_grupo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (model.estado == false)
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

            return View(model);
        }

        public ActionResult Modificar(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0)
        {
            ct_punto_cargo_grupo_Info model = bus_punto.GetInfo(IdEmpresa, IdPunto_cargo_grupo);
            if (model == null)
                return RedirectToAction("Index");

            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_punto_cargo_grupo_Info model)
        {
            model.IdUsuarioModificacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto.ModificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Consultar", new { IdEmpresa = model.IdEmpresa, IdPunto_cargo_grupo = model.IdPunto_cargo_grupo, Exito = true });
        }
        public ActionResult Anular(int IdEmpresa = 0, int IdPunto_cargo_grupo = 0)
        {
            #region Permisos
            seg_Menu_x_Empresa_x_Usuario_Info info = bus_permisos.get_list_menu_accion(Convert.ToInt32(SessionFixed.IdEmpresa), SessionFixed.IdUsuario, "Contabilidad", "PuntoCargoGrupo", "Index");
            if (!info.Modificar)
                return RedirectToAction("Index");
            #endregion

            ct_punto_cargo_grupo_Info model = bus_punto.GetInfo(IdEmpresa, IdPunto_cargo_grupo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_punto_cargo_grupo_Info model)
        {
            model.IdUsuarioAnulacion = Convert.ToString(SessionFixed.IdUsuario);
            if (!bus_punto.AnularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion

    }

    public class ct_punto_cargo_grupo_List
    {
        string Variable = "ct_punto_cargo_grupo_Info";
        public List<ct_punto_cargo_grupo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession] == null)
            {
                List<ct_punto_cargo_grupo_Info> list = new List<ct_punto_cargo_grupo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
            }
            return (List<ct_punto_cargo_grupo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession];
        }

        public void set_list(List<ct_punto_cargo_grupo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession] = list;
        }
    }
}
