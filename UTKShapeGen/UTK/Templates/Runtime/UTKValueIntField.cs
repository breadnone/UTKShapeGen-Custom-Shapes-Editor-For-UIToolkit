using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{

    public class UTKValueIntField : VisualElement, IUState<UTKValueIntField>, INotifyValueChanged<int>
    {
        public (Label upArrow, Label downArrow) containers;
        public IntegerField integerField{get;set;}
        private int _value;
        public int value
        {
            get => _value;
            set
            {
                if(_value == value)
                    return;

                using (ChangeEvent<int> evt = ChangeEvent<int>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }
        public void SetValueWithoutNotify(int value)
        {
            _value = value;
        }
        public UTKValueIntField(int initValue = 0)
        {
            this.Size(Utk.ScreenRatio(50) * 2f, Utk.ScreenRatio(50), false, false).FlexRow();
            var root = new VisualElement().FlexColumn().Size(30, 100, true, true).JustifyContent(Justify.Center);

            integerField = new IntegerField().Size(70, 100, true, true).SetOverflow(Overflow.Hidden);
            var cc  = integerField.Query("unity-text-input").First();
            cc.SetOverflow(Overflow.Hidden).RoundCorner(0).BcgColor(Color.white).Color(Color.black).Border(0, Color.clear).BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1"));

            integerField.SetValueWithoutNotify(initValue);

            var imgUp = new Label().Name("upArrow").FlexGrow(1).Text("<").TextAlignment(TextAnchor.MiddleCenter).Padding(0).Margin(0).BcgColor("#0063b1");
            TextElement u = imgUp.Query<TextElement>().First();
            u.Rotate(new Rotate(new Angle(90, AngleUnit.Degree))).TextAlignment(TextAnchor.MiddleCenter).Margin(0).Padding(0);

            var imgDown = new Label().Name("downArrow").FlexGrow(1).Text(">").TextAlignment(TextAnchor.MiddleCenter).Padding(0).Margin(0).BcgColor("#0063b1");
            TextElement d = imgDown.Query<TextElement>().First();
            d.Rotate(new Rotate(new Angle(90, AngleUnit.Degree))).TextAlignment(TextAnchor.MiddleCenter).Margin(0).Padding(0);

            containers = (imgUp, imgDown);
            this.AddChild(integerField as VisualElement);
            root.AddChild(imgUp).AddChild(imgDown);
            this.AddChild(root);
            Construct();
        }
        private void Construct()
        {
            integerField.OnValueChanged(x=>
            {
                value = x.newValue;
            });

            containers.upArrow.pickingMode = PickingMode.Position;
            containers.downArrow.pickingMode = PickingMode.Position;

            containers.upArrow.OnMouseDown(x=>
            {
                integerField.value += 1;

            }, TrickleDown.TrickleDown);

            containers.downArrow.OnMouseDown(x=>
            {
                integerField.value -= 1;

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