using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("relatorios")]
    public class RelatorioController : BaseController
    {
        public RelatorioController(BackendProxy proxy)
            : base(proxy)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            var footer = "--footer-right \"Emitido em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\" --footer-left \"Página: [page] de [toPage]\" --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Poppins\"";

            return new ViewAsPdf("RelatorioTeste")
            {
                CustomSwitches = footer,
                PageOrientation = Orientation.Portrait,
                FileName = "relatorio_teste.pdf"
            };
        }
    }
}
