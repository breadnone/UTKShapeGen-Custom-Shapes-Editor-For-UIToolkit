using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UTKShape
{
    public class UTKMesh : VisualElement
    {
        public List<Vector3> paths { get; set; }
        public UTKMeshProp meshProperty{get;set;}

        public UTKMesh(List<Vector3> paths, UTKMeshProp prop)
        {
            if(paths == null || paths.Count == 0)
                throw new Exception("UTKShape : Line paths can't be empty/null!");

            meshProperty = prop;
            this.paths = paths;
            generateVisualContent += OnGenerateVisualContent;
        }
        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            this.Draw(mgc);
        }
    }
}
