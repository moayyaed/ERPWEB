using Core.Erp.Web.Helps;
using Core.Erp.Web.Reportes.Compra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Core.Erp.Web.Areas.Reportes.Controllers
{
    public class CompraReportesController : Controller
    {
        public ActionResult COMP_001(int IdEmpresa = 0, int IdSucursal = 0,  decimal IdOrdenCompra = 0)
        {
            COMP_001_Rpt model = new COMP_001_Rpt();
            model.p_IdEmpresa.Value = IdEmpresa;
            model.p_IdSucursal.Value = IdSucursal;
            model.p_IdOrdenCompra.Value = IdOrdenCompra;

            model.usuario = SessionFixed.IdUsuario;
            model.empresa = SessionFixed.NomEmpresa;
            return View(model);
        }

    }
}