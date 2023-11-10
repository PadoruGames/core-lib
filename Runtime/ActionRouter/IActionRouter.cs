using System;

namespace Padoru.Core.ActionRouter
{
    public interface IActionRouter
    {
        void Subscribe(string actionId, Action<object> subscriber); 
        void Unsubscribe(string actionId, Action<object> subscriber);
		bool IsActionSubscribed(string actionId);
        void Invoke(string actionId, object actionObject);
    }
}