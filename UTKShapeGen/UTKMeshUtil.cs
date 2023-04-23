using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UTK;

namespace UTKShape
{
    [System.Serializable]
    public class UTKMeshProp
    {
        public string name;
        public Color fillColor;
        public Color lineColor;
        public LineCap lineCap;
        public LineJoin lineJoin;
        public LineAlignment lineAlignment;
        public FillRule fillRule;
        public float lineRadius = 5f;
        public float curveRadius = 20f;
        public Painter2D painter;
    }
    public static class UTKMeshUtil
    {
        public static UTKMesh CreateMesh(List<Vector3> paths, UTKMeshProp prop)
        {
            var newMesh = new UTKMesh(paths, prop);
            return newMesh;
        }
        public static void Draw<T>(this T mesh, MeshGenerationContext mgc) where T : UTKMesh
        {
            if (mesh.paths.Count == 0)
                return;

            mesh.meshProperty.painter = new Painter2D(); 

            var paint2D = mgc.painter2D;
            paint2D.strokeColor = mesh.meshProperty.lineColor;
            paint2D.fillColor = mesh.meshProperty.fillColor;
            paint2D.lineWidth = mesh.meshProperty.lineRadius;
            paint2D.lineCap = mesh.meshProperty.lineCap;
            paint2D.lineJoin = mesh.meshProperty.lineJoin;

            ///copy
            mesh.meshProperty.painter.strokeColor = paint2D.strokeColor;
            mesh.meshProperty.painter.fillColor = paint2D.fillColor;
            mesh.meshProperty.painter.lineWidth = paint2D.lineWidth;
            mesh.meshProperty.painter.lineCap = paint2D.lineCap;
            mesh.meshProperty.painter.lineJoin = paint2D.lineJoin;
            ///copy
            
            paint2D.BeginPath();
            mesh.meshProperty.painter.BeginPath();

            for (int i = 0; i < mesh.paths.Count; i++)
            {
                paint2D.LineTo(mesh.paths[i]);
                mesh.meshProperty.painter.LineTo(mesh.paths[i]);
            }

            paint2D.ClosePath();
            paint2D.Fill();
            paint2D.Stroke();   

            mesh.meshProperty.painter.ClosePath();
            mesh.meshProperty.painter.Fill();
            mesh.meshProperty.painter.Stroke();
        }
    }
}