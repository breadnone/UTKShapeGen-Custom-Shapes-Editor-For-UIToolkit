using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKPairVector2Field : VisualElement, ITemplatePair<UTKValueVector2Field>
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;}
        public UTKValueVector2Field objectElement{get;set;}
        public UTKPairVector2Field(string text = "Vector2Field :")
        {
            objectElement = new UTKValueVector2Field();
            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50));
            var iface = (ITemplatePair<UTKValueVector2Field>)this;
            iface.Init(text);
            iface.InsertToContainer(objectElement);
        }
        public virtual void SetSpacing(float value)
        {
            var iface = (ITemplatePair<UTKValueVector2Field>)this;
            iface.SetSpacing(value);
        }
        public void SetBackgroundColor(Color color)
        {
            objectElement.BackgroundColor(color);
        }
        public void TextColor(Color color)
        {
            objectElement.TextColor(color);
        }
    }
}