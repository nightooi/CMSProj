namespace CMSProj.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using CMSProj.Model;

    using Microsoft.AspNetCore.Mvc;

    public class ContactController : Controller
    {
        private readonly IContactManager _mgr;
        public ContactController(IContactManager mgr) { _mgr = mgr; }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactFormVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View("ContactAdmin", vm);
            var res = await _mgr.CreateAsync(vm, ct);
            return RedirectToAction(nameof(Thanks), new { id = res.Id, name = res.Name, pos = res.Position });
        }

        [HttpGet]
        public IActionResult Thanks(Guid id, string name, int pos)
        {
            ViewBag.Id = id;
            ViewBag.Name = name;
            ViewBag.Position = pos;
            return View();
        }
        [HttpGet]
        public IActionResult ContactAdmin() => PartialView("ContactAdmin", new ContactFormVm());
    }
}
