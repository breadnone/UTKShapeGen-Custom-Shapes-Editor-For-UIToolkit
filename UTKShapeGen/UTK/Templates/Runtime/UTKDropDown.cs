using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKDropDown : VisualElement, IUState<VisualElement>, INotifyValueChanged<string>
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
            containers.title.text = value;
        }
        private List<(string menu, Action callback, VisualElement element)> _choices = new();
        private bool _roundCorner;

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
        public (VisualElement mainContainer, Image icon, Label title, VisualElement titleContainer) containers;
        private string defaultText;
        public float fadeSpeed{get;set;} = 0.5f;
        private bool isAnimating;

        public UTKDropDown(string defaultText = "Select", bool roundCorner = false)
        {
            _roundCorner = roundCorner;
            this.defaultText = defaultText;

            containers = (new VisualElement(), new Image(), new Label(), new VisualElement());
            var val = Utk.ScreenRatio(50);
            this.JustifyContent(Justify.Center);

            this.Size(val * 6, val, false, false);
            containers.titleContainer.FlexRow().Size(100, 100, true, true).AddChild(containers.title).AddChild(containers.icon);
            containers.title.Size(85, 100, true, true).PaddingLeft(Utk.ScreenRatio(8));
            containers.icon.Size(15, 100, true, true);
            containers.mainContainer.SetOverflow(Overflow.Hidden);

            this.AddChild(containers.titleContainer).AddChild(containers.mainContainer);
            containers.icon.scaleMode = ScaleMode.ScaleToFit;
            containers.titleContainer.Border(Utk.ScreenRatio(Utk.ScreenRatio(5)), Utk.HexColor(Color.clear, "#744da9")).TextAlignment(TextAnchor.MiddleLeft);
            containers.mainContainer.FlexGrow(1).FlexColumn().BcgColor(Utk.HexColor(Color.clear, "#744da9")).Padding(5).Width(100, true);
            containers.icon.sprite = Resources.Load<Sprite>("triangle-utk-png");
            containers.mainContainer.Opacity(0f).Top(val + Utk.ScreenRatio(15));
            containers.mainContainer.Position(Position.Absolute);
            schedule.Execute(() => Construct());
            containers.titleContainer.pickingMode = PickingMode.Position;

            containers.titleContainer.OnMouseDown(x =>
            {
                if(isAnimating)
                    return;

                if (!isOpen)
                {
                    containers.icon.Rotate(new Rotate(new Angle(90, AngleUnit.Degree)));
                    InterpolateOpacity();
                }
                else
                {
                    containers.icon.Rotate(new Rotate(new Angle(0, AngleUnit.Degree)));
                    containers.mainContainer.Display(DisplayStyle.None);
                    isAnimating = false;
                    containers.mainContainer.Opacity(0f);
                }

                isOpen = !isOpen;
                x.StopImmediatePropagation();
            });

            containers.mainContainer.pickingMode = PickingMode.Position;
            containers.mainContainer.OnMouseMove(x=>
            {
                x.StopImmediatePropagation();
            });

            containers.title.text = defaultText;
            Construct();
        }
        public UTKDropDown SetContainerSpace(float value)
        {
            Construct();
            schedule.Execute(()=> containers.mainContainer.Top(value));
            return this;
        }
        public UTKDropDown SetMenuSpace(float value)
        {
            Construct();
            schedule.Execute(()=> 
            {
                for(int i = 0; i < _choices.Count; i++)
                {
                    _choices[i].element.MarginBottom(value);
                }
            });
            return this;
        }
        public UTKDropDown SetDefaultText(string text) { defaultText = text; containers.title.text = text; return this; }
        public void OnEnable()
        {
        }
        public void OnDisable()
        {

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

            SetRoundCorner();

            if (_choices.Count > 0)
                Rebuild();
        }

        private void SetRoundCorner()
        {
            if (_roundCorner)
            {
                containers.titleContainer.RoundCorner(10);
                containers.mainContainer.RoundCorner(10);
            }
            else
            {
                containers.titleContainer.RoundCorner(0);
                containers.mainContainer.RoundCorner(0);
            }
        }
        private UTKDropDown SetContainerColor(Color color)
        {
            containers.mainContainer.BcgColor(color);
            return this;
        }
        private UTKDropDown SetTitleColor(Color color)
        {
            containers.titleContainer.BcgColor(color);
            return this;
        }
        private UTKDropDown SetTitleIcon(Sprite sprite)
        {
            containers.icon.sprite = sprite;
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
        public UTKDropDown AddMenu(string menu, Action callback)
        {
            if (String.IsNullOrEmpty(menu) || _choices.Exists(x => menu == x.menu))
                return this;

            var lbl = new Label().Text(menu).Size(80, Utk.ScreenRatio(50), true, false).BorderBottom(Utk.ScreenRatio(2), Color.white).TextAlignment(TextAnchor.MiddleLeft);
            lbl.MarginBottom(5);

            lbl.OnMouseDown(x =>
            {
                containers.mainContainer.Display(DisplayStyle.None);
                containers.icon.style.rotate = new Rotate(new Angle(0, AngleUnit.Degree));
                isOpen = false;
                containers.mainContainer.Opacity(0f);
                value = menu;
                callback?.Invoke();
                x.StopImmediatePropagation();
            });

            _choices.Add((menu, callback, lbl));
            containers.mainContainer.AddChild(lbl);
            return this;
        }
        private void InterpolateOpacity()
        {
            if(isAnimating)
                return;
            
            isAnimating = true;
            float start = 0f;
            float end = 1f;

            containers.mainContainer.schedule.Execute(()=>
            {
                containers.mainContainer.Display(DisplayStyle.Flex);

                containers.mainContainer.schedule.Execute(()=>
                {
                    Utk.LerpOpacity(containers.mainContainer, start, end, fadeSpeed, onComplete: ()=>
                    {
                        isAnimating = false;
                    }, customId: this.GetHashCode());
                });
            });

        }
        public UTKDropDown RemoveMenu(string menu)
        {
            if (String.IsNullOrEmpty(menu))
                return this;

            var mn = GetMenu(menu);

            if (mn.element != null)
                _choices.RemoveAt(mn.index);

            return this;
        }
        public VisualElement GetMenuElement(string menu){return GetMenu(menu).element;}
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