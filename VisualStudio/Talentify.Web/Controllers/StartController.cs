using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Talentify.ORM.FrontendLogic;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Mvc;

namespace Talentify.Web.Controllers
{
    public class StartController : BaseController
	{
		#region Members & Properties

		protected DateTime? LastStreamItemDate
	    {
		    get
		    {
			    if (Session["LastStreamItemDate"] != null)
				    return Session["LastStreamItemDate"] as DateTime?;
			    
				return null;
		    }
			set { Session["LastStreamItemDate"] = value; }
	    }

		#endregion

		public ActionResult Index()
        {
			ViewBag.IsFirstLogin = true;
			if (Session["IsFirstLogin"] != null && Convert.ToBoolean(Session["IsFirstLogin"]))
				ViewBag.IsFirstLogin = true;

	        LastStreamItemDate = null;
	        Session["IsFirstLogin"] = null;
	        var student = UnitOfWork.StudentRepository.GetById(LoggedUser.Id);
			var surveyToken = UnitOfWork.ActionTokenRepository.GetSurveyToken(LoggedUser.Id);
			ViewBag.Latitude = (student.HasSchool) ? student.School.Latitude : ConfigurationManager.AppSettings["SchoolMap.Default.Lat"];
			ViewBag.Longitude = (student.HasSchool) ? student.School.Longitude : ConfigurationManager.AppSettings["SchoolMap.Default.Lng"];
			ViewBag.AllSchools = UnitOfWork.SchoolRepository.GetSchoolsWithInfo();
			ViewBag.ShowSurvey = surveyToken.ValidUntil != DateTime.MinValue;
            return View(new SearchParams());
        }

	    public ActionResult Stream()
	    {
		    var streamProvider = new StreamProvider(UnitOfWork);
		    var stream = streamProvider.GetStream(LastStreamItemDate);

		    if (stream != null)
			    LastStreamItemDate = stream.Last().Time;
			
			return View(stream);
	    }

	    public ActionResult Events()
	    {
			ViewBag.RegisteredIds = UnitOfWork.EventRepository.GetUserRegisteredEventIds(LoggedUser.Id);
		    return View(UnitOfWork.EventRepository.GetOnlineEvents().Take(3));
	    }

	    public ActionResult Survey()
	    {
		    var actionToken = UnitOfWork.ActionTokenRepository.GetSurveyToken(LoggedUser.Id);
		    actionToken.ValidUntil = DateTime.MinValue;
			UnitOfWork.ActionTokenRepository.Update(actionToken);
		    UnitOfWork.Save();

		    return View();
	    }

		[HttpPost]
		public ActionResult Survey(string parentEducation, string hearedOfTalentifyOption, string hearedOfTalentifyText)
		{
			var hearedOfTalentify = (!string.IsNullOrEmpty(hearedOfTalentifyText))
				? hearedOfTalentifyText
				: hearedOfTalentifyOption;

			if (UnitOfWork.StudentRepository.SetSurveyData(LoggedUser.Id, parentEducation, hearedOfTalentify))
			{
				FormSuccess = new FormFeedback();
			}
			else
			{
				FormError = new FormFeedback();
			}

			return View();
		}
    }
}
