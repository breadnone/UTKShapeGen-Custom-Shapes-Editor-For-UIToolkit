using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKProgressBarLine : VisualElement, INotifyValueChanged<float>
    {
        public (VisualElement mainContainer, Label indicator, VisualElement bar) containers = (new VisualElement(), new Label(), new VisualElement());
        public float minValue{get;set;} = 0f;
        public float maxValue{get;set;} = 1f;
        public float duration = 1f;
        public EventCallback<bool> onComplete;
        public bool isRunning {get;set;}
        private float defaultValue = 0f;
        private float _value;
        public float value
        {
            get => _value;
            set
            {
                if(_value == value)
                    return;

                using (ChangeEvent<float> evt = ChangeEvent<float>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }

        public UTKProgressBarLine(float minValue, float maxValue, float defaultValue = 0)
        {
            if(minValue < 0)
                minValue = 0;

            if(maxValue < 0)
                maxValue = 0;
            
            if(defaultValue < minValue)
                defaultValue = minValue;
            
            if(defaultValue > maxValue)
                defaultValue = maxValue;

            this.defaultValue = defaultValue;

            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(20), false, false).SetOverflow(Overflow.Visible); 
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.AddChild(containers.mainContainer);
            containers.mainContainer.Size(100, 100, true, true).SetOverflow(Overflow.Visible).FlexRow().BcgColor("#4c4a48");
            containers.mainContainer.AddChild(containers.bar).AddChild(containers.indicator);
            containers.bar.MarginRight(3).Size(defaultValue, 100, true, true).SetOverflow(Overflow.Hidden).BorderBottom(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#b146c2"));
            containers.indicator.AlignSelf(Align.FlexStart).TextAlignment(TextAnchor.MiddleLeft).Text(defaultValue.ToString()).Size(Utk.ScreenRatio(20), 100, false, true);            
        }


        public void Play()
        {
            LerpWidth(true);
        }
        public void Stop()
        {
            forcedStop = true;

            if(tweenId.HasValue)
            {
                var val = tweenId.Value;
                Utk.LerpCancel(ref val);
                tweenId = null;
                isRunning = false;
                forcedStop = false;
            }
        }
        public void SetValueWithoutNotify(float value)
        {
            _value = value;
        }
        private int? tweenId;
        private bool forcedStop = false;
        private void LerpWidth(bool forward)
        {
            if(isRunning)
                return;

            tweenId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            float start = minValue;
            float end = maxValue;

            if(defaultValue > 0)
                start = defaultValue;

            isRunning = true;

            containers.bar.LerpValue(start, end, duration, callback: (x)=>
            {
                float calc = (x/maxValue) * 100f;
                containers.bar.Width(calc, true);
                value = calc;
                containers.indicator.Text((int)calc + "%");

            }, onComplete: ()=>
            {
                float val = end;

                if(forcedStop)
                    val = _value;

                float calc = (val/maxValue) * 100f;

                if(forcedStop)
                    containers.indicator.Text(calc + "%");
                else
                    containers.indicator.Text((int)100 + "%");

                onComplete?.Invoke(true);
                containers.bar.Width(calc, true);
                isRunning = false;
                tweenId = null;
                forcedStop = false;
                
            }, customId: tweenId.Value);
        }
    }
}