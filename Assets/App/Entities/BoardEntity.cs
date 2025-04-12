using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Entities
{
    
    [RequireComponent(typeof(Image)), RequireComponent(typeof(Button))]
    public abstract class BoardEntity : MonoBehaviour
    {
        public Coordinate Coordinate => _coordinate;
        public string Id => _id;
        
        [SerializeField] protected string _id;
        [SerializeField] protected Coordinate _coordinate;
        [SerializeField] protected Image _image;
        [SerializeField] protected Button _button;
        
        public abstract void Setup();
        protected abstract void OnClick();

        void Awake()
        {
            _id = Guid.NewGuid().ToString().Substring(0, 6);
            _button.onClick.AddListener(OnClick);
        }
        
        protected virtual void OnDestroy(){}
        
        public virtual void SetCoordinate(Coordinate coordinate, bool autoPosition = false)
        {
            _coordinate = coordinate;
            if (autoPosition)
            {
                transform.localPosition = coordinate.Center;
            }
        }

        public void OverrideId(string id)
        {
            _id = id;
        }

        public virtual void SetColor(Color color)
        {
            _image.color = color;
        }

        private void OnValidate()
        {
            if(_image == null)
                _image = GetComponent<Image>();
            
            if(_button == null)
                _button = GetComponent<Button>();
        }
    }
}
