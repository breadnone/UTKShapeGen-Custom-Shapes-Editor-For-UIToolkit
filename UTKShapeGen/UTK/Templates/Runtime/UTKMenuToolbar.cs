using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKMenuToolbar : VisualElement, INotifyValueChanged<UTKDropDownMenu>
    {
        private List<UTKDropDownMenu> menus = new();
        public UTKDropDownMenu _value;
        public void SetValueWithoutNotify(UTKDropDownMenu menu) { _value = menu; }
        private float defHeight;
        public UTKDropDownMenu value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                using (ChangeEvent<UTKDropDownMenu> evt = ChangeEvent<UTKDropDownMenu>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }
        public UTKMenuToolbar()
        {
            var val = Utk.ScreenRatio(50);
            defHeight = val;

            this.FlexRow().Size(100, val, true, false).AlignItems(Align.FlexStart);
            
            this.OnGeometryChanged(x =>
            {
                schedule.Execute(() => Construct());
            });
        }
        private void Construct(bool center = false)
        {
            Iterate(x =>
            {
                if (x != 0)
                    menus[x].MarginLeft(2);
            });

            Iterate(x =>
            {
                menus[x].Height(defHeight);
            });
        }


        private void Iterate(Action<int> visualElement)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                visualElement(i);
            }
        }
        public UTKMenuToolbar AddMenu(UTKDropDownMenu menu)
        {
            menu.containers.titleContainer.OnMouseEnter(x =>
            {
                ClearSelector(menu);
                menu.OnSelected(true, true);
                x.StopImmediatePropagation();
            });

            menus.Add(menu);
            this.AddChild(menu as VisualElement);

            schedule.Execute(() => Construct()).ExecuteLater(1);
            return this;
        }
        private void ClearSelector(UTKDropDownMenu menu)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (menu != null)
                {
                    if (menu != menus[i])
                        menus[i].OnSelected(false, false);
                }
                else
                {
                    menus[i].OnSelected(false, false);
                }
            }
        }
        public UTKMenuToolbar RemoveMenu(UTKDropDownMenu menu)
        {
            menu.RemoveFromHierarchy();
            menus.Remove(menu);
            return this;
        }
        public UTKMenuToolbar ClearMenu()
        {
            for (int i = 0; i < menus.Count; i++)
            {
                menus[i].Clear();
                menus[i].RemoveFromHierarchy();
            }

            menus.Clear();
            return this;
        }
    }
}