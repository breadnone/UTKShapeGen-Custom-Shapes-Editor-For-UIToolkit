using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{

    public class UTKValueFloatField : VisualElement, IUState<UTKValueFloatField>, INotifyValueChanged<float>
    {
        public (Label upArrow, Label downArrow) containers;
        public FloatField floatField{get;set;}
        private float _value;
        public float value
        {
            get => _value;
            set
            {
                if(_value == value)
                    return;

                using (ChangeEvent<float> evt = ChangeEvent<float>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }
        public void SetValueWithoutNotify(float value)
        {
            _value = value;
            floatField.value = value;
        }
        public UTKValueFloatField(float initValue = 0f)
        {
            this.Size(Utk.ScreenRatio(50) * 2f, Utk.ScreenRatio(50), false, false).FlexRow();
            var root = new VisualElement().FlexColumn().Size(30, 100, true, true).JustifyContent(Justify.Center).FlexGrow(1);

            floatField = new FloatField().Size(70, 100, true, true).SetOverflow(Overflow.Hidden);
            var cc  = floatField.Query("unity-text-input").First();
            cc.SetOverflow(Overflow.Hidden).RoundCorner(0).BcgColor(Color.white).Color(Color.black).Border(0, Color.clear).BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1"));

            floatField.SetValueWithoutNotify(initValue);

            var imgUp = new Label().Name("upArrow").FlexGrow(1).Text("<").TextAlignment(TextAnchor.MiddleCenter).Padding(0).Margin(0).BcgColor("#0063b1");
            TextElement u = imgUp.Query<TextElement>().First();
            u.Rotate(new Rotate(new Angle(90, AngleUnit.Degree))).TextAlignment(TextAnchor.MiddleCenter).Margin(0).Padding(0);

            var imgDown = new Label().Name("downArrow").FlexGrow(1).Text(">").TextAlignment(TextAnchor.MiddleCenter).Padding(0).Margin(0).BcgColor("#0063b1");
            TextElement d = imgDown.Query<TextElement>().First();
            d.Rotate(new Rotate(new Angle(90, AngleUnit.Degree))).TextAlignment(TextAnchor.MiddleCenter).Margin(0).Padding(0);

            containers = (imgUp, imgDown);
            this.AddChild(floatField as VisualElement);
            root.AddChild(imgUp).AddChild(imgDown);
            this.AddChild(root);
            Construct();
        }
        private void Construct()
        {
            floatField.OnValueChanged(x=>
            {
                value = x.newValue;
            });

            containers.upArrow.pickingMode = PickingMode.Position;
            containers.downArrow.pickingMode = PickingMode.Position;

            containers.upArrow.OnMouseDown(x=>
            {
                floatField.value += 0.01f;

            }, TrickleDown.TrickleDown);

            containers.downArrow.OnMouseDown(x=>
            {
                floatField.value -= 0.01f;

            }, TrickleDown.TrickleDown);

        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
    }
}