using UnityEngine;

namespace Game.Lib
{
    public interface ICallbackReceiver
    {
        void OnCallbackReceived(GameObject caller);
    }
}