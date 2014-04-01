using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Library;
using KwIt.Project.Pattern.DAL.UnitOfWork;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.DAL.Repository;

namespace Talentify.ORM.DAL.UnitOfWork
{
	public class TalentifyUnitOfWork<TContext>: GenericUnitOfWork<TContext>
        where TContext : TalentifyContext, new()
	{
		private IRepository<SchoolType> _schoolTypeRepository;
		public IRepository<SchoolType> SchoolTypeRepository
		{
			get
			{
				if (_schoolTypeRepository == null)
					_schoolTypeRepository = new TalentifyRepository<SchoolType>(this.Context);

				return _schoolTypeRepository;
			}
		}

		private IRepository<School> _schoolRepository;
		public IRepository<School> SchoolRepository
		{
			get
			{
				if (_schoolRepository == null)
					_schoolRepository = new TalentifyRepository<School>(this.Context);

				return _schoolRepository;
			}
		}

		private IRepository<SubjectCategory> _subjectCategoryRepository;
		public IRepository<SubjectCategory> SubjectCategoryRepository
		{
			get
			{
				if (_subjectCategoryRepository == null)
					_subjectCategoryRepository = new TalentifyRepository<SubjectCategory>(this.Context);

				return _subjectCategoryRepository;
			}
		}

		private BaseUserRepository _baseUseRepository;
		public BaseUserRepository BaseUseRepository
		{
			get
			{
				if (_baseUseRepository == null)
					_baseUseRepository = new BaseUserRepository(this.Context);

				return _baseUseRepository;
			}
		}

		private BaseUserRepository _studentRepository;
		public BaseUserRepository StudentRepository
		{
			get
			{
				if (_studentRepository == null)
					_studentRepository = new BaseUserRepository(this.Context);

				return _studentRepository;
			}
		}

		private IRepository<DBMigrationHistory> _dBMigrationHistoryRepository;
		public IRepository<DBMigrationHistory> DBMigrationHistoryRepository
		{
			get
			{
				if (_dBMigrationHistoryRepository == null)
					_dBMigrationHistoryRepository = new TalentifyRepository<DBMigrationHistory>(this.Context);

				return _dBMigrationHistoryRepository;
			}
		}

		public TalentifyUnitOfWork()
        {
            _context = new TContext();
        }

		public TalentifyUnitOfWork(TContext context)
        {
            _context = context;
        }
	}
}
