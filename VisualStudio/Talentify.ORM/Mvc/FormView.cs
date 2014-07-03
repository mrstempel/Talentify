﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Talentify.ORM.DAL.Models.Coaching;

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
					_allSchools = new SelectList(BaseController.UnitOfWork.SchoolRepository.Get(), "Id", "Name");

				return _allSchools;
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