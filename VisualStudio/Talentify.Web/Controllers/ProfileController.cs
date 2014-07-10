using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class ProfileController : BaseController
    {
	    public override bool KeepSearchSessionAlive
	    {
		    get { return true; }
	    }

	    public ActionResult Index(int id = 0, bool f = false)
        {
	        var user = (id == 0)
				? UnitOfWork.StudentRepository.GetById(LoggedUser.Id)
				: UnitOfWork.StudentRepository.GetById(id);

			ViewBag.CompletionCount = UnitOfWork.StudentRepository.GetProfileCompleteStatus(user);
			ViewBag.CompletionCountFull = 100 - ViewBag.CompletionCount;

		    if (HasSearchSession)
		    {
			    SetSearchSession(id);
		    }

			if (f)
				FormSuccess = new FormFeedback() { AutoClose = true };

            return View(user);
        }

	    private void SetSearchSession(int id)
	    {
		    var currentStudent = SearchSession.Items.FirstOrDefault(i => i.Id == id);
		    if (currentStudent != null)
		    {
			    var currentIndex = SearchSession.Items.IndexOf(currentStudent);
			    SearchSession.PrevItem = currentIndex > 0 
					? SearchSession.Items.ElementAt(currentIndex - 1) : 
					SearchSession.Items.ElementAt(SearchSession.Items.Count - 1);
			    SearchSession.NextItem = currentIndex == SearchSession.Items.Count - 1
				    ? SearchSession.Items.ElementAt(0)
				    : SearchSession.Items.ElementAt(currentIndex + 1);
		    }
	    }

		public ActionResult ImageUpload(HttpPostedFileBase profileUpload)
		{
			UnitOfWork.BaseUserRepository.SetProfilePicture(LoggedUser, profileUpload, Server.MapPath("~" + ConfigurationManager.AppSettings["Upload.Profile"]));
			return RedirectToAction("Index");
		}

	    public JsonResult SetCoachingEnabled(bool isEnabled)
	    {
		    try
		    {
				var user = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
				user.IsCoachingEnabled = isEnabled;
				UnitOfWork.StudentRepository.Update(user);
				UnitOfWork.Save();
				return Json(true, JsonRequestBehavior.AllowGet);
		    }
		    catch (Exception ex)
		    {
				return Json(false, JsonRequestBehavior.AllowGet);
			}
	    }

	    public ActionResult Edit()
	    {
			ViewBag.AllSchools = new SelectList(UnitOfWork.SchoolRepository.Get(), "Id", "Name");
			return View(UnitOfWork.StudentRepository.GetById(LoggedUser.Id));
	    }

		[HttpPost]
	    public ActionResult Edit(Student student)
		{
			try
			{
				UnitOfWork.StudentRepository.Update(student);
				UnitOfWork.Save();
				return RedirectToAction("Index", new { id = 0, f = true });
			}
			catch (Exception ex)
			{
				FormError = new FormFeedback() { Text = "Ein unerwarteter Fehler ist aufgetreten. Bitte versuche es erneut."};
				return View(student);
			}
		}

	    public ActionResult AddCoaching()
	    {
			return View(new CoachingOffer() { UserId = LoggedUser.Id });
	    }

		[HttpPost]
		public ActionResult AddCoaching(CoachingOffer coachingOffer)
		{
			try
			{
				coachingOffer.CreatedDate = DateTime.Now;
				UnitOfWork.CoachingOfferRepository.Insert(coachingOffer);
				UnitOfWork.Save();
				FormSuccess = new FormFeedback();
			}
			catch (Exception ex)
			{
				FormError = new FormFeedback();
			}

			return View(coachingOffer);
		}

	    public ActionResult EditCoaching(int id)
	    {
		    return View(UnitOfWork.CoachingOfferRepository.GetById(id));
	    }

		[HttpPost]
		public ActionResult EditCoaching(CoachingOffer coachingOffer, bool deleteOffer)
		{
			try
			{
				if (deleteOffer)
				{
					coachingOffer.SubjectCategoryId = 0;
					UnitOfWork.CoachingOfferRepository.Delete(coachingOffer);
					UnitOfWork.Save();
					FormSuccess = new FormFeedback() { Headline = "Lernhilfe gelöscht", Text = "Die Lernhilfe wurde von deinem Profil entfernt."};
				}
				else
				{
					coachingOffer.UpdateDate = DateTime.Now;
					UnitOfWork.CoachingOfferRepository.Update(coachingOffer);
					UnitOfWork.Save();
					FormSuccess = new FormFeedback();	
				}
			}
			catch (Exception ex)
			{
				FormError = new FormFeedback();
			}

			return View(coachingOffer);
		}

	    public ActionResult EditCoachingPrice()
	    {
		    var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
		    ViewBag.Price = student.CoachingPrice;
			return View();
	    }

		[HttpPost]
		public ActionResult EditCoachingPrice(decimal price)
		{
			ViewBag.Price = price;
			try
			{
				var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
				student.CoachingPrice = price;
				UnitOfWork.StudentRepository.Update(student, false);
				UnitOfWork.Save();
				FormSuccess = new FormFeedback();
			}
			catch (Exception)
			{
				FormError = new FormFeedback();
			}
			
			return View();
		}

	    public ActionResult CoachingRequest(int toUserId, int searchClass, int subjectCategoryId)
	    {
			// create available subjects
			ViewBag.UserSubjects = new SelectList(UnitOfWork.CoachingOfferRepository.Get(o => o.UserId == toUserId, null, "SubjectCategory"), "SubjectCategoryId", "SubjectCategory.Name");
		    return View(new CoachingRequest() { FromUserId = LoggedUser.Id, ToUserId = toUserId, Class = searchClass, SubjectCategoryId = subjectCategoryId });
	    }

		[HttpPost]
		public ActionResult CoachingRequest(CoachingRequest coachingRequest, string message)
		{
			var formFeedback = UnitOfWork.CoachingRequestRepository.Insert(coachingRequest, message);
			if (formFeedback.IsError)
			{
				FormError = formFeedback;
			}
			else
			{
				FormSuccess = formFeedback;
			}
			return View();
		}
    }
}
