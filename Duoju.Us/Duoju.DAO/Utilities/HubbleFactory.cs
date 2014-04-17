using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Duoju.DAO.Utilities
{
    public class HubbleFactory
    {
        private static HubbleUtility hubbleInstance;

        private static readonly object locker = new object();

        private static string ConnectionStringHubble = ConfigurationManager.ConnectionStrings["connection.hubble"].ConnectionString;

        private HubbleFactory() { }

        public static HubbleUtility GetHubbleInstance()
        {
            if (hubbleInstance == null)
            {
                lock (locker)
                {
                    if (hubbleInstance == null)
                    {
                        hubbleInstance = new HubbleUtility(ConnectionStringHubble);
                    }
                }
            }
            return hubbleInstance;
        }
    }
}
