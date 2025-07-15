using Microsoft.AspNetCore.Mvc;

namespace SignalRWebUI.ViewComponents.UILayoutComponents
{
    public class _UILayoutHeaderComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }

    }
}
