using Core.Erp.Bus.ActivoFijo;
using Core.Erp.Info.ActivoFijo;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.ActivoFijo.Controllers
{
    public class ModeloController : Controller
    {
        #region Index
        Af_Modelo_Bus bus_modelo = new Af_Modelo_Bus();
        public ActionResult Index()
        {
            var model = new Af_Modelo_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_modelo_af()
        {
            int IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            var model = bus_modelo.GetList(IdEmpresa, true);
            return PartialView("_GridViewPartial_modelo_af", model);
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            Af_Modelo_Info model = new Af_Modelo_Info();
            model.IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa);
            return View(model);
        }
        [HttpPost]
        public ActionResult Nuevo(Af_Modelo_Info model)
        {
            model.IdUsuarioCreacion = SessionFixed.IdUsuario;
            if (!bus_modelo.GuardarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Modificar(int IdEmpresa=0, int IdModelo=0)
        {
            Af_Modelo_Info model = bus_modelo.GetInfo(IdEmpresa, IdModelo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Modificar(Af_Modelo_Info model)
        {
            if (!bus_modelo.ModificarDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdModelo = 0)
        {
            Af_Modelo_Info model = bus_modelo.GetInfo(IdEmpresa, IdModelo);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);
        }

        [HttpPost]
        public ActionResult Anular(Af_Modelo_Info model)
        {
            if (!bus_modelo.AnularDB(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }

    public class Af_Modelo_List
    {
        string Variable = "Af_Modelo_Info";
        public List<Af_Modelo_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<Af_Modelo_Info> list = new List<Af_Modelo_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<Af_Modelo_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<Af_Modelo_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}