using Swarms.Entities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Swarms.Utility
{
    public static class AgentCommunication
    {
        public static void broadcastMessage(Agent sender, List<Agent> receivers) {
            foreach (var receiver in receivers)
            {
                sendMessage(sender._location, sender._target);
            }
        }
        public static Message sendMessage(Vector2 location, Tree tree) {
            return new Message{_location = location, _tree = tree};
        }

        public static void receiveMessage() {
            
        }
    }
}