using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKPairFloatField : VisualElement, ITemplatePair<UTKValueFloatField>
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;}
        public UTKValueFloatField objectElement{get;set;}
        public UTKPairFloatField(string text = "FloatField :", float defaultValue = 0f)
        {
            objectElement = new UTKValueFloatField(defaultValue);
            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50));
            var iface = (ITemplatePair<UTKValueFloatField>)this;
            iface.Init(text);
            iface.InsertToContainer(objectElement);
        }
        public virtual void SetSpacing(float value)
        {
            var iface = (ITemplatePair<UTKValueFloatField>)this;
            iface.SetSpacing(value);
        }
    }
}