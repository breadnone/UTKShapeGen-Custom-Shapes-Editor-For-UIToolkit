using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKSliderFill : Slider
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
        public UTKSliderFill()
        {
            var val = Utk.ScreenRatio(50);
            this.Size(val * 6, val, false, false);
            GetTrackerElement();
        }
        private void GetTrackerElement()
        {
            var sl = this as Slider;
            var lis = sl.Query().ToList();
            VisualElement line = sl.Query("unity-tracker").First();

            var val = this.value / this.highValue * 100f;
            var fill = new VisualElement().Size(val, 100, true, false).BcgColor("#0078d7").JustifyContent(Justify.Center);
            line.AddChild(fill);
            line.Height(Utk.ScreenRatio(8));

            this.OnValueChanged(x =>
            {
                var scale = x.newValue / this.highValue * 100f;
                fill.Width(scale, true);
            });


            var t = sl.Query("unity-drag-container").First();
            t.ElementAt(2).Size(Utk.ScreenRatio(30), Utk.ScreenRatio(30)).FlexShrink(0);
            var tmp = t.ElementAt(2);
            tmp.RemoveFromHierarchy();

            fill.AddChild(tmp);

            t.ElementAt(1).RemoveFromHierarchy();
        }
    }
}