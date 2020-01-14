using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace JNogueira.Bufunfa.Web.Controllers
{
    /// <summary>
    /// Classe ancestral para todos os controles
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected readonly BackendProxy _proxy;

        protected BaseController(BackendProxy proxy)
        {
            _proxy = proxy;
        }
    }
}
