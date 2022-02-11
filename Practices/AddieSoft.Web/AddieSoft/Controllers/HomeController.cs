using AddieSoft.Models;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AddieSoft.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILifetimeScope _scope;

        public HomeController(ILogger<HomeController> logger, ILifetimeScope scope)
        {
            _scope = scope;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = _scope.Resolve<CreateModel>();
            model.Resolve(_scope);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.UserCreateAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User not create.");
                }
            }

            return RedirectToAction(nameof(Data));
        }

        public IActionResult Data()
        {
            var model = _scope.Resolve<DataModel>();
            model.Resolve(_scope);

            return View(model);
        }

        public async Task<JsonResult> GetData()
        {
            var dataTableModel = new DataTablesAjaxRequestModel(Request);
            var model = _scope.Resolve<DataModel>();
            model.Resolve(_scope);
            var data = await model.GetUserDataAsyns(dataTableModel);

            return Json(data);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var model = _scope.Resolve<EditModel>();

            if (ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.LoadDataAsync(Id);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "User not found.");
                }
            }
            
            return View(model);
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.UpadteAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User not updated.");
                }
            }

            return RedirectToAction(nameof(Data));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Id)
        {
            var model = _scope.Resolve<DataModel>();

            if(ModelState.IsValid)
            {
                try
                {
                    model.Resolve(_scope);
                    await model.DeleteDataAsync(Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "User not Deleted.");
                }
            }

            return RedirectToAction(nameof(Data));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}