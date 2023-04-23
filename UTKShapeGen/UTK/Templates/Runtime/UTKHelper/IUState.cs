using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTK
{
    public interface IUState<T> where T: VisualElement
    {
        public void UTKInit()
        {
            (this as VisualElement).OnGeometryChanged(evt =>
            {
                if (evt.newRect == Rect.zero)
                {
                    // "Likely", DisplayStyle was set to None
                    OnDisable();
                }
                else if(evt.oldRect == Rect.zero)
                {
                    // "Likely", DisplayStyle was set to None and is back to Flex
                    OnEnable();
                }
            });
        }
        public void OnEnable();
        public void OnDisable();
    }
}