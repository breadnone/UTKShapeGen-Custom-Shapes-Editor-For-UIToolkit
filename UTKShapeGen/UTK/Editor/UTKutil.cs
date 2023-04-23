using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;

namespace UTK
{
    [InitializeOnLoad]
    public class Startup
    {
    }
    [InitializeOnLoadAttribute]
    public static class HierarchyMonitor
    {
        static HierarchyMonitor()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        static void OnHierarchyChanged()
        {
            //UTKutil.CreateUTKAsset();
        }
    }
    public static class UTKutil
    {
        private static string upath = "Assets/UTK/Resources/utk-hn-template.asset";
        public static UTKScriptable Template{get;set;}
        public static void CreateUTKAsset()
        {
            if (File.Exists(upath))
            {
                Template = GetScriptableTemplate;
                return;
            }

            var styleAsset = ScriptableObject.CreateInstance<UTKScriptable>();
            AssetDatabase.CreateAsset(styleAsset, upath);
            AssetDatabase.SaveAssets();
            Template = GetScriptableTemplate;
        }
        public static UTKScriptable GetScriptableTemplate
        {
            get
            {
                var tmp = AssetDatabase.LoadAssetAtPath<UTKScriptable>(upath);

                if (tmp != null)
                    return tmp;
                return null;
            }
        }
    }
}