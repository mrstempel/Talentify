using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Talentify.ORM.DAL.Library;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;

namespace Talentify.ORM.Mvc
{
	public class FormView<T> : BaseView<T>
	{
		private SelectList _allSchools;
		public SelectList AllSchools
		{
			get
			{
				if (_allSchools == null)
				{
					_allSchools = new SelectList(BaseController.UnitOfWork.SchoolRepository.Get(), "Id", "Name");
				}

				return _allSchools;
			}
		}

		private SelectList _allSchoolsRegister;
		public SelectList AllSchoolsRegister
		{
			get
			{
				if (_allSchoolsRegister == null)
				{
					var schools = BaseController.UnitOfWork.SchoolRepository.Get().ToList();
					schools.Add(new School() { Id = 0, Name = "Meine Schule fehlt!" });
					_allSchoolsRegister = new SelectList(schools, "Id", "Name");
				}

				return _allSchoolsRegister;
			}
		}
		
		private SelectList _allCoachingSubjects;
		public SelectList AllCoachingSubjects
		{
			get
			{
				if (_allCoachingSubjects == null)
					_allCoachingSubjects = new SelectList(BaseController.UnitOfWork.SubjectCategoryRepository.Get(), "Id", "Name");

				return _allCoachingSubjects;
			}
		}

		private IEnumerable<IFormCheckable> _allCoachingSubjectsCheckable;
		public IEnumerable<IFormCheckable> AllCoachingSubjectsCheckable
		{
			get
			{
				if (_allCoachingSubjectsCheckable == null)
					_allCoachingSubjectsCheckable = BaseController.UnitOfWork.SubjectCategoryRepository.Get();

				return _allCoachingSubjectsCheckable;
			}
		}

		private SelectList _openCoachingSubjects;
		public SelectList OpenCoachingSubjects
		{
			get
			{
				if (_openCoachingSubjects == null)
				{
					var allSubjects = BaseController.UnitOfWork.SubjectCategoryRepository.Get();
					var myUsedSubjects = BaseController.UnitOfWork.CoachingOfferRepository.Get(o => o.UserId == LoggedUser.Id);
					var myOpenSubjects = new List<SubjectCategory>();
					foreach (var s in allSubjects)
					{
						if (!myUsedSubjects.Where(m => m.SubjectCategoryId == s.Id).Any())
							myOpenSubjects.Add(s);
					}
					_openCoachingSubjects = new SelectList(myOpenSubjects, "Id", "Name");
				}

				return _openCoachingSubjects;
			}
		}

		private SelectList _allClasses;
		public SelectList AllClasses
		{
			get
			{
				if (_allClasses == null)
				{
					var classes = new List<KeyValuePair<int, string>>();
					for(int i = 1; i < 9; i++)
						classes.Add(new KeyValuePair<int, string>(i, i + ". Schulstufe"));
					_allClasses = new SelectList(classes, "key", "value");
				}

				return _allClasses;
			}
		}

		public SelectList GetEditSubjects(int selectedSubjectId)
		{
			var allSubjects = BaseController.UnitOfWork.SubjectCategoryRepository.Get();
			var myUsedSubjects = BaseController.UnitOfWork.CoachingOfferRepository.Get(o => o.UserId == LoggedUser.Id);
			var myOpenSubjects = new List<SubjectCategory>();
			foreach (var s in allSubjects)
			{
				if (!myUsedSubjects.Where(m => m.SubjectCategoryId == s.Id).Any() || s.Id == selectedSubjectId)
					myOpenSubjects.Add(s);
			}

			return new SelectList(myOpenSubjects, "Id", "Name");
		}
	}
}
