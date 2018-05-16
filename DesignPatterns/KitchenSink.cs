using System;
using System.Collections.Generic;

namespace DesignPatterns
{
    public interface ISubject
    {
        void Register(IObserver observer, string action = "");
        void UnRegister(IObserver observer, string action = "");
    }

    public interface IObserver
    {
        void Update(string action = "");
        void Dispose();
    }



    public class BasicChatroom : ISubject
    {
        private List<IObserver> _Observables = new List<IObserver>();
        private List<string> _Messages = new List<string>();

        public void AddMessage(string msg)
        {
            _Messages.Add(msg);
            Notify();
        }
        public IEnumerable<string> GetMessages()
        {
            return _Messages;
        }

        private void Notify()
        {
            foreach (var o in _Observables)
            {
                o.Update();
            }
        }

        public void Register(IObserver observer, string x ="")
        {
            _Observables.Add(observer);
        }

        public void UnRegister(IObserver observer, string x = "")
        {
            _Observables.Remove(observer);
        }
    }

    public class ChatRoomUser : IObserver
    {

        private string Username { get; set; }

        public BasicChatroom ActiveRoom { get; set; }

        public void JoinRoom(BasicChatroom chatroom)
        {
            ActiveRoom = chatroom;
            ActiveRoom.Register(this, "NEWMESSAGE");
        }

        public void SendMessage(string msg)
        {
            ActiveRoom.AddMessage(msg);
        }

        public void Update(string action)
        {
            switch (action)
            {
                case "NEWMESSAGE":
                    DrawMessages();
                    break;
            }
        }

        private void DrawMessages()
        {
            ActiveRoom.GetMessages();
            //foreach display
        }

        public void Dispose()
        {
            ActiveRoom.UnRegister(this);
        }
    }



    public class EventDrivenChatroom : ISubject
    {
        private Dictionary<string, List<IObserver>> _Observables = new Dictionary<string, List<IObserver>>();
        private List<string> _Messages = new List<string>();
        private List<ChatRoomUser> _Users = new List<ChatRoomUser>();

        public void AddMessage(string msg)
        {
            _Messages.Add(msg);
            Notify("NEWMESSAGE");
        }

        public void AddUser(ChatRoomUser user)
        {
            _Users.Add(user);
            Notify("NEWUSER");
        }

        public IEnumerable<string> GetMessages()
        {
            return _Messages;
        }

        private void Notify(string action)
        {
            if (!_Observables.ContainsKey(action))
            {
                return;
            }

            foreach (var o in _Observables[action])
            {
                o.Update(action);
            }
        }

        public void Register(IObserver observer, string action)
        {
            if (!_Observables.ContainsKey(action))
            {
                _Observables.Add(action, new List<IObserver>());
            }
            _Observables[action].Add(observer);
        }

        public void UnRegister(IObserver observer, string action)
        {
            if (!_Observables.ContainsKey(action))
            {
                return;
            }
            _Observables[action].Remove(observer);
        }
    }




}
