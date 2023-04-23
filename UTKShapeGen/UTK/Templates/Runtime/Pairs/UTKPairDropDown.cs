using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public class UTKPairDropDown : VisualElement, ITemplatePair<UTKDropDown>
    {
        public Label title{get;set;}
        public VisualElement rightContainer{get;set;}
        public UTKDropDown objectElement{get;set;}
        public UTKPairDropDown(string dropdownValue = "Select", string text = "DropDown :")
        {
            objectElement = new UTKDropDown(dropdownValue);
            objectElement.Size(100, 100, true, true).AlignSelf(Align.Center);
            this.Size(Utk.ScreenRatio(50) * 6, Utk.ScreenRatio(50));

  

            var iface = (ITemplatePair<UTKDropDown>)this;
            iface.Init(text);
            iface.InsertToContainer(objectElement);

        }
        public virtual void SetSpacing(float value)
        {
            var iface = (ITemplatePair<UTKDropDown>)this;
            iface.SetSpacing(value);
        }
    }
}