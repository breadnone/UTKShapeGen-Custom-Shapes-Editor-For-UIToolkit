using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.IO;
using UnityEditor;

namespace UTKShape
{
    [System.Serializable]
    public class UTKPropSettings
    {
        public string name;
        public string id;
        public UTKMesh utkMesh;
        public List<(Vector3 point, int root)> points = new();
        public Color fillColor = Color.white;
        public bool fill = false;
        public LineJoin lineJoin = LineJoin.Miter;
        public LineCap lineCap = LineCap.Round;
        public float mitterLimit = 0f;
        public bool closePath;
        public UTKDrawType drawType = UTKDrawType.Line;
        public Color lineColor = Color.white;
        public float lineRadius = 10f;
        public float curveRadius = 20f;
        public bool line = true;
        public FillRule fillRule;
    }
    [FilePath("UTKShapeGen/UTKShapeGen-conf.utkShapeGenConfig", FilePathAttribute.Location.PreferencesFolder)]
    public class UTKScriptableShapeGen : ScriptableSingleton<UTKScriptableShapeGen>
    {
        [SerializeField] public UTKPropSettings settings;
        [SerializeField] public List<UTKPropSettings>  profiles = new();
        public void SaveThis()
        {
            Save(true);
        }
    }
}