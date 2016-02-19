using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using AspNetIdentity.WebApi.Entities;
using AspNetIdentity.WebApi.Infrastructure;

namespace AspNetIdentity.WebApi.Hubs
{
    [Authorize]
    public class CentralHub : Hub
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<CentralHub>();

        public override Task OnConnected()
        {
            string ID = HttpContext.Current.User.Identity.GetUserId();
            string userName = HttpContext.Current.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            ConnectedUsers connectedUser = db.ConnectedUsers.FirstOrDefault(c => c.UserName == userName);

            if (Object.Equals(connectedUser, null))
            {
                try
                {
                    connectedUser = new ConnectedUsers()
                    {
                        ConnectionId = connectionId,
                        UserName = userName,
                        UserID = ID,
                        Date = DateTime.Now
                    };
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        context.ConnectedUsers.Add(connectedUser);
                        int x = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    new ArgumentException("Problem adding ConnectedUsers");
                }
            }
            else
            {
                try
                {
                    connectedUser.ConnectionId = connectionId;
                    connectedUser.Date = DateTime.Now;
                    int x = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    new ArgumentException("Problem adding ConnectedUsers");
                }
            }

            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            string ID = HttpContext.Current.User.Identity.GetUserId();
            string userName = HttpContext.Current.User.Identity.Name;
            var connectionId = Context.ConnectionId;
            ConnectedUsers connectedUser = db.ConnectedUsers.FirstOrDefault(c => c.UserName == userName);

            if (Object.Equals(connectedUser, null))
            {
                try
                {
                    connectedUser = new ConnectedUsers()
                    {
                        ConnectionId = connectionId,
                        UserName = userName,
                        UserID = ID,
                        Date = DateTime.Now
                    };
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        context.ConnectedUsers.Add(connectedUser);
                        int x = context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    new ArgumentException("Problem reconnecting to ConnectedUsers");
                }
            }
            else
            {
                try
                {
                    connectedUser.ConnectionId = connectionId;
                    connectedUser.Date = DateTime.Now;
                    int x = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    new ArgumentException("Problem reconnecting ConnectedUsers");
                }
            }

            return base.OnReconnected();
        }
         
        public void SendNotification(Enum type, ApplicationUser user, List<ApplicationUser> users)
        {
            foreach (var appUser in users)
            {
                ConnectedUsers connectedUser = db.ConnectedUsers.FirstOrDefault(c => c.UserID == appUser.Id);
                if (!Object.Equals(connectedUser, null))
                    Clients.Client(connectedUser.ConnectionId).notificationReceived(user.UserName, type);
             
            }
        }

        public static async Task Static_SendNotification(Enum type, ApplicationUser user, List<string> users)
        {
            foreach (var appUser in users)
            {
                ConnectedUsers connectedUser = db.ConnectedUsers.FirstOrDefault(c => c.UserID == appUser);
                if (!Object.Equals(connectedUser, null))
                    await hubContext.Clients.Client(connectedUser.ConnectionId).notificationReceived(user.UserName, type);
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUsers connectedUser = db.ConnectedUsers.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            if (!Object.Equals(connectedUser, null))
            {
                try
                {
                    db.ConnectedUsers.Remove(connectedUser);
                    int x = db.SaveChanges();
                }
                catch (Exception ex)
                {
                    new ArgumentException("Problem removing ConnectedUser: " + connectedUser.UserID);
                }
            }


            return base.OnDisconnected(stopCalled);
        }
    }
}