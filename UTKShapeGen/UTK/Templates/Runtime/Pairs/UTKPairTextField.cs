using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKPairTextField : VisualElement, ITemplatePair<UTKTextField>
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;}
        public UTKTextField objectElement{get;set;}
        public UTKPairTextField(string text = "TextField :", string defaultEmptyText = "<i>Enter text...</i>")
        {
            objectElement = new UTKTextField(defaultEmptyText);
            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50));
            var iface = (ITemplatePair<UTKTextField>)this;
            iface.Init(text);
            iface.InsertToContainer(objectElement);

        }
        public virtual void SetSpacing(float value)
        {
            var iface = (ITemplatePair<UTKTextField>)this;
            iface.SetSpacing(value);
        }
    }
}