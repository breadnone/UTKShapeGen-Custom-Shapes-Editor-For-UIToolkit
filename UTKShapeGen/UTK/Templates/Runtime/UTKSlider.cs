using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKSlider : Slider
    {
        public float minValue
        {
            get => this.minValue;
            set => this.minValue = value;
        }
        public float maxValue
        {
            get => this.maxValue;
            set => this.maxValue = value;
        }
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

        public UTKSlider(float minValue = 0, float maxValue = 1)
        {
            var val = Utk.ScreenRatio(50);
            this.Size(val * 6, val, false, false);
            GetTrackerElement();
        }
        private void GetTrackerElement()
        {
            var sl = this as Slider;
            VisualElement dragCon = sl.Query().Where(x => x.name == "unity-drag-container");
            var lis = dragCon.Query().ToList();

            for(int i = 0; i < lis.Count; i++)
            {
                if(lis[i].name == "unity-dragger")
                {
                    lis[i].JustifyContent(Justify.Center);
                    var tracker = new Image().Size(Utk.ScreenRatio(30), Utk.ScreenRatio(20)).Top(Utk.ScreenRatio(-30)).AlignSelf(Align.Center).Position(Position.Absolute);
                    tracker.sprite = Resources.Load<Sprite>("triangle-utk-full");

                    var subs = lis[i].Query().ToList();

                    for(int j = 0; j < subs.Count; j++)
                    {
                        subs[j].Visibility(Visibility.Hidden);
                    }

                    lis[i].Add(tracker);
                    lis[i].Visibility(Visibility.Hidden);
                    tracker.Visibility(Visibility.Visible);
                    break;
                }
            } 
        }
    }
}