using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UTK;

namespace UTKShape
{
    public class GizmoRotate : VisualElement
    {
        public VisualElement leftHandle{get;set;}
        public VisualElement rightHandle{get;set;}
        private VisualElement bar;
        public GizmoRotate()
        {

        }
        private void Construct()
        {
            bar = new VisualElement().Size(Utk.ScreenRatio(100), Utk.ScreenRatio(5)).BcgColor(Color.red).JustifyContent(Justify.Center);
            this.AddChild(bar).BcgColor(Color.red);
            leftHandle = new VisualElement().Size(15, 15).AlignSelf(Align.FlexStart).BcgColor(Color.blue).Border(2, Color.yellow);
            rightHandle = new VisualElement().Size(15, 15).AlignSelf(Align.FlexEnd).BcgColor(Color.blue).Border(2, Color.yellow);
            bar.AddChild(leftHandle).AddChild(rightHandle);

            leftHandle.OnPointerDown(x=>
            {

            });

            rightHandle.OnPointerDown(x=>
            {

            });
        }
    }
}