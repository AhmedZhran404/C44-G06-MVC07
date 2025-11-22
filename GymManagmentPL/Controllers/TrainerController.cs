using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagmentPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {

        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            this._trainerService = trainerService;
        }

        #region Retrive All Trainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();

            return View(trainers);

        }
        #endregion

        #region Create Trainer

        // 1
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateTrainerViewModel createTrainer)
        {
            if(!ModelState.IsValid)
            {
                return View(createTrainer);
            }

            var createdTrainer = _trainerService.CreateTrainer(createTrainer);

            if(createdTrainer)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Faild To Create";
            }

            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Details

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id , Must Be Greater Than 0";
                return RedirectToAction(nameof(Index));
            }

            var trainerDetails = _trainerService.GetTrainerById(id);
            
            if(trainerDetails is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(trainerDetails);

        }


        #endregion

        #region Edit Trainer

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id , Must Be Greater Than 0";
                return RedirectToAction(nameof(Index));
            }

            var trainerToEdit = _trainerService.GetTrainerToUpdate(id);

            if (trainerToEdit is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerToEdit);
        }


        [HttpPost]
        public ActionResult Edit(int id , TrainerToUpdateViewModel EditTrainer)
        {

            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(EditTrainer);
            }

            var IsUpdated = _trainerService.UpdateTrainer(id, EditTrainer);

            if(IsUpdated)
            {
                TempData["SuccessMessage"] = "Trainer Updated";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update";
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id , Must Be Greater Than 0";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerById(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var IsRemoved = _trainerService.RemoveTrainer(id);

            if (IsRemoved)
            {
                TempData["SuccessMessage"] = "Trainer Is Deleted Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Cannot Delete This Session";

            }

            return RedirectToAction(nameof(Index));

        }


        #endregion

    }
}
