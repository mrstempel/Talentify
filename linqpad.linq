<Query Kind="Statements">
  <Reference Relative="VisualStudio\Talentify.Web\bin\EntityFramework.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\EntityFramework.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\EntityFramework.SqlServer.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\EntityFramework.SqlServer.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\KwIt.Project.Pattern.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\KwIt.Project.Pattern.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Microsoft.Practices.Unity.Configuration.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Microsoft.Practices.Unity.Configuration.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Microsoft.Practices.Unity.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Microsoft.Practices.Unity.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Microsoft.Web.Infrastructure.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Microsoft.Web.Infrastructure.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Microsoft.Web.Mvc.FixedDisplayModes.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Microsoft.Web.Mvc.FixedDisplayModes.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Newtonsoft.Json.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Newtonsoft.Json.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Net.Http.Formatting.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Net.Http.Formatting.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Helpers.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Helpers.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Http.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Http.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Http.WebHost.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Http.WebHost.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Mvc.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Mvc.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Optimization.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Optimization.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.Razor.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.Razor.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.WebPages.Deployment.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.WebPages.Deployment.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.WebPages.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.WebPages.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\System.Web.WebPages.Razor.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\System.Web.WebPages.Razor.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Talentify.ORM.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Talentify.ORM.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\Talentify.Web.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\Talentify.Web.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\WebMatrix.Data.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\WebMatrix.Data.dll</Reference>
  <Reference Relative="VisualStudio\Talentify.Web\bin\WebMatrix.WebData.dll">&lt;MyDocuments&gt;\GitHub\Talentify\VisualStudio\Talentify.Web\bin\WebMatrix.WebData.dll</Reference>
</Query>

var UnitOfWork = new Talentify.ORM.DAL.UnitOfWork.TalentifyUnitOfWork<Talentify.ORM.DAL.Context.TalentifyContext>();

//var test = UnitOfWork.CoachingRequestRepository.GetById(8).Id;
//Console.WriteLine(test);

	var requestGroup = from coachingRequest in UnitOfWork.CoachingRequestRepository.AsQueryable()
						where (coachingRequest.FromUserId == 6 || coachingRequest.ToUserId == 6)
						group coachingRequest by coachingRequest.Id into rGrouped
						select new { Id = rGrouped.Key, All = rGrouped};

	var test2 = from schools in UnitOfWork.SchoolRepository.AsQueryable() join
				student in UnitOfWork.SchoolRepository.AsQueryable()
				select schools;
				
	//var test3 = from test in test2 group test by test.Id into groupedTest select groupedTest.AsQueryable();
Console.WriteLine(test2);