using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKSliderFillBold : Slider
    {
        private VisualElement _pointer;
        public VisualElement pointer
        {
            get
            {
                return _pointer;
            }
            set
            {
                _pointer = value;
            }
        }
        public UTKSliderFillBold()
        {
            var val = Utk.ScreenRatio(50);
            this.Size(val * 6, val, false, false);
            GetTrackerElement();
        }
        private void GetTrackerElement()
        {
            var sl = this as Slider;
            VisualElement line = sl.Query("unity-tracker").First();

            var val = this.value / this.highValue * 100f;
            var fill = new VisualElement().Size(val, 100, true, true).BcgColor("#0078d7").JustifyContent(Justify.Center);
            line.AddChild(fill);

            this.OnValueChanged(x =>
            {
                var scale = x.newValue / this.highValue * 100f;
                fill.Width(scale, true);
            });

            line.Height(Utk.ScreenRatio(20)).BcgColor("#744da9");

            var t = sl.Query("unity-drag-container").First();
            t.ElementAt(2).Height(Utk.ScreenRatio(40)).Width(Utk.ScreenRatio(20));

            var tmp = t.ElementAt(2);
            tmp.FlexShrink(0);
            tmp.RemoveFromHierarchy();

            fill.AddChild(tmp);
            t.ElementAt(1).RemoveFromHierarchy();
        }
    }
}