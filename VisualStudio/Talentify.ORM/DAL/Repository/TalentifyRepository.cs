using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using KwIt.Project.Pattern.DAL.Repository;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.UnitOfWork;

namespace Talentify.ORM.DAL.Repository
{
	public class TalentifyRepository<TEntity> : GenericRepository<TEntity> 
        where TEntity: BaseEntity
    {
		private TalentifyUnitOfWork<TalentifyContext> _unitOfWork;
		protected TalentifyUnitOfWork<TalentifyContext> UnitOfWork
		{
			get
			{
				if (_unitOfWork == null)
					_unitOfWork = new TalentifyUnitOfWork<TalentifyContext>(this.Context as TalentifyContext);
				return _unitOfWork;
			}
		}

		public TalentifyRepository(TalentifyContext context)
            : base(context)
        {
        }
    }
}
