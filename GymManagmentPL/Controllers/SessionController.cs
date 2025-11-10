using GymManagmentBLL.Services.Classes;
using GymManagmentBLL.Services.Interfaces;
using GymManagmentBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagmentPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService sessionService;

        public SessionController(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var sessions = sessionService.GetAllSessions();
            return View(sessions);
        }

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id";
                return RedirectToAction("Index");
            }
            var session = sessionService.GetSessionById(id);

            if(session is null)
            {
                TempData["ErrorMessage"] = "Session NotFound";
                return RedirectToAction("Index");
            }

            return View(session);

        }
  

        public ActionResult Create()
        {
            LoadCategoryForDropDowen();
            LoadTrainerForDropDowen();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if(!ModelState.IsValid)
            {
                LoadCategoryForDropDowen();
                LoadTrainerForDropDowen();
                return View(createSession);
            }

            var Created = sessionService.CreateSession(createSession);

            if(Created)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                LoadCategoryForDropDowen();
                LoadTrainerForDropDowen();
                TempData["ErrorMessage"] = "Faild To Create Session";
                return View(createSession);
            }
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Id";
                return RedirectToAction("Index");
            }

            var session = sessionService.GetSessionToUpdate(id);

            if(session is null)
            {

                TempData["ErrorMessage"] = "Not Found Session";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainerForDropDowen();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id , UpdateSessionViewModel UpdatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerForDropDowen();
                TempData["ErrorMessage"] = "Invalid Data Inputs";
                return  View(UpdatedSession);
            }

            var IsUpdatedSession = sessionService.UpdateSession(id , UpdatedSession);
            if(IsUpdatedSession)
            {
                TempData["SuccessMessage"] = "Updated Session Successfully";
           
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Update Session";
            }

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }

            var session = sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Removedsession = sessionService.DeleteSession(id);

            if(Removedsession)
            {
                TempData["SuccessMessage"] = "Session Is Deleted Successfully";
               
            }
            else
            {
                TempData["ErrorMessage"] = "Session Cannot Delete This Session";

            }

            return RedirectToAction(nameof(Index));

        }


        #region Helper Method

        private void LoadCategoryForDropDowen()
        {
            var Categories = sessionService.GetCategorySelectForDropDowen();
            ViewBag.Categories = new SelectList(Categories , "Id" , "Name");

        }
        private void LoadTrainerForDropDowen()
        {
         
            var Trainers = sessionService.GetTrainerSelectForDropDowen();
            ViewBag.Trainers = new SelectList(Trainers , "Id" , "Name");
        }


        #endregion

    }
}
