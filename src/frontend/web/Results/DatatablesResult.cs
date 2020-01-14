using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Results
{
    /// <summary>
    /// ActionResults que alimentarão componentes Datatables.net
    /// </summary>
    public class DatatablesResult : IActionResult
    {
        private readonly int _draw;
        private readonly IEnumerable _itens;
        private readonly int _totalRegistros;

        public DatatablesResult(int draw, int totalRegistros, IEnumerable registros)
        {
            _draw = draw;
            _itens = registros;
            _totalRegistros = totalRegistros;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(new
            {
                _draw,
                recordsTotal    = _totalRegistros,
                recordsFiltered = _totalRegistros,
                data            = _itens
            });

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
