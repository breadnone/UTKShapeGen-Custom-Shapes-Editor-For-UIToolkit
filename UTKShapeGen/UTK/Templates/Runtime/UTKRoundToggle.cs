using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UTK
{
    ///<summary>Toggle visualElement. Returns boolean (true/false).</summary>
    public class UTKRoundToggle : BindableElement, INotifyValueChanged<bool>
    {
        public (Image mainContainer, Image leftLabel, Image rightLabel) containers;
        private bool _value;
        public bool value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                using (ChangeEvent<bool> evt = ChangeEvent<bool>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                    evt.StopImmediatePropagation();
                }

                SetToggle(value);
            }
        }

        public UTKRoundToggle()
        {
            focusable = true;
            containers = (new Image(), new Image(), new Image());
            containers.leftLabel.Size(50, 100, true, true);
            containers.rightLabel.Size(50, 100, true, true);
            this.Size(Utk.ScreenRatio(50), Utk.ScreenRatio(30), false, false);

            this.Add(containers.mainContainer);
            containers.mainContainer.FlexRow().FlexGrow(1).JustifyContent(Justify.Center);
            containers.mainContainer.OnMouseDown(x=>{value = !value; x.StopImmediatePropagation();});

            containers.rightLabel.scaleMode = ScaleMode.ScaleToFit;
            containers.mainContainer.scaleMode = ScaleMode.ScaleToFit;

            var line = Resources.Load<Sprite>("ROUND-LINE-thick-utk-512");
            var sprite = Resources.Load<Sprite>("circle-utk-512");
            
            containers.mainContainer.sprite = line;
            containers.leftLabel.sprite = sprite;
            containers.rightLabel.sprite = sprite;
            containers.leftLabel.AlignSelf(Align.FlexStart);
            containers.rightLabel.AlignSelf(Align.FlexEnd);

            containers.mainContainer.AddChild(containers.leftLabel);
            containers.mainContainer.AddChild(containers.rightLabel);
            _value = true;
            SetToggle(true);
        }
        private void SetToggle(bool state)
        {
            schedule.Execute(()=>
            {
                if(state)
                {
                    containers.rightLabel.Opacity(1f);
                    containers.leftLabel.Opacity(0);
                }
                else
                {
                    containers.rightLabel.Opacity(0f);
                    containers.leftLabel.Opacity(0.5f);
                }
            });
        }
        public void SetValueWithoutNotify(bool state)
        {
            _value = state;
            SetToggle(state);
        }
    }
}