using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{

    ///<summary>To be used with UTKMenu. Acts as parent container for UTKMenu.</summary>
    public class UTKMenuContainer : VisualElement
    {
        private int _value = -1;
        private List<VisualElement> menus;
        public EventCallback<VisualElement> activeMenu;
        public int value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                using (ChangeEvent<int> evt = ChangeEvent<int>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }

        public void SetValueWithoutNotify(int value)
        {
            _value = value;
        }
        public UTKMenuContainer()
        {
            this.BcgColor("#744da9").FlexRow().Size(100, Utk.ScreenRatio(50), true, false).PaddingTop(Utk.ScreenRatio(2)).PaddingBottom(Utk.ScreenRatio(Utk.ScreenRatio(3)));
            this.menus = new();
        }

        public UTKMenuContainer AddMenuComponent(VisualElement menu)
        {
            if (menus.Exists(x => x.GetHashCode() == menu.GetHashCode()))
                return this;

            this.AddChild(menu);
            menu.Height(100, true);
            menus.Add(menu);

            if (menu is UTKMenu utm)
            {
                utm.onExpandChanged += x =>
                {
                    if (menus.Count == 0 || !x)
                        return;

                    Iterate(menu, typeof(UTKMenu));
                };
            }
            else if (menu is UTKMenuSideBorder)
            {
                (menu as UTKMenuSideBorder).onExpandChanged += x =>
                {
                    if (menus.Count == 0 || !x)
                        return;

                    Iterate(menu, typeof(UTKMenuSideBorder));
                };
            }
            return this;
        }
        private void Iterate(VisualElement menu, Type type)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i] == menu)
                    continue;

                if(type == typeof(UTKMenu))
                {
                    var astype = (UTKMenu)menus[i];
                    astype.CloseMenu();
                }
                else if(type == typeof(UTKMenuSideBorder))
                {
                    var astype = (UTKMenuSideBorder)menus[i];
                    astype.CloseMenu();
                }
            }
        }

        public UTKMenuContainer SetHorizontal(bool state)
        {
            if (state)
                this.FlexRow();

            return this;
        }
        public UTKMenuContainer SetVertical(bool state)
        {
            if (state)
                this.FlexColumn();

            return this;
        }
        public UTKMenuContainer SetSpacing(float value)
        {
            if (menus.Count == 0)
                return this;

            for (int i = 0; i < menus.Count; i++)
            {
                if (i != 0 && i != menus.Count - 1)
                    menus[i].MarginRight(value);
            }

            return this;
        }
        public UTKMenuContainer SetBackgroundColor(Color color)
        {
            this.BcgColor(color);
            return this;
        }
        public UTKMenuContainer SetBackgroundColor(string hexColor)
        {
            this.BcgColor(hexColor);
            return this;
        }
        public UTKMenuContainer Center(bool state)
        {
            if (state)
                this.JustifyContent(Justify.Center);
            else
                this.JustifyContent(Justify.FlexStart);

            return this;
        }
        public UTKMenuContainer SetMenuColor(Color color)
        {
            if(menus.Count == 0)
                return this;

            for(int i = 0; i < menus.Count; i++)
            {
                if(menus[i] is UTKMenu mn)
                {
                    mn.menuColor = color;
                }
                else if(menus[i] is UTKMenuSideBorder ts)
                {
                    ts.menuColor = color;
                }
            }
            return this;
        }
    }
}