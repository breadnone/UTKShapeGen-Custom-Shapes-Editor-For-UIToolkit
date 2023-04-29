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
        public float mitterLimit;
        public bool closePath = true;
        public float lineRadius = 20f;
        public float curveRadius = 20f;
        public Painter2D painter;
        public UTKDrawType drawType = UTKDrawType.Line;
        public Vector2 startingPoint; //mostly used for premad shapes
    }
    public static class UTKMeshUtil
    {
        public static UTKMesh CreateMesh(List<Vector3> paths, UTKMeshProp prop)
        {
            var newMesh = new UTKMesh(paths, prop);
            return newMesh;
        }
        public static void DrawRoundPath<T>(this T mesh) where T : UTKMesh
        {
            if (mesh.paths.Count == 0)
                return;

            mesh.meshProperty.painter = new Painter2D(); 
            var paint2D = mesh.utkContext.painter2D;
            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
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

        public static void DrawLinePath<T>(this T mesh) where T : UTKMesh
        {
            if (mesh.paths.Count == 0)
                return;

            mesh.meshProperty.painter = new Painter2D(); 

            var paint2D = mesh.utkContext.painter2D;
            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
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
        public static void DrawCurvePath<T>(this T mesh) where T : UTKMesh
        {
            if (mesh.paths.Count == 0)
                return;

            mesh.meshProperty.painter = new Painter2D(); 
            var paint2D = mesh.utkContext.painter2D;

            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
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
            Vector2 center = Vector2.zero;

            for (int i = 0; i < mesh.paths.Count; i++)
            {
                if(mesh.paths.Count > 2)
                {
                    if(i == 0)
                    {
                        center = Vector3.Lerp(mesh.paths[i], mesh.paths[mesh.paths.Count - 1], 0.5f - (mesh.meshProperty.curveRadius * 0.001f));
                        paint2D.MoveTo(center);
                        mesh.meshProperty.painter.MoveTo(center);
                        paint2D.ArcTo(mesh.paths[i], center, mesh.meshProperty.curveRadius);
                        mesh.meshProperty.painter.ArcTo(mesh.paths[i], center, mesh.meshProperty.curveRadius);
                    }

                    if(i + 1 < mesh.paths.Count)
                    {
                        paint2D.ArcTo(mesh.paths[i], mesh.paths[i + 1], mesh.meshProperty.curveRadius);
                        mesh.meshProperty.painter.ArcTo(mesh.paths[i], mesh.paths[i + 1], mesh.meshProperty.curveRadius);
                    }

                    if(i == mesh.paths.Count - 1)
                    {
                        paint2D.ArcTo(mesh.paths[i], center, mesh.meshProperty.curveRadius);
                        mesh.meshProperty.painter.ArcTo(mesh.paths[i], center, mesh.meshProperty.curveRadius);
                        //paint2D.MoveTo(center);
                        paint2D.LineTo(center);
                        mesh.meshProperty.painter.LineTo(center);

                        //TODO: Workround
                        paint2D.ArcTo(center, center, mesh.meshProperty.curveRadius);
                        mesh.meshProperty.painter.ArcTo(center, center, mesh.meshProperty.curveRadius);
                    }
                }
                else
                {
                    paint2D.LineTo(mesh.paths[i]);
                    mesh.meshProperty.painter.LineTo(mesh.paths[i]);
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

        public static void DrawBezierPath<T>(this T mesh) where T : UTKMesh
        {
            if (mesh.paths.Count == 0)
                return;

            mesh.meshProperty.painter = new Painter2D(); 
            var paint2D = mesh.utkContext.painter2D;

            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
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
                if(mesh.paths.Count > 1)
                {
                    if(i + 1 < mesh.paths.Count)
                    {
                        Vector2 mid = Vector2.Lerp(mesh.paths[i], mesh.paths[i + 1], 0.5f); 
                        var xMin = mesh.paths[i].x > mesh.paths[i+1].x ? -50f : 50;
                        var xMax = mesh.paths[i].x > mesh.paths[i + 1].x ? 50:-50;
                        xMin = mesh.paths[i].x == mesh.paths[i + 1].x ? 0 : xMin;
                        xMax = xMax == 0 ? 0 : xMax;

                        if(mesh.paths[i].y < mesh.paths[i + 1].y && xMin < 0)
                        {
                            xMin = xMin * -1;
                        }

                        if(mesh.paths[i + 1].x < mesh.paths[i].x && xMin > 0)
                        {
                            xMin = xMin * -1;
                        }

                        if(i == 0)
                            paint2D.MoveTo(mesh.paths[i]);

                        if(i < mesh.paths.Count - 2)
                        {
                            paint2D.BezierCurveTo(new Vector2(mesh.paths[i].x + xMin, mesh.paths[i].y), new Vector2(mid.x, mid.y + 50f), new Vector2(mesh.paths[i + 1].x, mesh.paths[i + 1].y));
                        }
                    }
                }
                else
                {
                    paint2D.LineTo(mesh.paths[i]);
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
        public static void DrawLine<T>(this T mesh, Vector3 source, Vector3 target, bool moveToSource) where T : UTKMesh
        {
            mesh.meshProperty.painter = new Painter2D(); 
            var paint2D = mesh.utkContext.painter2D;

            paint2D.strokeColor = mesh.meshProperty.lineColor;
            paint2D.fillColor = mesh.meshProperty.fillColor;
            paint2D.lineWidth = mesh.meshProperty.lineRadius;
            paint2D.lineCap = mesh.meshProperty.lineCap;
            paint2D.lineJoin = mesh.meshProperty.lineJoin;

            paint2D.BeginPath();    

            if(moveToSource)         
                paint2D.MoveTo(source);

            paint2D.LineTo(target);

            if(mesh.meshProperty.closePath)
            {
                paint2D.ClosePath();
                mesh.meshProperty.painter.ClosePath();
            }
            
            paint2D.Stroke(); 
            paint2D.Fill(mesh.meshProperty.fillRule); 
        }
        public static UTKMesh DrawCurve<T>(this T mesh, Vector3 source, Vector3 target, float radius) where T : UTKMesh
        {
            mesh.meshProperty.painter = new Painter2D(); 
            
            var paint2D = mesh.utkContext.painter2D;
            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
            paint2D.strokeColor = mesh.meshProperty.lineColor;
            paint2D.fillColor = mesh.meshProperty.fillColor;
            paint2D.lineWidth = mesh.meshProperty.lineRadius;
            paint2D.lineCap = mesh.meshProperty.lineCap;
            paint2D.lineJoin = mesh.meshProperty.lineJoin;

            paint2D.BeginPath();
            paint2D.MoveTo(source);
            paint2D.ArcTo(source, target, radius);

            mesh.utkContext.painter2D.MoveTo(source);
            mesh.utkContext.painter2D.LineTo(target);

            if(mesh.meshProperty.closePath)
            {
                paint2D.ClosePath();
                mesh.meshProperty.painter.ClosePath();
            }

            paint2D.Stroke(); 
            paint2D.Fill(mesh.meshProperty.fillRule); 
            return mesh;
        }
        public static UTKMesh DrawText<T>(this T mesh, string text, Vector2 startPosition, float fontSize, Color color, UnityEngine.TextCore.Text.FontAsset font) where T : UTKMesh
        {
            mesh.utkContext.DrawText(text, startPosition, fontSize, color, font);
            return mesh;
        }    
        public static UTKMesh ClearResources<T>(this T mesh) where T : UTKMesh
        {
            mesh.utkContext.painter2D.Clear();
            mesh.utkContext.painter2D.Dispose();
            return mesh;
        }
        public static UTKMesh DrawTriangle<T>(this T mesh, Vector2 startingPoint, float width = 300) where T : UTKMesh
        {
            var paint2D = new Painter2D();
            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
            paint2D.strokeColor = mesh.meshProperty.lineColor;
            paint2D.fillColor = mesh.meshProperty.fillColor;
            paint2D.lineWidth = mesh.meshProperty.lineRadius;
            paint2D.lineCap = mesh.meshProperty.lineCap;
            paint2D.lineJoin = mesh.meshProperty.lineJoin;

            var center = Vector2.Lerp(startingPoint, new Vector2(startingPoint.x + width, startingPoint.y), 0.5f);
            paint2D.BeginPath();    
            paint2D.MoveTo(startingPoint);
            paint2D.LineTo(new Vector2(startingPoint.x + width, startingPoint.y));
            paint2D.LineTo(new Vector2(width/2f, center.y + width));
            paint2D.LineTo(startingPoint);

            if(mesh.meshProperty.closePath)
                paint2D.ClosePath();

            paint2D.Stroke();
            paint2D.Fill(mesh.meshProperty.fillRule); 
            return mesh;
        }
        public static UTKMesh DrawCircle<T>(this T mesh, Vector2 startingPoint, float circleRadius) where T : UTKMesh
        {
            var paint2D = mesh.utkContext.painter2D;
            paint2D.miterLimit = mesh.meshProperty.mitterLimit;
            paint2D.strokeColor = mesh.meshProperty.lineColor;
            paint2D.fillColor = mesh.meshProperty.fillColor;
            paint2D.lineWidth = mesh.meshProperty.lineRadius;
            paint2D.lineCap = mesh.meshProperty.lineCap;
            paint2D.lineJoin = mesh.meshProperty.lineJoin;

            paint2D.BeginPath();
            paint2D.Arc(new Vector2(100, 100), 50, 0f, 360f);
            paint2D.Stroke(); 
            paint2D.Fill(mesh.meshProperty.fillRule); 
            return mesh;
        }
        public static UTKMesh CopyPainter2D<T>(this T mesh, Painter2D target) where T: UTKMesh
        {
            return mesh;
        }
    }
}