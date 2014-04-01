using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Repository
{
	public class BaseUserRepository : TalentifyRepository<BaseUser>
	{
		public BaseUserRepository(TalentifyContext context)
            : base(context)
        {
        }

		public virtual BaseUser GetByEmail(string email)
		{
			return UnitOfWork.BaseUseRepository.Get(u => u.Email == email).FirstOrDefault();
		}
	}
}
