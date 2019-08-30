using Core.Erp.Bus.Contabilidad;
using Core.Erp.Info.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Contabilidad.Controllers
{
    public class ClasificacionEBITController : Controller
    {
        #region Variables
        ct_ClasificacionEBIT_Bus bus_clasificacion = new ct_ClasificacionEBIT_Bus();

        #endregion
        #region Index
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ClasificacionEBIT()
        {
            List<ct_ClasificacionEBIT_Info> model = new List<ct_ClasificacionEBIT_Info>();
            model = bus_clasificacion.GetList();
            return PartialView("_GridViewPartial_ClasificacionEBIT", model);
        }
        #endregion

        #region Acciones

        public ActionResult Nuevo()
        {
            ct_ClasificacionEBIT_Info model = new ct_ClasificacionEBIT_Info();
            return View(model);
        }

        [HttpPost]
        public ActionResult Nuevo(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.GuardarBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Modificar(int IdClasificacionEBIT = 0)
        {
            ct_ClasificacionEBIT_Info model = bus_clasificacion.GetInfo(IdClasificacionEBIT);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Modificar(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.ModificarBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Anular(int IdClasificacionEBIT = 0)
        {
            ct_ClasificacionEBIT_Info model = bus_clasificacion.GetInfo(IdClasificacionEBIT);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
        [HttpPost]
        public ActionResult Anular(ct_ClasificacionEBIT_Info model)
        {
            if (!bus_clasificacion.AnularBD(model))
            {
                return View(model);
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}