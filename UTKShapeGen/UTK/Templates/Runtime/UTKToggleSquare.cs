using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UTK
{
    ///<summary>Toggle visualElement. Returns boolean (true/false).</summary>
    public class UTKToggleSquare : BindableElement, INotifyValueChanged<bool>
    {
        public (VisualElement mainContainer, VisualElement leftLabel, VisualElement rightLabel) containers;
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

        public UTKToggleSquare()
        {
            focusable = true;
            containers = (new VisualElement(), new VisualElement(), new VisualElement());
            containers.leftLabel.Size(50, 100, true, true);
            containers.rightLabel.Size(50, 100, true, true);
            this.Size(Utk.ScreenRatio(50), Utk.ScreenRatio(30), false, false);

            var line = new VisualElement().Size(100, Utk.ScreenRatio(10), true, false).BcgColor("#744da9").AlignSelf(Align.Center);

            this.Add(containers.mainContainer);
            containers.mainContainer.FlexRow().FlexGrow(1).JustifyContent(Justify.Center).Height(5);
            containers.mainContainer.OnMouseDown(x=>{value = !value; x.StopImmediatePropagation();});
            
            containers.leftLabel.BcgColor("#8e8cd8").Position(Position.Absolute).Left(0, true).RoundCorner(Utk.ScreenRatio(5));
            containers.rightLabel.BcgColor("#8e8cd8").Position(Position.Absolute).Left(50, true).RoundCorner(Utk.ScreenRatio(5));

            containers.mainContainer.AddChild(line).AddChild(containers.leftLabel).AddChild(containers.rightLabel);
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
                    containers.leftLabel.Opacity(1f);
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