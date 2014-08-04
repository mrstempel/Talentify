using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Migrations
{
	public class SchoolLatLngSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "SchoolLatLngSeeder"; }
		}

		public SchoolLatLngSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			adLatLng();

			return true;
		}

		private void adLatLng()
		{
			// 	HTL Innsbruck, Anichstraße
			var school1 = Unit.SchoolRepository.GetById(1);
			school1.Latitude = "47.2642253";
			school1.Longitude = "11.3897389";
			Unit.SchoolRepository.Update(school1);
			// Ferrarischule Innsbruck
			var school2 = Unit.SchoolRepository.GetById(2);
			school2.Latitude = "47.267663";
			school2.Longitude = "11.4036234";
			Unit.SchoolRepository.Update(school2);
			// Vienna Business School Schönborngasse
			var school3 = Unit.SchoolRepository.GetById(3);
			school3.Latitude = "48.2096681";
			school3.Longitude = "16.3471955";
			Unit.SchoolRepository.Update(school3);
			// GRG 21 "Bertha von Suttner" - Schulschiff
			var school4 = Unit.SchoolRepository.GetById(4);
			school4.Latitude = "48.245964";
			school4.Longitude = "16.387378";
			Unit.SchoolRepository.Update(school4);
			
			Unit.Save();
		}
	}
}
