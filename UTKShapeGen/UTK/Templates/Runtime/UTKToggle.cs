using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UTK
{
    ///<summary>Toggle visualElement. Returns boolean (true/false).</summary>
    public class UTKToggle : BindableElement, INotifyValueChanged<bool>
    {
        public (VisualElement mainContainer, VisualElement leftToggle, VisualElement rightToggle, Label leftLabel, Label rightLabel) containers;
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
        private string _activeString = "ON";
        private string _inactiveString = "OFF";
        public string activeString
        {
            get
            {
                return _activeString;
            }
            set
            {
                this.containers.leftLabel.text = value;
                _activeString = value;
            }
        }
        public string inactiveString
        {
            get
            {
                return _inactiveString;
            }
            set
            {
                this.containers.rightLabel.text = value;
                _inactiveString = value;
            }
        }

        public UTKToggle()
        {
            focusable = true;
            containers = (new VisualElement(), new VisualElement(), new VisualElement(), new Label(), new Label().Text(inactiveString).TextAlignment(TextAnchor.MiddleCenter).FlexGrow(1));
            containers.leftLabel.Text(activeString).TextAlignment(TextAnchor.MiddleCenter).FlexGrow(1);
            containers.rightLabel.Text(inactiveString).TextAlignment(TextAnchor.MiddleCenter).FlexGrow(1);

            this.AddChild(containers.mainContainer);
            containers.mainContainer.AddChild(containers.leftToggle).AddChild(containers.rightToggle);
            containers.mainContainer.BcgColor("#4c4a48").Border(2, Utk.HexColor(Color.clear, "#0063b1")).FlexRow().RoundCorner(4, true);
            containers.mainContainer.Height(Utk.ScreenRatio(40)).Width(100, false);
            containers.mainContainer.OnMouseDown(x=>{value = !value; x.StopImmediatePropagation();});
            containers.leftToggle.RoundCorner(new Vector4(4 ,4 ,4 ,4), true).Size(50, 100, true, true).BcgColor("#0078d7");
            containers.rightToggle.RoundCorner(new Vector4(4, 4, 4, 4), true).Size(50, 100, true, true).BcgColor("#ea005e");

            containers.leftToggle.AddChild(containers.leftLabel);
            containers.rightToggle.AddChild(containers.rightLabel);
            _value = true;
            SetToggle(true);
        }
        private void SetToggle(bool state)
        {
            schedule.Execute(()=>
            {
            if(state)
            {
                containers.leftToggle.Opacity(1f);
                containers.leftLabel.text = "<b>" + containers.leftLabel.text + "</b>";
                containers.rightToggle.Opacity(0.2f);
                containers.rightLabel.text = containers.rightLabel.text;
            }
            else
            {
                containers.leftToggle.Opacity(0.2f);
                containers.leftLabel.text = containers.leftLabel.text;
                containers.rightToggle.Opacity(1f);
                containers.rightLabel.text = "<b>" + containers.rightLabel.text + "</b>";
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