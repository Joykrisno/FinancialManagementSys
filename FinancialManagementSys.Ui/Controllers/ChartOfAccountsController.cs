using FinancialManagement.Domain.Entities;
using FinancialManagement.Web.Servic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Web.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private readonly IApiService _apiService;

        public ChartOfAccountsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: ChartOfAccounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _apiService.GetAsync<List<ChartOfAccount>>("ChartOfAccounts");
            return View(accounts);
        }

        // GET: ChartOfAccounts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var account = await _apiService.GetAsync<ChartOfAccount>($"ChartOfAccounts/{id}");
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // GET: ChartOfAccounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChartOfAccounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChartOfAccount account)
        {
            if (ModelState.IsValid)
            {
                var result = await _apiService.PostAsync("ChartOfAccounts", account);
                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(account);
        }

        // GET: ChartOfAccounts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _apiService.GetAsync<ChartOfAccount>($"ChartOfAccounts/{id}");
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: ChartOfAccounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ChartOfAccount account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _apiService.PutAsync($"ChartOfAccounts/{id}", account);
                if (result.IsSuccess)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(account);
        }

        // GET: ChartOfAccounts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _apiService.GetAsync<ChartOfAccount>($"ChartOfAccounts/{id}");
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: ChartOfAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _apiService.DeleteAsync($"ChartOfAccounts/{id}");
            if (result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Delete), new { id, error = result.ErrorMessage });
        }
    }
}