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
	public class InitialSeeder : BaseSeeder
	{
		public override string Id
		{
			get { return "InitialSeeder"; }
		}

		public InitialSeeder(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			addSchools();
			addSubjectCategories();
			addMemberships();

			return true;
		}

		private void addSchools()
		{
			// cretae school types
			var htl = new SchoolType() {Code = "HTL", Name = "Höhere Technische Lehranstalt", StartClass = 1, EndClass = 5};
			var hak = new SchoolType() {Code = "HAK", Name = "Handelsakademie", StartClass = 1, EndClass = 5};
			var hbla = new SchoolType() {Code = "HBLA", Name = "Höhere Bundeslehranstalt", StartClass = 1, EndClass = 5};
			var bakip = new SchoolType() {Code = "BAKIP", Name = "Bundesbildungsanstalt für Kindergartenpädagogik", StartClass = 1, EndClass = 5};
			var ahs = new SchoolType() {Code = "AHS", Name = "Allgemeinbildende höhere Schule", StartClass = 5, EndClass = 8};
			var hlfs = new SchoolType()  {Code = "HLFS", Name = "Höhere Land- und forstwirtschaftliche Lehranstalt", StartClass = 1, EndClass = 5};
			var hlwb = new SchoolType() {Code = "HLWB", Name = "Höhere Lehranstalt für wirtschaftliche Berufe", StartClass = 1, EndClass = 5};

			Unit.SchoolTypeRepository.Insert(htl);
			Unit.SchoolTypeRepository.Insert(hak);
			Unit.SchoolTypeRepository.Insert(hbla);
			Unit.SchoolTypeRepository.Insert(bakip);
			Unit.SchoolTypeRepository.Insert(ahs);
			Unit.SchoolTypeRepository.Insert(hlfs);
			Unit.SchoolTypeRepository.Insert(hlwb);

			// create schools
			Unit.SchoolRepository.Insert(new School()
			{
				Name = "HTL Innsbruck, Anichstraße", 
				Code = "701417", 
				Address = "Anichstraße 26 - 28", 
				ZipCode = "6020", 
				City = "Innsbruck", 
				Country = "AT", 
				Website = "http://www.htlinn.ac.at", 
				Email = "direktion@htlinn.ac.at", 
				Phone = "+43 (0) 512 59717 - 0", 
				SchoolType = htl
			});

			Unit.SchoolRepository.Insert(new School()
			{
				Name = "Ferrarischule Innsbruck",
				Code = "701439",
				Address = "Weinhartstraße 4",
				ZipCode = "6020",
				City = "Innsbruck",
				Country = "AT",
				Website = "http://www.ferrarischule.at",
				Email = "hbla-w-ibk@lsr-t.gv.at",
				Phone = "+43 (0) 512 587191",
				SchoolType = hbla
			});

			Unit.SchoolRepository.Insert(new School()
			{
				Name = "Vienna Business School Schönborngasse",
				Code = "908438",
				Address = "Schönborngasse 3-5",
				ZipCode = "1080",
				City = "Wien",
				Country = "AT",
				Website = "http://www.schoenborngasse.vbs.ac.at",
				Email = "schoenborngasse@vbs.ac.at",
				Phone = "+43 (0) 1 4064514",
				SchoolType = hak
			});

			Unit.SchoolRepository.Insert(new School()
			{
				Name = "GRG 21 \"Bertha von Suttner\" - Schulschiff",
				Code = "921066",
				Address = "Donauinselplatz",
				ZipCode = "1210",
				City = "Wien",
				Country = "AT",
				Website = "http://www.schulschiff.at",
				Email = "grg21donau@921066.ssr-wien.gv.at",
				Phone = "+43 (0) 1 2714097",
				SchoolType = ahs
			});

		}

		private void addSubjectCategories()
		{
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Mathematik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Deutsch" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Fremdspachen" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Geschichte" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Geographie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Physik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Buchhaltung" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Biologie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Chemie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Informatik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Kommunikation" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Psychologie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Musik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Literatur" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Astronomie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Hauswirtschaft" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Theater" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Kunst" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Religion/Ethik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Politische Bildung" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Technik" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Darstellende Geometrie" });
			Unit.SubjectCategoryRepository.Insert(new SubjectCategory() { Name = "Wirtschaft" });
		}

		private void addMemberships()
		{
			Unit.MembershipRepository.Insert(new Membership()
			{
				Type = MembershipType.Free,
				Name = "Gratis",
				Description = "Beschreibung für Gratis Membership",
				Price = 10,
				ValidDays = 181
			});
		}
	}
}
