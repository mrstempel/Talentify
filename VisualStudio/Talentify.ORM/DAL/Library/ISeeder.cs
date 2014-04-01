using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.UnitOfWork;

namespace Talentify.ORM.DAL.Library
{
	public interface ISeeder
	{
		string Id { get; }

		TalentifyUnitOfWork<TalentifyContext> Unit { get; }
		bool ShouldSeed { get; }

		bool Seed();
		bool FinishSeed();
	}
}
