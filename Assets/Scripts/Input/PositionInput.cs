using System;
using UnityEngine;
using UnityEngine.UI;

namespace Input
{
    public class PositionInput : BaseInput
    {

        [SerializeField]
        private Slider _slider;
        protected override void HandleInput()
        {
           
            _slider.onValueChanged.AddListener(delegate(float arg0)
            {
                onPositionChanged?.Invoke(new Vector3(arg0/100, 0f, 0));
            });
        }

        public void ShowSlider(bool show)
        {
            _slider.gameObject.SetActive(show);
        }

        public void ResetPosition()
        {
            _slider.value = 0f;
        }

    }
}