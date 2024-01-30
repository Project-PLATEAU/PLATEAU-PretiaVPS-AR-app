using UnityEngine;
using UnityEditor;
using System.Text;

namespace Pretia.RelocChecker.Editor.Exporters
{
    public class ObjExporter : AbstractExporter
    {
        
        [MenuItem("GameObject/Export To OBJ...", isValidateFunction: false, priority: 31)]
        private static void ExportToObj()
        {            
            var exporter = new ObjExporter();
            var gameObject = Selection.activeObject as GameObject;
            if (gameObject == null) return;
            
            var path = EditorUtility.SaveFilePanel("Export To OBJ", "", gameObject.name, "obj");
            exporter.Export(gameObject, path);
        }

        protected override string Serialize(string name, Mesh mesh)
        {
            var sb = new StringBuilder();

            foreach (var v in mesh.vertices)
                sb.Append($"v {v.x} {v.y} {v.z}\n");

            foreach (var v in mesh.normals)
                sb.Append($"vn {v.x} {v.y} {v.z}\n");

            for (var material = 0; material < mesh.subMeshCount; material++)
            {
                sb.Append($"\ng {name}\n");
                var triangles = mesh.GetTriangles(material);
                for (var i = 0; i < triangles.Length; i += 3)
                    sb.Append(
                        string.Format("f {0}/{0} {1}/{1} {2}/{2}\n",
                        triangles[i] + 1,
                        triangles[i + 1] + 1,
                        triangles[i + 2] + 1)
                    );
            }

            return sb.ToString();
        }
    }
}