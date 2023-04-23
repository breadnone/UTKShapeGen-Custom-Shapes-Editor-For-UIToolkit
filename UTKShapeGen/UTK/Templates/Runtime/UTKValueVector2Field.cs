using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{

    public class UTKValueVector2Field : VisualElement, IUState<VisualElement>, INotifyValueChanged<Vector2>
    {
        public (Label upArrow, Label downArrow)[] containers { get; set; } = new[] { (new Label(), new Label()), (new Label(), new Label())};
        public FloatField[] floatField { get; set; } = new FloatField[] { new FloatField(), new FloatField()};
        private Vector2 _value;
        public Vector2 value
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;

                using (ChangeEvent<Vector2> evt = ChangeEvent<Vector2>.GetPooled(_value, value))
                {
                    evt.target = this;
                    SetValueWithoutNotify(value);
                    SendEvent(evt);
                }
            }
        }
        public void SetValueWithoutNotify(Vector2 value)
        {
            _value = value;

            for (int i = 0; i < floatField.Length; i++)
            {
                if (i == 0)
                    floatField[i].value = value.x;
                else if (i == 1)
                    floatField[i].value = value.y;
            }
        }
        public UTKValueVector2Field()
        {
            this.Size(Utk.ScreenRatio(50) * 4f, Utk.ScreenRatio(50), false, false).FlexRow();
            var siz = (float)100 / 3F;

            for (int i = 0; i < floatField.Length; i++)
            {
                floatField[i].Size(siz, 100, true, true).SetOverflow(Overflow.Hidden).MarginRight(10, true);
                var cc = floatField[i].Query("unity-text-input").First();
                cc.SetOverflow(Overflow.Hidden).RoundCorner(0).BcgColor(Color.white).Color(Color.black).Border(0, Color.clear).BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1"));
                this.AddChild(floatField[i] as VisualElement);
                Construct(floatField[i], i);               
            }
        }
        private void Construct(FloatField floatField, int i)
        {
            floatField.OnValueChanged(x =>
            {
                if (i == 0)
                {
                    value = new Vector2(x.newValue, value.y);
                }
                else if (i == 1)
                {
                    value = new Vector2(value.x, x.newValue);
                }
            });
        }
        public void BackgroundColor(Color color)
        {
            for (int i = 0; i < floatField.Length; i++)
            {
                var cc = floatField[i].Query("unity-text-input").First();

                if (cc != null)
                {
                    cc.BcgColor(color);
                }
            }
        }
        public void TextColor(Color color)
        {
            for (int i = 0; i < floatField.Length; i++)
            {
                var cc = floatField[i].Query("unity-text-input").First();

                if (cc != null)
                {
                    cc.Color(color);
                }
            }
        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
    }
}