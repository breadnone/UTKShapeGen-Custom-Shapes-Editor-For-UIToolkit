using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKTextField : VisualElement, INotifyValueChanged<string>, IUState<UTKTextField>
    {
        public Label label;
        public TextField textField;
        public bool isTextArea{get;set;}
        public float _scalerMultiplier{get;set;} = 60;
        public bool isNumeric{get;set;}
        private string _defaultText = "<i>Enter text...</i>";
        public string defaultText
        {
            get
            {
                return _defaultText;
            }
            set
            {
                if(_defaultText == value)
                    return;

                _defaultText = value;
                label.text = value;
            }
        }
        public float scalerSize 
        {
            get => Utk.ScreenRatio(_scalerMultiplier);
            set
            {
                _scalerMultiplier = value;
                Construct();
            }
        }
        private string _value;
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
                }
            }
        }
        public UTKTextField(string defaultText = "")
        {
            this.textField = new TextField().Size(100, 100, true, true).Position(Position.Absolute).Display(DisplayStyle.None);
            this.JustifyContent(Justify.Center).Size(scalerSize * 6, scalerSize, false, false);
            var el = new Label().Text(defaultText).Size(100, 100, true, true).BorderBottom(Utk.ScreenRatio(7), Utk.HexColor(Color.clear, "#0063b1")).Position(Position.Absolute);
            this.AddChild(el as VisualElement).AddChild(textField);
            label = el;
            label.SetOverflow(Overflow.Hidden);
            label.TextAlignment(TextAnchor.MiddleLeft).PaddingLeft(Utk.ScreenRatio(5)).PaddingRight(Utk.ScreenRatio(5));
            Construct();
        }
        public void ClearText()
        {
            textField.value = string.Empty;
            label.text = string.Empty;
        }
        private void Construct()
        {
            label.OnMouseDown(x=>
            {
                textField.Display(DisplayStyle.Flex);
                label.Display(DisplayStyle.None);
                textField.schedule.Execute(()=>textField.Focus());
            });

            textField.OnFocusOut(x=>
            {
                textField.Display(DisplayStyle.None);
                label.Display(DisplayStyle.Flex);

                if(String.IsNullOrEmpty(textField.value))
                {
                    label.text = defaultText;
                }
                else
                {
                    label.text = textField.value;
                }
            });

            textField.OnValueChanged(x=>
            {
                if(!isNumeric)
                {
                    value = x.newValue;
                }
                else
                {
                    if(!String.IsNullOrEmpty(x.newValue) &&  char.IsDigit(x.newValue[x.newValue.Length - 1]))
                    {
                        value = x.newValue;
                    }
                }
            });

            label.Text(defaultText);
        }

        public void SetValueWithoutNotify(string value)
        {
            _value = value;
            textField.value = value;
        }
        public virtual void OnEnable()
        {

        }
        public virtual void OnDisable()
        {

        }
    }
}