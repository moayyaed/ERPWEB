using Core.Erp.Bus.Facturacion;
using Core.Erp.Info.Facturacion;
using Core.Erp.Web.Helps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Facturacion.Controllers
{
    public class ProbabilidadCobroController : Controller
    {
        #region Variables
        fa_ProbabilidadCobro_Bus bus_probabilidad = new fa_ProbabilidadCobro_Bus();
        fa_ProbabilidadCobroDet_Bus bus_probabilidad_det = new fa_ProbabilidadCobroDet_Bus();
        fa_ProbabilidadCobro_List Lista_ProbabilidadCobro = new fa_ProbabilidadCobro_List();
        fa_ProbabilidadCobroDet_List Lista_ProbabilidadCobroDet = new fa_ProbabilidadCobroDet_List();
        fa_ProbabilidadCobroDet_XCruzar_List Lista_Probabilidad_X_Cruzar = new fa_ProbabilidadCobroDet_XCruzar_List();
        fa_factura_Bus bus_factura = new fa_factura_Bus();
        string MensajeSuccess = "La transacción se ha realizado con éxito";
        string mensaje = string.Empty;
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

            fa_ProbabilidadCobro_Info model = new fa_ProbabilidadCobro_Info
            {
                IdEmpresa = string.IsNullOrEmpty(SessionFixed.IdEmpresa) ? 0 : Convert.ToInt32(SessionFixed.IdEmpresa),
                IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual)
            };

            var lst = bus_probabilidad.get_list(model.IdEmpresa, true);
            Lista_ProbabilidadCobro.set_list(lst, model.IdTransaccionSession);
            return View(model);
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ProbabilidadCobro()
        {
            try
            {
                SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

                List<fa_ProbabilidadCobro_Info> model = Lista_ProbabilidadCobro.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
                return PartialView("_GridViewPartial_ProbabilidadCobro", model);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Acciones
        public ActionResult Nuevo()
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion

                fa_ProbabilidadCobro_Info model = new fa_ProbabilidadCobro_Info
                {
                    IdEmpresa = Convert.ToInt32(SessionFixed.IdEmpresa),
                    IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual),
                };

                model.lst_detalle = new List<fa_ProbabilidadCobroDet_Info>();
                Lista_ProbabilidadCobroDet.set_list(model.lst_detalle, model.IdTransaccionSession);
                //Lista_Probabilidad_X_Cruzar.set_list(model.lst_detalle, model.IdTransaccionSession);

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult Nuevo(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                info.IdUsuarioCreacion = SessionFixed.IdUsuario;
                info.lst_detalle = Lista_ProbabilidadCobroDet.get_list(info.IdTransaccionSession);

                if (ModelState.IsValid)
                {
                    if (!bus_probabilidad.GuardarDB(info))
                    {
                        ViewBag.mensaje = "No se ha podido guardar el registro";
                        SessionFixed.IdTransaccionSessionActual = info.IdTransaccionSession.ToString();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Modificar", new { IdEmpresa=info.IdEmpresa, IdProbabilidad=info.IdProbabilidad, Exito = true });
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Modificar(int IdEmpresa=0, int IdProbabilidad=0, bool Exito = false)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion
                var model = bus_probabilidad.get_info(IdEmpresa, IdProbabilidad);
                model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
                model.lst_detalle = bus_probabilidad_det.get_list(IdEmpresa, IdProbabilidad);

                Lista_ProbabilidadCobroDet.set_list(model.lst_detalle, model.IdTransaccionSession);

                if (Exito)
                    ViewBag.MensajeSuccess = MensajeSuccess;

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult Modificar(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                info.IdUsuarioModificacion = SessionFixed.IdUsuario;
                info.lst_detalle = Lista_ProbabilidadCobroDet.get_list(info.IdTransaccionSession);

                if (ModelState.IsValid)
                {
                    if (!bus_probabilidad.ModificarDB(info))
                    {
                        ViewBag.mensaje = "No se ha podido guardar el registro";
                        SessionFixed.IdTransaccionSessionActual = info.IdTransaccionSession.ToString();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Modificar", new { IdEmpresa = info.IdEmpresa, IdProbabilidad = info.IdProbabilidad, Exito = true });
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Anular(int IdEmpresa = 0, int IdProbabilidad = 0)
        {
            try
            {
                #region Validar Session
                if (string.IsNullOrEmpty(SessionFixed.IdTransaccionSession))
                    return RedirectToAction("Login", new { Area = "", Controller = "Account" });
                SessionFixed.IdTransaccionSession = (Convert.ToDecimal(SessionFixed.IdTransaccionSession) + 1).ToString();
                SessionFixed.IdTransaccionSessionActual = SessionFixed.IdTransaccionSession;
                #endregion
                var model = bus_probabilidad.get_info(IdEmpresa, IdProbabilidad);
                model.IdTransaccionSession = Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual);
                model.lst_detalle = bus_probabilidad_det.get_list(IdEmpresa, IdProbabilidad);

                Lista_ProbabilidadCobroDet.set_list(model.lst_detalle, model.IdTransaccionSession);

                return View(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult Anular(fa_ProbabilidadCobro_Info info)
        {
            try
            {
                info.IdUsuarioAnulacion = SessionFixed.IdUsuario;
                info.lst_detalle = Lista_ProbabilidadCobroDet.get_list(info.IdTransaccionSession);

                if (ModelState.IsValid)
                {
                    if (!bus_probabilidad.AnularDB(info))
                    {
                        ViewBag.mensaje = "No se ha podido anular el registro";
                        SessionFixed.IdTransaccionSessionActual = info.IdTransaccionSession.ToString();
                        return View(info);
                    }
                    else
                        return RedirectToAction("Index");
                }
                else
                    return View(info);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Detalle
        [ValidateInput(false)]
        public ActionResult GridViewPartial_Facturas()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            var model = Lista_Probabilidad_X_Cruzar.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_Facturas", model);
        }

        public void AddFacturas(string IDs = "", int IdEmpresa = 0, int IdProbabilidad=0, decimal IdTransaccionSession = 0)
        {
            if (!string.IsNullOrEmpty(IDs))
            {
                string[] array = IDs.Split(',');
                var lst_x_cruzar = Lista_Probabilidad_X_Cruzar.get_list(IdTransaccionSession);
                var lst_det_probabilidad = Lista_ProbabilidadCobroDet.get_list(IdTransaccionSession);
                foreach (var item in array)
                {
                    var info = lst_x_cruzar.Where(q => q.IdString == item).FirstOrDefault();
                    if (info != null)
                    {
                        if (lst_det_probabilidad.Where(q => q.IdEmpresa == info.IdEmpresa && q.IdSucursal == info.IdSucursal && q.IdBodega == info.IdBodega && q.IdCbteVta == info.IdCbteVta).Count() == 0)
                        {
                            info.Secuencia = lst_det_probabilidad.Count == 0 ? 1 : lst_det_probabilidad.Max(q => q.Secuencia) + 1;
                            info.IdEmpresa = IdEmpresa;
                            info.IdProbabilidad = IdProbabilidad;
                            lst_det_probabilidad.Add(info);
                            bus_probabilidad_det.GuardarDB(info);
                        }
                    }                    
                }
                Lista_ProbabilidadCobroDet.set_list(lst_det_probabilidad, IdTransaccionSession);
            }
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial_ProbabilidadCobroDet()
        {
            SessionFixed.IdTransaccionSessionActual = Request.Params["TransaccionFixed"] != null ? Request.Params["TransaccionFixed"].ToString() : SessionFixed.IdTransaccionSessionActual;

            var model = Lista_ProbabilidadCobroDet.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ProbabilidadCobroDet", model);
        }
        public ActionResult EditingDelete(int Secuencia)
        {
            Lista_ProbabilidadCobroDet.DeleteRow(Secuencia, Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));
            var model = Lista_ProbabilidadCobroDet.get_list(Convert.ToDecimal(SessionFixed.IdTransaccionSessionActual));

            return PartialView("_GridViewPartial_ProbabilidadCobroDet", model);
        }
        #endregion

        #region Json
        public JsonResult GetFacturas(int IdEmpresa, decimal IdTransaccionSession)
        {
            var resultado = "";

            var list = bus_probabilidad_det.get_list_x_ingresar(IdEmpresa);
            var detalle_probabilidad = Lista_ProbabilidadCobroDet.get_list(IdTransaccionSession);
            var lst_det_x_ingresar = new List<fa_ProbabilidadCobroDet_Info>();
            var cont = 0;
            foreach (var item1 in list)
            {
                cont = 0;
                foreach (var item2 in detalle_probabilidad)
                {
                    if (item1.IdEmpresa == item2.IdEmpresa && item1.IdCbteVta == item2.IdCbteVta)
                    {
                        cont++;
                    }
                }

                if (cont==0)
                {
                    lst_det_x_ingresar.Add(item1);
                }
            }
            Lista_Probabilidad_X_Cruzar.set_list(lst_det_x_ingresar,IdTransaccionSession);

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }

    public class fa_ProbabilidadCobro_List
    {
        string Variable = "fa_ProbabilidadCobro_Info";
        public List<fa_ProbabilidadCobro_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_ProbabilidadCobro_Info> list = new List<fa_ProbabilidadCobro_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_ProbabilidadCobro_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_ProbabilidadCobro_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }

    public class fa_ProbabilidadCobroDet_List
    {
        string Variable = "fa_ProbabilidadCobroDet_Info";
        fa_ProbabilidadCobroDet_Bus bus_probabilidad_det = new fa_ProbabilidadCobroDet_Bus();
        public List<fa_ProbabilidadCobroDet_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_ProbabilidadCobroDet_Info> list = new List<fa_ProbabilidadCobroDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_ProbabilidadCobroDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_ProbabilidadCobroDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }

        public void DeleteRow(int Secuencia, decimal IdTransaccionSession)
        {
            List<fa_ProbabilidadCobroDet_Info> list = get_list(IdTransaccionSession);
            var info = list.Where(m => m.Secuencia == Secuencia).FirstOrDefault();
            
            if (bus_probabilidad_det.EliminarDB(info))
            {
                list.Remove(info);
            }
        }
    }

    public class fa_ProbabilidadCobroDet_XCruzar_List
    {
        string Variable = "fa_ProbabilidadCobroDet_XCruzar_Info";
        public List<fa_ProbabilidadCobroDet_Info> get_list(decimal IdTransaccionSession)
        {
            if (HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] == null)
            {
                List<fa_ProbabilidadCobroDet_Info> list = new List<fa_ProbabilidadCobroDet_Info>();

                HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
            }
            return (List<fa_ProbabilidadCobroDet_Info>)HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()];
        }

        public void set_list(List<fa_ProbabilidadCobroDet_Info> list, decimal IdTransaccionSession)
        {
            HttpContext.Current.Session[Variable + IdTransaccionSession.ToString()] = list;
        }
    }
}