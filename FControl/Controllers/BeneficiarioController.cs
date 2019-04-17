using FControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FControl.Controllers
{
    public class BeneficiarioController : Controller
    {
        // GET: Beneficiario
        private FinancialControlEntities db = new FinancialControlEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            
            return View();
        }

        public ActionResult Guardar(Beneficiario beneficiario)
        {
            FinancialControlEntities db = new FinancialControlEntities();
            object fc = Session["UsuarioID"].ToString();
            beneficiario.UsuarioID = Convert.ToInt16(fc);
            db.Beneficiario.Add(beneficiario);
            db.SaveChanges();
            return View("Regsitrar", beneficiario);
        }
    }
}