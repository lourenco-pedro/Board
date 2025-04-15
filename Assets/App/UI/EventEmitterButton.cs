using ppl.SimpleEventSystem;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI
{
    [RequireComponent(typeof(Button))]
    public class EventEmitterButton : MonoBehaviour, IEventBindable
    {
        [SerializeField] protected Button _button;
        [SerializeField] private string _eventName;

        protected virtual void Awake()
        {
            _button.onClick.AddListener(()=> this.EmitEvent<bool>(_eventName, true));
        }

        void OnValidate()
        {
            if(null == _button)
                _button = GetComponent<Button>();
        }
    }
}