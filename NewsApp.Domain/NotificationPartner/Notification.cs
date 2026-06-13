using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsApp.Domain.NotificationPartner
{
    public  class Notification
    { 
		public string Key { get; }
		public string Message { get; }

		public Notification()
        {

        }
		public Notification(string key, string message)
		{
			Key = key;
			Message = message;
		}
	}

}
