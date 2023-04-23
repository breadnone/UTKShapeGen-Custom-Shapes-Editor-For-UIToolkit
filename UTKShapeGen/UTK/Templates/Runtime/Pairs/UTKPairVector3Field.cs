using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKPairVector3Field : VisualElement, ITemplatePair<UTKValueVector3Field>
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;}
        public UTKValueVector3Field objectElement{get;set;}
        public UTKPairVector3Field(string text = "Vector3Field :")
        {
            objectElement = new UTKValueVector3Field();
            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50));
            var iface = (ITemplatePair<UTKValueVector3Field>)this;
            iface.Init(text);
            iface.InsertToContainer(objectElement);
        }
        public virtual void SetSpacing(float value)
        {
            var iface = (ITemplatePair<UTKValueVector3Field>)this;
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