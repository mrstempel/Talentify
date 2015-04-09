using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.Models.Talentecheck;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class TalentecheckSessionRepository : TalentifyRepository<TalentecheckSession>
	{
		public TalentecheckSessionRepository(TalentifyContext context)
            : base(context)
        {
        }

		public bool SetAnswer(TalentecheckSession session, int frage, int answer)
		{
			if (session != null && frage > 0 && frage <= 10 && answer > 0 && answer < 5)
			{
				Type t = session.GetType();
				t.GetProperty("Frage" + frage).SetValue(session, answer);
				Update(session);
				return true;
			}

			return false;
		}

		public void SetFinished(TalentecheckSession session)
		{
			session.FinishTime = DateTime.Now;
			session.IsFinished = true;
			var results = UnitOfWork.TalentecheckSessionRepository.GetResults(session);
			session.TypMax = results.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
			session.MaxPercent = Convert.ToInt16((results[session.TypMax] / 45) * 100);
			session.TypMin = results.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
			session.MinPercent = Convert.ToInt16((results[session.TypMin] / 45) * 100);
			Update(session);
		}

		public Dictionary<TalentecheckTyp, double> GetResults(TalentecheckSession session)
		{
			var results = new Dictionary<TalentecheckTyp, double>();
			results.Add(TalentecheckTyp.Brain, 0);
			results.Add(TalentecheckTyp.Creative, 0);
			results.Add(TalentecheckTyp.Fashionista, 0);
			results.Add(TalentecheckTyp.Kommunikator, 0);
			results.Add(TalentecheckTyp.Nerd, 0);
			results.Add(TalentecheckTyp.Rockstar, 0);
			results.Add(TalentecheckTyp.Sport, 0);
			results.Add(TalentecheckTyp.Weltverbesserer, 0);

			const double max = 10;

			// frage 1
			// 1: 100% Komm., 2: 100% Sport, 3: 100% IT, 4: 100% Musik
			if (session.Frage1 == 1)
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + max;
			if (session.Frage1 == 2)
				results[TalentecheckTyp.Sport] = results[TalentecheckTyp.Sport] + max;
			if (session.Frage1 == 3)
				results[TalentecheckTyp.Nerd] = results[TalentecheckTyp.Nerd] + max;
			if (session.Frage1 == 4)
				results[TalentecheckTyp.Rockstar] = results[TalentecheckTyp.Rockstar] + max;

			// frage 2
			// 1: 50% Fashion, 50% Sport, 2: 100% Peace, 3: 100% Komm, 4: 100% Kunst
			if (session.Frage2 == 1)
			{
				results[TalentecheckTyp.Fashionista] = results[TalentecheckTyp.Fashionista] + (double)max / 2;
				results[TalentecheckTyp.Sport] = results[TalentecheckTyp.Sport] + (double)max / 2;
			}
			if (session.Frage2 == 2)
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + max;
			if (session.Frage2 == 3)
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + max;
			if (session.Frage2 == 4)
				results[TalentecheckTyp.Creative] = results[TalentecheckTyp.Creative] + max;

			// frage 3
			// 1: 100% Sport, 2: 100% Brain, 3: 100% Komm, 4: 50% Musik, 50% Kunst
			if (session.Frage3 == 1)
				results[TalentecheckTyp.Sport] = results[TalentecheckTyp.Sport] + max;
			if (session.Frage3 == 2)
				results[TalentecheckTyp.Brain] = results[TalentecheckTyp.Brain] + max;
			if (session.Frage3 == 3)
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + max;
			if (session.Frage3 == 4)
			{
				results[TalentecheckTyp.Rockstar] = results[TalentecheckTyp.Rockstar] + (double)max / 2;
				results[TalentecheckTyp.Creative] = results[TalentecheckTyp.Creative] + (double)max / 2;
			}

			// frage 4
			// 1: 100% Musik, 2: 50% Brain, 50% IT, 3: 100% Peace, 4: 100% Fashion
			if (session.Frage4 == 1)
				results[TalentecheckTyp.Rockstar] = results[TalentecheckTyp.Rockstar] + max;
			if (session.Frage4 == 2)
			{
				results[TalentecheckTyp.Nerd] = results[TalentecheckTyp.Nerd] + (double)max / 2;
				results[TalentecheckTyp.Brain] = results[TalentecheckTyp.Brain] + (double)max / 2;
			}
			if (session.Frage4 == 3)
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + max;
			if (session.Frage4 == 4)
				results[TalentecheckTyp.Fashionista] = results[TalentecheckTyp.Fashionista] + max;

			// frage 5
			// 1: 100% Brain, 2: 100% Peace, 3: 100% Fashion, 4: 100% Kunst
			if (session.Frage5 == 1)
				results[TalentecheckTyp.Brain] = results[TalentecheckTyp.Brain] + max;
			if (session.Frage5 == 2)
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + max;
			if (session.Frage5 == 3)
				results[TalentecheckTyp.Fashionista] = results[TalentecheckTyp.Fashionista] + max;
			if (session.Frage5 == 4)
				results[TalentecheckTyp.Creative] = results[TalentecheckTyp.Creative] + max;

			// frage 6
			// 1: 100% Musik, 2: 100% Brain, 3: 100% IT, 4: 100% Sport
			if (session.Frage6 == 1)
				results[TalentecheckTyp.Rockstar] = results[TalentecheckTyp.Rockstar] + max;
			if (session.Frage6 == 2)
				results[TalentecheckTyp.Brain] = results[TalentecheckTyp.Brain] + max;
			if (session.Frage6 == 3)
				results[TalentecheckTyp.Nerd] = results[TalentecheckTyp.Nerd] + max;
			if (session.Frage6 == 4)
				results[TalentecheckTyp.Sport] = results[TalentecheckTyp.Sport] + max;

			// frage 7
			// 1: 50% Komm., 50% Peace, 2: 100% Kunst, 3: 100% Brain, 4: 100% IT
			if (session.Frage7 == 1)
			{
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + (double)max / 2;
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + (double)max / 2;
			}
			if (session.Frage7 == 2)
				results[TalentecheckTyp.Creative] = results[TalentecheckTyp.Creative] + max;
			if (session.Frage7 == 3)
				results[TalentecheckTyp.Brain] = results[TalentecheckTyp.Brain] + max;
			if (session.Frage7 == 4)
				results[TalentecheckTyp.Nerd] = results[TalentecheckTyp.Nerd] + max;

			// frage 8
			// 1: 100% IT, 2: 100% Fashion, 3: 100% Kunst, 4: 50% Peace, 50% Komm.
			if (session.Frage8 == 1)
				results[TalentecheckTyp.Nerd] = results[TalentecheckTyp.Nerd] + max;
			if (session.Frage8 == 2)
				results[TalentecheckTyp.Fashionista] = results[TalentecheckTyp.Fashionista] + max;
			if (session.Frage8 == 3)
				results[TalentecheckTyp.Creative] = results[TalentecheckTyp.Creative] + max;
			if (session.Frage8 == 4)
			{
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + (double)max / 2;
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + (double)max / 2;
			}

			// frage 9
			// 1: 100% Musik, 2: 100% Fashion, 3: 100% Sport, 4: 50% Komm., 50% Peace
			if (session.Frage9 == 1)
				results[TalentecheckTyp.Rockstar] = results[TalentecheckTyp.Rockstar] + max;
			if (session.Frage9 == 2)
				results[TalentecheckTyp.Fashionista] = results[TalentecheckTyp.Fashionista] + max;
			if (session.Frage9 == 3)
				results[TalentecheckTyp.Sport] = results[TalentecheckTyp.Sport] + max;
			if (session.Frage9 == 4)
			{
				results[TalentecheckTyp.Weltverbesserer] = results[TalentecheckTyp.Weltverbesserer] + (double)max / 2;
				results[TalentecheckTyp.Kommunikator] = results[TalentecheckTyp.Kommunikator] + (double)max / 2;
			}

			return results;
		}

		public string GetTypReadable(TalentecheckTyp typ)
		{
			if (typ == TalentecheckTyp.Brain)
				return "the Brain";

			if (typ == TalentecheckTyp.Creative)
				return "Der kreative Kopf";

			if (typ == TalentecheckTyp.Fashionista)
				return "Fashionista";

			if (typ == TalentecheckTyp.Kommunikator)
				return "Kommunikator";

			if (typ == TalentecheckTyp.Nerd)
				return "Nerd";

			if (typ == TalentecheckTyp.Rockstar)
				return "Rockstar";

			if (typ == TalentecheckTyp.Sport)
				return "Sportskanone";

			if (typ == TalentecheckTyp.Weltverbesserer)
				return "Weltverbesserer";

			return string.Empty;
		}

		public Dictionary<TalentecheckTyp, int> GetTypeOverview()
		{
			var typeOverview = new Dictionary<TalentecheckTyp, int>();

			var results = Get(m => m.IsFinished).ToList();
			var countAll = results.Count;
			var test = (double) results.Count(r => r.TypMax == TalentecheckTyp.Brain)/countAll;
			typeOverview.Add(TalentecheckTyp.Brain, Convert.ToInt16(((double) results.Count(r => r.TypMax == TalentecheckTyp.Brain)/countAll)*10));
			typeOverview.Add(TalentecheckTyp.Creative, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Creative) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Fashionista, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Fashionista) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Kommunikator, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Kommunikator) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Nerd, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Nerd) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Rockstar, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Rockstar) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Sport, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Sport) / countAll) * 10));
			typeOverview.Add(TalentecheckTyp.Weltverbesserer, Convert.ToInt16(((double)results.Count(r => r.TypMax == TalentecheckTyp.Weltverbesserer) / countAll) * 10));

			return typeOverview;
		}

		public void AddShareBonus(string network, TalentecheckSession session)
		{
			var bonus = new TalentecheckBonus()
			{
				Action = TalentecheckBonusAction.ShareFb.ToString(),
				Points = TalentecheckBonusPointsFor.Share,
				CreateDate = DateTime.Now,
				TalentecheckSessionId = session.Id
			};
			UnitOfWork.TalentecheckBonusRepository.Insert(bonus);
		}
	}
}
