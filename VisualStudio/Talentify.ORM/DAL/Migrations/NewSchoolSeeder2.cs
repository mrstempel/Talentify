using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.School;
using Talentify.ORM.DAL.Models.User;

namespace Talentify.ORM.DAL.Migrations
{
	public class NewSchoolSeeder2 : BaseSeeder
	{
		public override string Id
		{
			get { return "NewSchoolSeeder2"; }
		}

		public NewSchoolSeeder2(TalentifyContext context)
			: base(context)
		{

		}

		public override bool Seed()
		{
			if (!ShouldSeed)
				return true;

			//addSchools();

			return true;
		}

		private void addSchools()
		{
			var csvPath = HostingEnvironment.MapPath("~/Plattform_Schulen_noe.csv");
			var reader = new StreamReader(File.OpenRead(csvPath));
			var i = 0;


			using (var textReader = new StreamReader(csvPath))
			{
				string line = textReader.ReadLine();
				int skipCount = 0;
				while (line != null && skipCount < 1)
				{
					line = textReader.ReadLine();

					skipCount++;
				}

				while (line != null)
				{
					string[] columns = line.Split(';');

					Unit.SchoolRepository.Insert(new School()
					{
						Name = columns[1],
						Code = columns[2],
						Address = columns[3],
						ZipCode = columns[4],
						City = columns[5],
						Country = columns[6],
						Website = columns[7],
						Email = columns[8],
						Phone = columns[9],
						SchoolTypeId = Convert.ToInt16(columns[0]),
						Longitude = columns[10],
						Latitude = columns[11],
						IsActive = columns[12] == "1",
						State = columns[13],
						EmailSuffix = columns[14]
					});

					//perform your logic
					line = textReader.ReadLine();
				}
			}

			Unit.Save();
		}
	}
}
