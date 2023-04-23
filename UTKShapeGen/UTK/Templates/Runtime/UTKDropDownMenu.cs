using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKDropDownMenu : VisualElement, IUState<VisualElement>, INotifyValueChanged<string>
    {
        private string _value;
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                using (ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }
        public void SetValueWithoutNotify(string value)
        { 
            _value = value;
        }
        private List<(string menu, Action callback, VisualElement element)> _choices = new();
        private VisualElement dummy;
        private List<(string, Action, VisualElement)> choices
        {
            get
            {
                return _choices;
            }
            set
            {
                _choices = value;
            }
        }
        private bool isOpen;
        public (VisualElement mainContainer, Label title, VisualElement titleContainer) containers;
        private string defaultText;

        public UTKDropDownMenu(string defaultText = "Menu 1")
        {
            var val = Utk.ScreenRatio(50);
            this.Size(val * 6, val);
            this.defaultText = defaultText;
            containers = (new VisualElement(), new Label(), new VisualElement());

            containers.titleContainer.FlexRow().Size(100, val, true, false).AddChild(containers.title);
            containers.title.Size(100, 100, true, true);
            containers.mainContainer.SetOverflow(Overflow.Hidden).FlexRow().Position(Position.Absolute).Top(Utk.ScreenRatio(0));
            dummy = new VisualElement().Height(0).FlexGrow(0);
            dummy.AddChild(containers.mainContainer);
            this.AddChild(containers.titleContainer).AddChild(dummy);

            containers.titleContainer.BorderBottom(Utk.ScreenRatio(Utk.ScreenRatio(10)), Utk.HexColor(Color.clear, "#ea005e")).TextAlignment(TextAnchor.MiddleCenter).AlignSelf(Align.Center);
            containers.mainContainer.FlexGrow(1).FlexColumn().BcgColor(Utk.HexColor(Color.clear, "#744da9")).Padding(5);
            containers.mainContainer.Width(100, true);
            containers.mainContainer.Opacity(0f);
            schedule.Execute(() => Construct());

            containers.titleContainer.OnMouseDown(x =>
            {
                OnSelected(!isOpen, true);
                x.StopImmediatePropagation();
            });

            containers.title.text = defaultText;
            Construct();
        }
        private int animId;
        public void OnSelected(bool state, bool onPointerOver)
        {
            if(animId != 0)
                Utk.LerpCancel(ref animId);
                
            containers.mainContainer.Opacity(0f);

            if (state)
            {
                containers.mainContainer.Display(DisplayStyle.Flex);
            }
            else
            {
                containers.mainContainer.Display(DisplayStyle.None);
            }

            if(onPointerOver && state)
            {
                animId = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
                Utk.LerpOpacity(containers.mainContainer, 0f, 1f, 0.5f, customId: animId);
            }

            isOpen = state;
        }

        public UTKDropDownMenu SetContainerSpace(float value)
        {
            Construct();
            schedule.Execute(() => containers.mainContainer.Top(value));
            return this;
        }
        public UTKDropDownMenu SetMenuSpace(float value)
        {
            Construct();
            schedule.Execute(() =>
            {
                for (int i = 0; i < _choices.Count; i++)
                {
                    _choices[i].element.MarginBottom(value);
                }
            });
            return this;
        }

        public UTKDropDownMenu SetDefaultText(string text) { defaultText = text; containers.title.text = text; return this; }
        public void OnEnable()
        {
            //TODO:
        }
        public void OnDisable()
        {
            //TODO:
            containers.mainContainer.Display(DisplayStyle.None);
            isOpen = false;
            containers.mainContainer.Opacity(0f);
        }
        private void Construct()
        {
            containers.mainContainer.Clear();
            _value = null;
            isOpen = false;
            containers.mainContainer.Display(DisplayStyle.None);

            if (!String.IsNullOrEmpty(defaultText))
                containers.title.text = defaultText;
            else
                containers.title.text = "Select";

            if (_choices.Count > 0)
                Rebuild();
        }

        private UTKDropDownMenu SetContainerColor(Color color)
        {
            containers.mainContainer.BcgColor(color);
            return this;
        }
        private UTKDropDownMenu SetTitleColor(Color color)
        {
            containers.titleContainer.BcgColor(color);
            return this;
        }

        private void Rebuild()
        {
            for (int i = 0; i < _choices.Count; i++)
            {
                containers.mainContainer.AddChild(_choices[i].element);

                if (!String.IsNullOrEmpty(value) && _choices[i].menu == value)
                {
                    containers.title.text = _choices[i].menu;
                }
            }
        }
        public UTKDropDownMenu AddMenu(string menu, Action callback)
        {
            if (String.IsNullOrEmpty(menu) || _choices.Exists(x => menu == x.menu))
                return null;
            
            var lbl = new Label().Text(menu).Size(80, Utk.ScreenRatio(50), true, false).BorderBottom(Utk.ScreenRatio(2), Color.white).TextAlignment(TextAnchor.MiddleLeft);
            lbl.MarginBottom(5);

            lbl.OnMouseDown(x =>
            {
                containers.mainContainer.Display(DisplayStyle.None);
                isOpen = false;
                containers.mainContainer.Opacity(0f);
                callback?.Invoke();
                value = menu;
                x.StopImmediatePropagation();
            });

            lbl.pickingMode = PickingMode.Position;
            lbl.OnMouseEnter(x => { lbl.FontStyleAndWeight(FontStyle.Bold); x.StopImmediatePropagation(); });
            lbl.OnMouseExit(x => { lbl.FontStyleAndWeight(FontStyle.Normal); x.StopImmediatePropagation(); });

            _choices.Add((menu, callback, lbl));
            containers.mainContainer.AddChild(lbl);
            return this;
        }
        public UTKDropDownMenu RemoveMenu(string menu)
        {
            if (String.IsNullOrEmpty(menu))
                return this;

            var mn = GetMenu(menu);

            if (mn.element != null)
                _choices.RemoveAt(mn.index);

            return this;
        }
        public VisualElement GetMenuElement(string menu) { return GetMenu(menu).element; }
        public VisualElement[] GetMenuElements(string menu)
        {
            VisualElement[] vis = new VisualElement[_choices.Count];

            for (int i = 0; i < _choices.Count; i++)
            {
                vis[i] = _choices[i].element;
            }
            return vis;
        }
        public void SetSelection(string menu)
        {
            var mn = GetMenu(menu);

            if (mn.element != null)
            {
                containers.title.text = mn.menu;
                value = mn.menu;

                if (_value != mn.menu)
                    mn.callback?.Invoke();
            }
        }
        private (string menu, Action callback, VisualElement element, int index) GetMenu(string menu)
        {
            for (int i = 0; i < _choices.Count; i++)
            {
                if (menu == _choices[i].menu)
                    return (_choices[i].menu, _choices[i].callback, _choices[i].element, i);
            }
            return (null, null, null, -1);
        } 
    }
}