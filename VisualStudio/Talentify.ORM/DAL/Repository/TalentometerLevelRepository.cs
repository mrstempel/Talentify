﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using KwIt.Project.Pattern.Utils;
using Talentify.ORM.DAL.Context;
using Talentify.ORM.DAL.Models.Achievements;
using Talentify.ORM.DAL.Models.Coaching;
using Talentify.ORM.DAL.Models.Membership;
using Talentify.ORM.DAL.Models.Notification;
using Talentify.ORM.DAL.Models.User;
using Talentify.ORM.FrontendLogic.Models;
using Talentify.ORM.Utils;

namespace Talentify.ORM.DAL.Repository
{
	public class TalentometerLevelRepository : TalentifyRepository<TalentometerLevel>
	{
		public TalentometerLevelRepository(TalentifyContext context)
            : base(context)
        {
        }

		public Talentometer GetTalentometer(int userId)
		{
			var talentometer = new Talentometer();

			// get user points
			talentometer.Points = UnitOfWork.BonuspointRepository.GetUserBonus(userId);
			talentometer.PointsPlus = UnitOfWork.BonuspointRepository.GetUserBonus(userId, true);

			// get current level
			talentometer.CurrentLevel = (from tal in UnitOfWork.TalentometerLevelRepository.AsQueryable()
										 where tal.MinPoints <= talentometer.PointsPlus
										orderby tal.Level descending
										select tal).FirstOrDefault();
			// get next level
			talentometer.NextLevel =
				UnitOfWork.TalentometerLevelRepository.AsQueryable().FirstOrDefault(t => t.Level > talentometer.CurrentLevel.Level);
			// set points to next level
			talentometer.PointsToNextLevel = talentometer.NextLevel.MinPoints - talentometer.PointsPlus;

			// calculate finished percent
			talentometer.PercentFinished = (int)(((double)talentometer.PointsPlus / talentometer.NextLevel.MinPoints) * 100);
			talentometer.PercentOpen = 100 - talentometer.PercentFinished;

			return talentometer;
		}
	}
}
