using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Library;
using Talentify.ORM.DAL.Models;
using Talentify.ORM.DAL.UnitOfWork;

namespace Talentify.ORM.DAL.Migrations
{
	public abstract class BaseSeeder : ISeeder
	{
		/// <summary>
		/// ID of Seeder, idenfies also if allready done
		/// </summary>
		public virtual string Id
		{
			get
			{
				throw new MissingMemberException("Specifiy Id of Seeder");
			}
		}

		public TalentifyUnitOfWork<TalentifyContext> Unit { get; private set; }

		private bool? shouldSeed;
		/// <summary>
		/// if seed should be done or not
		/// </summary>
		public bool ShouldSeed
		{
			get
			{
				if (!shouldSeed.HasValue)
				{
					shouldSeed = Unit.DBMigrationHistoryRepository.Get(f => f.Name == this.Id && f.IsDone).FirstOrDefault() == null;
				}
				return shouldSeed.Value;
			}
		}

		public BaseSeeder(TalentifyContext context)
		{
			Unit = new TalentifyUnitOfWork<TalentifyContext>(context);
		}

		/// <summary>
		/// Do the work
		/// </summary>
		/// <returns></returns>
		public virtual bool Seed()
		{
			throw new MissingMethodException("Implement Seed!");
		}

		public bool FinishSeed()
		{
			if (this.ShouldSeed)
			{
				Unit.DBMigrationHistoryRepository.Insert(new DBMigrationHistory() { CreatedOn = DateTime.Now, Name = this.Id, IsDone = true });
				Unit.Save();
			}
			return true;
		}
	}
}
