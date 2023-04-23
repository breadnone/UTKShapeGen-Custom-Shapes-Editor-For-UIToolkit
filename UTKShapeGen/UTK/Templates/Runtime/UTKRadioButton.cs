using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UTK
{
    ///<summary>Toggle visualElement. Returns boolean (true/false).</summary>
    public class UTKRadioButton : BindableElement, INotifyValueChanged<bool>
    {
        public (VisualElement mainContainer, VisualElement toggle, VisualElement innerToggle, Label label) containers;
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

        public string text
        {
            get => containers.label.text;
            set
            {
                containers.label.Text(value);
            }
        }
        public UTKRadioButton(string text = "Radio Button")
        {
            focusable = true;
            containers = (new VisualElement(), new VisualElement(), new VisualElement(), new Label());

            this.AddChild(containers.mainContainer);
            containers.mainContainer.Size(Utk.ScreenRatio(220), Utk.ScreenRatio(30), false, false).FlexRow();
            containers.mainContainer.AddChild(containers.toggle).AddChild(containers.label);
            containers.toggle.AddChild(containers.innerToggle).Padding(Utk.ScreenRatio(3), true).JustifyContent(Justify.Center).RoundCorner(Utk.ScreenRatio(5)).Border(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#8e8cd8")).Size(Utk.ScreenRatio(35), Utk.ScreenRatio(35), false, false);
            containers.innerToggle.BcgColor("#b146c2").Size(100, 100, true, true).FlexGrow(1).JustifyContent(Justify.Center);
            containers.toggle.pickingMode = PickingMode.Position;

            containers.label.Text(text).MarginLeft(Utk.ScreenRatio(10)).TextAlignment(TextAnchor.MiddleLeft).Height(100, true);

            containers.toggle.OnMouseDown(x=>
            {
                value = !value;
            });

            containers.label.OnMouseDown(x=>
            {
                value = !value;
            });

            SetToggle(false);
        }
        private void SetToggle(bool state)
        {
            schedule.Execute(()=>
            {
                if(state)
                {
                    containers.innerToggle.Opacity(1f);
                }
                else
                {
                    containers.innerToggle.Opacity(0f);
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