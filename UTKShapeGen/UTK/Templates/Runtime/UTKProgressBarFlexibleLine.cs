using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKProgressBarFlexibleLine : VisualElement, INotifyValueChanged<float>
    {
        public (VisualElement mainContainer, UTKTitle bar) containers = (new VisualElement(), new UTKTitle());
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

        public UTKProgressBarFlexibleLine(float minValue, float maxValue, float defaultValue = 0)
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
            containers.mainContainer.AddChild(containers.bar);
            containers.bar.containers.label.Text(defaultValue.ToString());

            containers.bar.containers.label.AlignSelf(Align.FlexStart).TextAlignment(TextAnchor.UpperCenter);
            containers.bar.containers.leftLine.AlignSelf(Align.FlexStart).BcgColor("#b146c2");
            containers.bar.containers.rightLine.AlignSelf(Align.FlexStart);

            containers.bar.containers.leftLine.Width(0, true).Height(Utk.ScreenRatio(12));
            containers.bar.containers.rightLine.Width(100, true).Height(Utk.ScreenRatio(8));
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
                containers.bar.containers.leftLine.Width(calc, true);
                containers.bar.containers.rightLine.Width(100 - calc, true);
                value = calc;
                containers.bar.containers.label.Text((int)calc + "%");
            }, onComplete: ()=>
            {
                float val = end;

                if(forcedStop)
                    val = _value;

                float calc = (val/maxValue) * 100f;

                if(forcedStop)
                    containers.bar.containers.label.Text(calc + "%");
                else
                    containers.bar.containers.label.Text((int)100 + "%");

                onComplete?.Invoke(true);
                containers.bar.containers.leftLine.Width(100, true);
                containers.bar.containers.rightLine.Width(0f, true);
                isRunning = false;
                tweenId = null;
                forcedStop = false;
            }, customId: tweenId.Value);
        }
    }
}