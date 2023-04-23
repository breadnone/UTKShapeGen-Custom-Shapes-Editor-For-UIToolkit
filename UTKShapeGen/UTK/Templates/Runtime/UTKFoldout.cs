using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Breadnone;

#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif

namespace UTK
{
    public class UTKFoldout : VisualElement, INotifyValueChanged<bool>
    {
        public (UTKToggle toggle, Label title) elements { get; set; }
        public Action<bool> toggleValueChanged;
        public (VisualElement mainContainer, VisualElement titleContainer, VisualElement leftContainer, VisualElement rightContainer, Image icon) containers;
        public (Color foldColor, Color unfoldColor) colors { get; set; }
        public string text { get { return elements.title.text; } set { elements.title.text = value; } }
        private bool _value = true;
        public float duration { get; set; } = 0.3f;
        public bool isAnimating { get; private set; } = false;
        public Ease ease { get; set; } = Ease.Linear;
        public bool value
        {
            get { return _value; }
            set
            {
                if (!value)
                {
                    containers.icon.style.rotate = new Rotate(new Angle(90, AngleUnit.Degree));
                }
                else
                {
                    containers.icon.style.rotate = new Rotate(new Angle(0, AngleUnit.Degree));
                }

                _value = value;
            }
        }

        public UTKFoldout(string text = "", bool defaultIsFolded = true)
        {
            this.FlexColumn();
            var titleContainer = new VisualElement().BcgColor("#744da9").Size(100, Utk.ScreenRatio(50), true, false).FlexShrink(0).FlexRow();
            this.AddChild(titleContainer);
            var mainContainer = new VisualElement().FlexGrow(1);

            titleContainer.OnMouseDown(MouseDownEvent);

            containers = (mainContainer, titleContainer, new VisualElement(), new VisualElement(), new Image());
            containers.leftContainer.Size(47.5f, 100, true, true);
            containers.rightContainer.Size(47.5f, 100, true, true);

            titleContainer.AddChild(containers.icon).AddChild(containers.leftContainer).AddChild(containers.rightContainer);
            this.AddChild(titleContainer).AddChild(mainContainer);

            elements = (new UTKToggle(), new Label());
            ConstructLayout(defaultIsFolded);
            elements.title.text = text;

            var tri = Resources.Load<Sprite>("triangle-utk-png");
            containers.icon.scaleMode = ScaleMode.ScaleToFit;
            containers.icon.sprite = tri;
            containers.icon.Size(5, 100, true, true);
        }
        private void MouseDownEvent(MouseDownEvent evt)
        {
            if (isAnimating || containers.mainContainer.childCount == 0)
                return;

            Animate();
        }
        private void ConstructLayout(bool foldedState)
        {
            containers.rightContainer.PaddingRight(2).PaddingLeft(2).JustifyContent(Justify.Center);
            elements.title.TextAlignment(TextAnchor.MiddleLeft).PaddingLeft(2, true).AlignSelf(Align.FlexStart);
            containers.leftContainer.AddChild(elements.title).JustifyContent(Justify.Center);
            containers.rightContainer.AddChild(elements.toggle);
            elements.toggle.AlignSelf(Align.FlexEnd);
            containers.mainContainer.Display(DisplayStyle.None);

            elements.toggle.OnValueChanged(x =>
            {
                toggleValueChanged?.Invoke(x.newValue);
                x.StopImmediatePropagation();
            });
        }
        public void AddToContainer(VisualElement visualElement)
        {
            if (value)
            {
                visualElement.Opacity(0);
            }
            else
            {
                visualElement.Opacity(1);
            }

            containers.mainContainer.AddChild(visualElement);
        }
        public void ClearContainers() { containers.mainContainer.Clear(); }
        public void SetColorState(Color foldColor, Color unfoldColor) { colors = (foldColor, unfoldColor); }
        public void SetTitleFontStyle(float size, Color color, bool isDynamic) { elements.title.FontSize(size, isDynamic).Color(color); }
        private List<VisualElement> GetChild()
        {
            List<VisualElement> lis = new();
            foreach (var i in containers.mainContainer.Children()) { lis.Add(i); }
            return lis;
        }
        public void SetValueWithoutNotify(bool state) { value = state; }
        private void Animate()
        {
            if (value)
            {
                isAnimating = true;
                containers.mainContainer.Display(DisplayStyle.Flex);

                VTween.valueFast(containers.mainContainer, 0, 1, duration, callback: (float x) => { SetOpacity(x); }, 0, false, () =>
                {
                    //OnComplete alpha
                    SetOpacity(1f);
                    isAnimating = false;
                }, ease: ease);
            }
            else
            {
                containers.mainContainer.Display(DisplayStyle.None);
                SetOpacity(0);
            }

            value = !value;
        }
        private void SetOpacity(float value)
        {
            var child = GetChild();
            for (int i = 0; i < child.Count; i++) { child[i].Opacity(value); }
        }
    }
}
