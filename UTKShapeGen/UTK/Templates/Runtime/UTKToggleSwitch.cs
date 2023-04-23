using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;


namespace UTK
{
    ///<summary>Toggle visualElement. Returns boolean (true/false).</summary>
    public class UTKToggleSwitch : BindableElement, INotifyValueChanged<bool>
    {
        public (VisualElement mainContainer, VisualElement leftToggle, VisualElement rightToggle, Label label) containers;
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

        public UTKToggleSwitch()
        {
            focusable = true;
            containers = (new VisualElement(), new VisualElement(), new VisualElement(), new Label());

            this.AddChild(containers.label as VisualElement).AddChild(containers.mainContainer).FlexRow().Height(Utk.ScreenRatio(50));
            containers.mainContainer.AddChild(containers.leftToggle).AddChild(containers.rightToggle).AlignSelf(Align.FlexStart);
            containers.mainContainer.BcgColor("#744da9").Border(2, Utk.HexColor(Color.clear, "#8e8cd8")).FlexRow().RoundCorner(Utk.ScreenRatio(15), true);
            containers.mainContainer.Height(Utk.ScreenRatio(30)).Width(Utk.ScreenRatio(60));
            containers.mainContainer.OnMouseDown(x=>{value = !value; x.StopImmediatePropagation();});
            containers.leftToggle.RoundCorner(Utk.ScreenRatio(5), true).Size(50, 100, true, true).BcgColor("#8e8cd8");
            containers.rightToggle.RoundCorner(Utk.ScreenRatio(5), true).Size(50, 100, true, true).BcgColor("#8e8cd8");
            containers.label.AlignSelf(Align.FlexStart).Text("On").Size(Utk.ScreenRatio(50), 100, false, true);
            _value = true;
            SetToggle(true);
        }
        private void SetToggle(bool state)
        {
            schedule.Execute(()=>
            {
                if(state)
                {
                    containers.label.Text("On");
                    containers.leftToggle.Opacity(0f);
                    containers.rightToggle.Opacity(1f);
                }
                else
                {
                    containers.label.Text("Off");
                    containers.leftToggle.Opacity(0.5f);
                    containers.rightToggle.Opacity(0f);
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