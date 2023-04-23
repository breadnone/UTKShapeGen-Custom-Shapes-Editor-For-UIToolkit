#if UNITY_EDITOR

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;
    using System;
    using UnityEditor.UIElements;
    

namespace UTK
{
    public static class UTKContextManipulator
    {
        public static ColorField colorPicker(VisualElement parent, Func<VisualElement> visualElement)
        {
            var col = new ColorField();
            col.RegisterValueChangedCallback(x =>
            {

            });
            parent.AddChild(col);

            return col;
        }
    }
}
#endif