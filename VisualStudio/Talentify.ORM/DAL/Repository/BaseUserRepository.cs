using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.Utils;

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

		public virtual bool Register(BaseUser user)
		{
			// set dates
			user.JoinedDate = DateTime.Now;
			// create register code
			user.RegisterCode = Guid.NewGuid();
			// insert user
			Insert(user);
			// create default subscription
			var subscription = new Subscription()
			{
				Membership = UnitOfWork.MembershipRepository.AsQueryable().FirstOrDefault(m => m.Type == MembershipType.Free),
				User = user,
				PurchaseDate = DateTime.Now
			};
			// insert default subscription
			UnitOfWork.SubscriptionRepository.Insert(subscription);
			// commit to database
			UnitOfWork.Save();

			return true;
		}
	}
}
