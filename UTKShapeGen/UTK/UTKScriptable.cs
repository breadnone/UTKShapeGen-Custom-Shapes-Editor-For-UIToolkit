using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UTK;

namespace UTK
{
    [System.Serializable]
    public class UTKScriptable : ScriptableObject
    {
        public List<UTKTemplate> Template = new List<UTKTemplate>();
    }
    [System.Serializable]
    public sealed class UTKTemplate 
    {
        public string templateName;
        public VisualElement template;
    }
}