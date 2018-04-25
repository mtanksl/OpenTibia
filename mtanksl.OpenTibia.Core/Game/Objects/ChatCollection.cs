using System;
using System.Linq;
using System.Collections.Generic;

namespace OpenTibia
{
    public class ChatCollection
    {
        private List<Chat> chats = new List<Chat>
        {
            new Chat() { Id = 2, Name = "Tutor" },

            new Chat() { Id = 3, Name = "Rule Violations" },

            new Chat() { Id = 4, Name = "Gamemaster" },

            new Chat() { Id = 5, Name = "Game Chat" },

            new Chat() { Id = 6, Name = "Trade" },

            new Chat() { Id = 7, Name = "Trade-Rookgaard" },

            new Chat() { Id = 8, Name = "Real Life Chat" },

            new Chat() { Id = 9, Name = "Help" }
        };

        private ushort GenerateId()
        {
            for (ushort id = 10; id < 65535; id++)
            {
                if ( !chats.Any(chat => chat.Id == id) )
                {
                    return id;
                }
            }

            throw new Exception();
        }

        public T AddChat<T>(T chat) where T : Chat
        {
            chat.Id = GenerateId();

            chats.Add(chat);

            return chat;
        }

        public void RemoveChat(Chat chat)
        {
            chats.Remove(chat);
        }

        public Chat GetChat(int chatId)
        {
            return GetChats().Where(chat => chat.Id == chatId).FirstOrDefault();
        }

        public PrivateChat GetPrivateChat(Player owner)
        {
            return GetPrivateChats().Where(chat => chat.Owner == owner).FirstOrDefault();
        }

        public IEnumerable<Chat> GetChats()
        {
            return chats;
        }

        public IEnumerable<PrivateChat> GetPrivateChats()
        {
            return chats.OfType<PrivateChat>();
        }
    }
}