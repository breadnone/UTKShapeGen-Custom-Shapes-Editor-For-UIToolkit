using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKDropDownFluent : VisualElement, INotifyValueChanged<string>
    {
        private string _value;
        public bool isExpanded{get;private set;}
        public float duration = 0.5f;
        public Color theme
        {
            set
            {
                iconContainer.BcgColor(value);
                containers.mainContainer.BcgColor(value);
                containers.label.BcgColor(value);
            }
        }
        private Color _fontColor;
        public Color fontColor
        {
            get => _fontColor;
            set
            {
                if(_fontColor == value)
                    return;

                _fontColor = value;

                containers.label.Color(value);
            }
        }
        public string value
        {
            get => _value;
            set
            {
                if(_value == value)
                    return;

                using (ChangeEvent<string> evt = ChangeEvent<string>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                    evt.StopImmediatePropagation();
                }
            }
        }
        public void SetValueWithoutNotify(string value)
        {
            _value = value;
            containers.label.Text(value);
        }
        public (VisualElement mainContainer, Label label, Image icon) containers = (new VisualElement(), new Label(), new Image());
        private VisualElement titleContainer;
        private VisualElement iconContainer;
        public UTKDropDownFluent(string text = "Select")
        {
            this.AlignItems(Align.FlexStart).Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50), false, false);
            this.AddChild(containers.label as VisualElement).AddChild(containers.icon);
            this.containers.mainContainer.Top(100, true).Position(Position.Absolute).BcgColor(Color.white);
            this.containers.label.PaddingLeft(5).TextAlignment(TextAnchor.MiddleLeft).Color(Utk.HexColor(Color.clear, "#4c4a48")).Text(text).BcgColor(Color.white).BorderLeft(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#0078d7")).BorderTop(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#0078d7")).BorderBottom(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#0078d7")).Size(80, 100, true, true);
            this.containers.icon.Size(100, 100, true, true);
            this.containers.icon.sprite = Resources.Load<Sprite>("triangle-utk-png");
            this.containers.icon.scaleMode = ScaleMode.ScaleToFit;

            iconContainer = new VisualElement().BcgColor(Color.white).AddChild(containers.icon).Size(10, 100, true, true).FlexGrow(1).Border(Utk.ScreenRatio(5), Utk.HexColor(Color.clear, "#0078d7"));
            titleContainer = new VisualElement().FlexRow().AddChild(containers.label).AddChild(iconContainer).Size(90, 100, true, true);
            var mainCon = new VisualElement().FlexRow().AddChild(containers.mainContainer).Top(0);
            
            var radius = Utk.ScreenRatio(15);
            this.containers.mainContainer.Display(DisplayStyle.None).RoundCorner(new Vector4(0, radius, radius, radius));
            this.AddChild(titleContainer).AddChild(mainCon);

            this.OnGeometryChanged(x=>
            {
                if(this.containers.mainContainer.childCount > 0)
                {
                    schedule.Execute(()=> ReConstruct()).ExecuteLater(1);

                    this.containers.mainContainer.schedule.Execute(()=>
                    {
                        this.containers.mainContainer.Width(this.resolvedStyle.width);
                    }).ExecuteLater(3);
                }
            });
            Construct();
        }

        private void Construct()
        {
            this.titleContainer.OnMouseDown(x=>
            {
                if(tweenId != -1)
                    return;
                    
                containers.mainContainer.Opacity(0f);

                if(containers.mainContainer.resolvedStyle.display == DisplayStyle.None)
                {
                    containers.icon.Rotate(new Rotate(new Angle(90, AngleUnit.Degree)));
                    containers.mainContainer.Display(DisplayStyle.Flex);
                    isExpanded = true;
                    containers.mainContainer.schedule.Execute(()=>
                    {
                        LerpStyle();
                    });
                }
                else
                {
                    containers.icon.Rotate(new Rotate(new Angle(0, AngleUnit.Degree)));
                    containers.mainContainer.Display(DisplayStyle.None);
                    isExpanded = false;
                }
            });
        }
        private int tweenId = -1;
        private void LerpStyle()
        {
            if(tweenId != -1)
            {
                Utk.LerpCancel(tweenId);
                tweenId = -1;
            }

            tweenId = UnityEngine.Random.Range(0, int.MaxValue);

            containers.mainContainer.LerpValue(0, 1f, duration, callback: x =>
            {
                containers.mainContainer.Opacity(x);
            }, onComplete: ()=>
            {
                containers.mainContainer.Opacity(1f);
                tweenId = -1;
            }, customId: tweenId);
        }
        public void AddMenu(string menu, Action callback)
        {
            var root = new VisualElement().Size(100, 100, true, true).PaddingLeft(Utk.ScreenRatio(10));
            var lbl = new Label().TextAlignment(TextAnchor.MiddleLeft).Text("<u>" + menu + "</u>").Size(100, 100, true, true).FlexGrow(1).Color(Utk.HexColor(Color.clear, "#4c4a48"));
            lbl.name = menu;
            root.pickingMode = PickingMode.Position;

            root.OnMouseDown(x=>
            {
                containers.label.Text(menu);
                containers.mainContainer.Display(DisplayStyle.None);
                callback?.Invoke();
            });

            root.OnMouseEnter(x=>
            {
                lbl.FontStyleAndWeight(FontStyle.BoldAndItalic);
            });
            root.OnMouseExit(x=>
            {
                lbl.FontStyleAndWeight(FontStyle.Normal);
            });

            root.AddChild(lbl);
            containers.mainContainer.AddChild(root);
        }
        private void ReConstruct()
        {
            Iterate(x=>
            {
                float sum = containers.mainContainer.childCount == 0 ? 1 : containers.mainContainer.childCount + 1;
                x.parent.Size(100f, Utk.ScreenRatio(50), true, false);
            });
        }

        private void Iterate(Action<VisualElement> action)
        {
            foreach(var child in containers.mainContainer.Query<Label>().ToList())
            {
                action(child);
            }
        }
    }
}