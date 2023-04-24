using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTKShape
{
    [System.Serializable]
    public enum UTKDrawType
    {
        Line,
        Curve
    }
    public class UTKMesh : VisualElement
    {
        public List<Vector3> paths { get; set; }
        public UTKMeshProp meshProperty{get;set;}
        private UTKDrawType drawType;

        public UTKMesh(List<Vector3> paths, UTKMeshProp prop, UTKDrawType drawType)
        {
            if(paths == null || paths.Count == 0)
                throw new Exception("UTKShape : Line paths can't be empty/null!");
            this.drawType = drawType;

            meshProperty = prop;
            this.paths = paths;
            generateVisualContent += OnGenerateVisualContent;
        }
        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if(drawType == UTKDrawType.Line)
            {
                this.Draw(mgc);
            }
            else if(drawType == UTKDrawType.Curve)
            {
                this.DrawRoundPath(mgc);
            }
        }
    }
}
