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
        public FillRule fillRule;
        public bool closePath = true;
        public float lineRadius = 20f;
        public float curveRadius = 20f;
        public Painter2D painter;
    }
    public static class UTKMeshUtil
    {
        public static UTKMesh CreateMesh(List<Vector3> paths, UTKMeshProp prop, UTKDrawType drawType)
        {
            var newMesh = new UTKMesh(paths, prop, drawType);
            return newMesh;
        }
        public static void DrawRoundPath<T>(this T mesh, MeshGenerationContext mgc) where T : UTKMesh
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
                if(i == 0 && mesh.paths.Count > 2)
                {
                    float sum = mesh.paths[i].x >= mesh.paths[mesh.paths.Count - 1].x ? -mesh.meshProperty.curveRadius : mesh.meshProperty.curveRadius;
                    paint2D.LineTo(mesh.paths[i]);
                    mesh.meshProperty.painter.MoveTo(new Vector2(mesh.paths[i].x + sum, mesh.paths[i].y));
                }

                if(i + 1 < mesh.paths.Count)
                {
                    paint2D.ArcTo(mesh.paths[i], mesh.paths[i + 1], mesh.meshProperty.curveRadius);
                    mesh.meshProperty.painter.ArcTo(mesh.paths[i], mesh.paths[i + 1], mesh.meshProperty.curveRadius);
                }
                
                if(i == mesh.paths.Count - 1 && mesh.paths.Count > 2)
                {
                    paint2D.ArcTo(mesh.paths[i], mesh.paths[0], mesh.meshProperty.curveRadius);
                    mesh.meshProperty.painter.ArcTo(mesh.paths[i], mesh.paths[0], mesh.meshProperty.curveRadius);                 
                }
            }

            if(mesh.meshProperty.closePath)
            {
                paint2D.ClosePath();
                mesh.meshProperty.painter.ClosePath();
            }
            
            paint2D.Stroke(); 
            paint2D.Fill(mesh.meshProperty.fillRule); 

            mesh.meshProperty.painter.Stroke();
            mesh.meshProperty.painter.Fill(mesh.meshProperty.fillRule);  
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

            if(mesh.meshProperty.closePath)
            {
                paint2D.ClosePath();
                mesh.meshProperty.painter.ClosePath();
            }

            paint2D.Fill(mesh.meshProperty.fillRule);
            paint2D.Stroke();   

            mesh.meshProperty.painter.Fill(mesh.meshProperty.fillRule);
            mesh.meshProperty.painter.Stroke();
        }
        public static void DrawBezierPath<T>(this T mesh, MeshGenerationContext mgc) where T : UTKMesh
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
                if(i == 0 && mesh.paths.Count > 2)
                {
                    paint2D.LineTo(mesh.paths[i]); 
                    mesh.meshProperty.painter.LineTo(mesh.paths[i]);
                }

                if(i + 2 < mesh.paths.Count)
                {
                    paint2D.BezierCurveTo(mesh.paths[i], mesh.paths[i + 1], mesh.paths[i + 2]);
                    mesh.meshProperty.painter.BezierCurveTo(mesh.paths[i], mesh.paths[i + 1], mesh.paths[i + 2]);
                }
            }
            
            if(mesh.meshProperty.closePath)
            {
                paint2D.ClosePath();
                mesh.meshProperty.painter.ClosePath();
            }
            
            paint2D.Stroke(); 
            paint2D.Fill(mesh.meshProperty.fillRule); 

            mesh.meshProperty.painter.Stroke();
            mesh.meshProperty.painter.Fill(mesh.meshProperty.fillRule);  
        }

    }
}