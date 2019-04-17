using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FControl.Tests.SeleniumTest
{
    [TestFixture]
    class FControlTest
    {
        private IWebDriver navegador;

        [SetUp]
        public void AntesDeCadaTest()
        {
            navegador = new ChromeDriver();
        }
        [Test]
        public void PaginaIniciar()
        {
            navegador.Url = SeleniumContaste.URL_BASE;

       
            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

        }
        [Test]
        public void CrearCuentaEfectivo()
        {
            navegador.Url = SeleniumContaste.URL_BASE;


            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

            navegador.Url = SeleniumContaste.URL_BASE1;

            navegador.FindElement(By.Name("Nombre")).SendKeys("PruevaCuentaEfectivo");
            SelectElement oSelect = new SelectElement(navegador.FindElement(By.Id("Divisa")));
            oSelect.SelectByValue("2");
            navegador.FindElement(By.Name("SaldoInicial")).SendKeys("500");
            navegador.FindElement(By.Id("Concepto")).SendKeys("Rifas");

            IWebElement btnCrearCuenta = navegador.FindElement(By.Id("btnCrear"));
            btnCrearCuenta.Click();

        }
        [Test]
        public void CrearCuentaDebito()
        {
            navegador.Url = SeleniumContaste.URL_BASE;


            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

            navegador.Url = SeleniumContaste.URL_BASE1;

            IWebElement bdtDebito = navegador.FindElement(By.Id("btnCuentaDebito"));
            bdtDebito.Click();
           

            SelectElement oSelect = new SelectElement(navegador.FindElement(By.Name("EntidadEmisoraID")));
            oSelect.SelectByValue("3");

            navegador.FindElement(By.Id("NumeroTarjeta")).SendKeys("1234");

            navegador.FindElement(By.Name("Entidad")).SendKeys("BBVA");

            navegador.FindElement(By.Name("Nombre")).SendKeys("PruevaCuentaDebito");

            SelectElement oSelectD = new SelectElement(navegador.FindElement(By.Name("DivisaId")));
            oSelectD.SelectByValue("1");

            navegador.FindElement(By.Name("SaldoInicial")).SendKeys("500");
            navegador.FindElement(By.Id("Concepto")).SendKeys("Rifas");

            IWebElement btnCrearCuenta = navegador.FindElement(By.Id("btnCrear"));
            btnCrearCuenta.Click();
        }
        [Test]
        public void CrearCuentaCredito()
        {
            navegador.Url = SeleniumContaste.URL_BASE;


            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

            navegador.Url = SeleniumContaste.URL_BASE1;

            IWebElement bdtCredito = navegador.FindElement(By.Id("btnCuentaCredito"));
            bdtCredito.Click();

            

            SelectElement oSelect = new SelectElement(navegador.FindElement(By.Name("EntidadEmisoraID")));
            oSelect.SelectByValue("3");
            navegador.FindElement(By.Name("Entidad")).SendKeys("BBVA");

            navegador.FindElement(By.Id("NumeroTarjeta")).SendKeys("1234");


            SelectElement oSelectDP = new SelectElement(navegador.FindElement(By.Name("DiaPago")));
            oSelectDP.SelectByValue("24");

            SelectElement oSelectDC = new SelectElement(navegador.FindElement(By.Name("DiaCierre")));
            oSelectDC.SelectByValue("30");

            navegador.FindElement(By.Name("LimiteCredito")).SendKeys("2000");
            

            navegador.FindElement(By.Name("Nombre")).SendKeys("PruevaCuentaCredito");

            SelectElement oSelectD = new SelectElement(navegador.FindElement(By.Name("DivisaId")));
            oSelectD.SelectByValue("1");

            navegador.FindElement(By.Name("SaldoInicial")).SendKeys("500");
            navegador.FindElement(By.Id("Concepto")).SendKeys("Rifas");

            IWebElement btnCrearCuenta = navegador.FindElement(By.Id("btnCrear"));
            btnCrearCuenta.Click();
        }
        [Test]
        public void CrearTransaccion()
        {
            navegador.Url = SeleniumContaste.URL_BASE;

            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

            navegador.Url = SeleniumContaste.URL_BASE3;

            SelectElement oSelect = new SelectElement(navegador.FindElement(By.Name("BenefeciarioID")));
            oSelect.SelectByValue("2");

            SelectElement oSelectC = new SelectElement(navegador.FindElement(By.Id("CuentaID")));
            oSelectC.SelectByValue("1003");

            SelectElement oSelectD = new SelectElement(navegador.FindElement(By.Name("DivisaID")));
            oSelectD.SelectByValue("2");

            SelectElement oSelectT = new SelectElement(navegador.FindElement(By.Name("TipoTransaccion")));
            oSelectT.SelectByValue("2");

            navegador.FindElement(By.Name("Importe")).SendKeys("10");

            navegador.FindElement(By.Name("Concepto")).SendKeys("Pago de apuestas");

        }

        [Test]
        public void CrearTraspaso()
        {

            navegador.Url = SeleniumContaste.URL_BASE;

            navegador.FindElement(By.Name("Name")).SendKeys("fran");
            navegador.FindElement(By.Id("Password")).SendKeys("1");

            IWebElement bdtLogin = navegador.FindElement(By.Id("iniciar"));
            bdtLogin.Click();

            navegador.Url = SeleniumContaste.URL_BASE4;

            SelectElement oSelect = new SelectElement(navegador.FindElement(By.Name("CuentaOrigenID")));
            oSelect.SelectByValue("1002");

            SelectElement oSelect1 = new SelectElement(navegador.FindElement(By.Name("CuentaOrigenID")));
            oSelect1.SelectByValue("1003");


            SelectElement oSelectD = new SelectElement(navegador.FindElement(By.Name("DivisaId")));
            oSelectD.SelectByValue("2");

           

            navegador.FindElement(By.Name("Monto")).SendKeys("10");

            navegador.FindElement(By.Name("Concepto")).SendKeys("Pago de apuestas");
        }
        //[TearDown]
        //public void CerraSiFalla()
        //{
        //    navegador.Close();
        //}
    }
}

