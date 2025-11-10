using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        // Index
        // Detais
        // GET : Edit
        // Post: Edit
        // Activate
        public PlanController(IPlanService planService)
        {
            this._planService = planService;
        }

        #region Index
        public IActionResult Index()
        {
            var plans = _planService.GetPlans();
            return View(plans);
        }
        #endregion

        #region Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Not Valid , Must Be Greater Than 0";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);

        }


        #endregion

        #region Edit

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErorrMassage"] = "Id Of Plan Can Not Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErorrMassage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(plan);

        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan)
        {
          
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View( UpdatedPlan);
            }
           
            var result = _planService.UpdatePlan(id, UpdatedPlan);

            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update";
            }

            return RedirectToAction(actionName: nameof(Index));
        }


        #endregion

    }
}
