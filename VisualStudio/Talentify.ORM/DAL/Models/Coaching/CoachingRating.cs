using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Models;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Models.Coaching
{
	public enum RatingType
	{
		Helpful,
		OnTime,
		Price
	}

	public class CoachingRating : BaseEntity
	{
		public int CoachingRequestId { get; set; }
		private CoachingRequest _coachingRequest;
		public CoachingRequest CoachingRequest
		{
			get { return _coachingRequest; }
			set
			{
				_coachingRequest = value;
				if (value != null)
					CoachingRequestId = value.Id;
			}
		}

		public int FromUserId { get; set; }
		private Student _fromUser;
		public virtual Student FromUser
		{
			get { return _fromUser; }
			set
			{
				_fromUser = value;
				if (value != null)
					FromUserId = value.Id;
			}
		}

		public int ToUserId { get; set; }
		private Student _toUser;
		public virtual Student ToUser
		{
			get { return _toUser; }
			set
			{
				_toUser = value;
				if (value != null)
					ToUserId = value.Id;
			}
		}

		public RatingType RatingType { get; set; }
		public int Value { get; set; }
	}
}
