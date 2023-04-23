using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UTK
{
    public class UTKSliderValue : Slider
    {
        private VisualElement _pointer;
        public Color valueBackgroundColor
        {
            get
            {
                return valueField.resolvedStyle.backgroundColor;
            }
            private set
            {
                valueField.style.backgroundColor = value;
            }
        }
        private VisualElement valueField;
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
        public UTKSliderValue()
        {
            this.showInputField = true;
            var val = Utk.ScreenRatio(50);
            this.Size(val * 8, val + (val/2f), false, false);
            GetTrackerElement(); 
        }
        private void GetTrackerElement()
        {
            var sl = this as Slider;
            var lis = sl.Query().ToList();

            VisualElement contentDragger = null;
            VisualElement valueField = null;
            VisualElement icon = null;


            for(int i = 0; i < lis.Count; i++)
            {
                if(lis[i].name == "unity-tracker")
                {
                    lis[i].Height(Utk.ScreenRatio(8)).BcgColor("#744da9");
                }

                if(lis[i].name =="unity-text-field")
                {
                    valueField = lis[i];
                    valueField.RemoveFromHierarchy();
                }
                
                if(lis[i].name == "unity-dragger")
                {
                    lis[i].JustifyContent(Justify.Center);
                    var tracker = new Image().Size(Utk.ScreenRatio(30), Utk.ScreenRatio(20)).Top(Utk.ScreenRatio(-30)).AlignSelf(Align.Center).Position(Position.Absolute);
                    tracker.sprite = Resources.Load<Sprite>("triangle-utk-full");
                    icon = tracker;

                    var subs = lis[i].Query().ToList();

                    for(int j = 0; j < subs.Count; j++)
                    {
                        subs[j].Visibility(Visibility.Hidden);
                    }

                    contentDragger = lis[i];
                    lis[i].Add(tracker);
                    lis[i].Visibility(Visibility.Hidden);
                    tracker.Visibility(Visibility.Visible);
                }
            } 

            var t = sl.Query("unity-drag-container").First();
            t.ElementAt(1).RemoveFromHierarchy();

            icon.AddChild(valueField);
            valueField.Bottom(Utk.ScreenRatio(30)).AlignSelf(Align.Center).Position(Position.Absolute);
            valueField.Visibility(Visibility.Visible);
            valueField.ElementAt(0).BcgColor("#0078d7");
            valueField.ElementAt(0).TextAlignment(TextAnchor.MiddleCenter);
        } 
    }
}