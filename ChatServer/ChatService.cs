using ChatInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]

    public class ChatService : IChatService
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClient = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName)
        {
            // Checking if username is in use
            foreach(var client in _connectedClient)
            {
                if(client.Key.ToLower() == userName.ToLower()) 
                { 
                    // If in use
                    return 1; 
                }
            }
            var establishUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();
            ConnectedClient newClient = new ConnectedClient();
            newClient.connection = establishUserConnection;
            newClient.UserName = userName;

            _connectedClient.TryAdd(userName, newClient);

            return 0;
        }

        public void SendToAll(string message, string userName)
        {
            foreach(var client in _connectedClient)
            {
                if (client.Key.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
        }
    }
}
