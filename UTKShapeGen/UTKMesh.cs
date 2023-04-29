using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UTK;

namespace UTKShape
{
    ///<summary>Line style. e.g: Line, Curve, Bezier.etc.</summary>
    [System.Serializable]
    public enum UTKDrawType
    {
        Line,
        Curve,
        Bezier,
        Text,
        Circle,
        Triangle
    }
    ///<summary>Class to contain vector api.</summary>
    public class UTKMesh : VisualElement
    {
        ///<summary>Vector coordinates used to paint via Paint2D.</summary>
        public List<Vector3> paths { get; set; }
        public UTKMeshProp meshProperty{get;set;}
        private UTKDrawType drawType;
        public MeshGenerationContext utkContext{get;set;}

        public UTKMesh(List<Vector3> paths, UTKMeshProp prop)
        {
            this.drawType = prop.drawType;
            meshProperty = prop;
            this.paths = paths;
            (this as VisualElement).JustifyContent(Justify.Center);
            generateVisualContent += OnGenerateVisualContent;
        }

        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            utkContext = mgc;

            if(drawType == UTKDrawType.Line)
            {
                this.DrawLinePath();
            }
            else if(drawType == UTKDrawType.Curve)
            {
                this.DrawCurvePath();
            }
            else if(drawType == UTKDrawType.Bezier)
            {
                this.DrawBezierPath();
            }
            else if(drawType == UTKDrawType.Text)
            {

            }
            else if(drawType == UTKDrawType.Circle)
            {
                this.DrawCircle(meshProperty.startingPoint, 50f);
            }
            else if(drawType == UTKDrawType.Triangle)
            {
                this.DrawTriangle(meshProperty.startingPoint);
            }
        }
    }
}
