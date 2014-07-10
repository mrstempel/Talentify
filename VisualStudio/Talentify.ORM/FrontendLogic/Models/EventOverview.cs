using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talentify.ORM.DAL.Models.Content;

namespace Talentify.ORM.FrontendLogic.Models
{
	public class EventOverview
	{
		private List<Event> _myEvents;

		public List<Event> MyEvents
		{
			get
			{
				if (_myEvents == null)
					_myEvents = new List<Event>();
				return _myEvents;
			}
			set { _myEvents = value; }
		}

		private List<Event> _events;
		public List<Event> Events
		{
			get
			{
				if (_events == null)
					_events = new List<Event>();
				return _events;
			}
			set { _events = value; }
		}
	}
}
