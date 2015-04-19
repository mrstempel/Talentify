using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Content;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class SchoolRepository : TalentifyRepository<School>
	{
		public SchoolRepository(TalentifyContext context)
            : base(context)
        {
        }

		public new School GetById(int id)
		{
			return GetById(id, "SchoolType");
		}

		public IEnumerable<int> GetClasses(int schoolId)
		{
			var school = GetById(schoolId);
			var classes = new List<int>();
			for (int i = school.SchoolType.StartClass; i <= school.SchoolType.EndClass; i++)
			{
				classes.Add(i);
			}

			return classes;
		}

		public IEnumerable<School> GetMapSchools()
		{
			return AsQueryable().Where(s => s.IsActive);
		}

		public IEnumerable<SchoolInfo> GetSchoolsWithInfo()
		{
			var schoolInfos = new List<SchoolInfo>();
			var allSchools = AsQueryable().Where(s => s.IsActive);//.Take(100);
			
			foreach (var school in allSchools)
			{
				var info = from student in UnitOfWork.StudentRepository.AsQueryable()
						   join coaching in UnitOfWork.CoachingOfferRepository.AsQueryable() on student.Id equals coaching.UserId
						   where student.SchoolId == school.Id
						   select new
						   {
							   studentId = student.Id,
							   coachingId = coaching.Id
						   };
				var studentCount = info.GroupBy(i => i.studentId).Count();
				var schoolInfo = new SchoolInfo() {School = school, CoachingSubjectCount = info.Count(), CoachingStudentCount = studentCount};
				schoolInfos.Add(schoolInfo);
			}

			return schoolInfos;
		}

		public IEnumerable<School> SearchSchools(string bundesland, int schoolTypeId, string name, string address, bool onlyActive = true)
		{
			var schools = (onlyActive) ? Get(s => s.IsActive) : Get();

			if (!string.IsNullOrEmpty(bundesland))
			{
				schools = schools.Where(s => s.State == bundesland);
			}

			if (schoolTypeId > 0)
			{
				schools = schools.Where(s => s.SchoolTypeId == schoolTypeId);
			}

			if (!string.IsNullOrEmpty(name))
			{
				schools = schools.Where(s => s.Name.ToLower().Contains(name.ToLower()));
			}

			if (!string.IsNullOrEmpty(address))
			{
				schools = schools.Where(s => s.Address.ToLower().Contains(address.ToLower()));
			}

			return schools.OrderBy(s => s.Name);
		}
	}
}
