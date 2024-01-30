using UnityEngine;
using UnityEditor;
using System.Text;

namespace Pretia.RelocChecker.Editor.Exporters
{
    public class PlyExporter : AbstractExporter
    {
        [MenuItem("GameObject/Export To PLY...", isValidateFunction: false, priority: 31)]
        private static void ExportToPly()
        {            
            var exporter = new PlyExporter();
            var gameObject = Selection.activeObject as GameObject;
            if (gameObject == null) return;
            
            var path = EditorUtility.SaveFilePanel("Export To PLY", "", gameObject.name, "ply");
            exporter.Export(gameObject, path);
        }

        protected override string Serialize(string name, Mesh mesh)
        {
            var sb = new StringBuilder();
            var vertices = mesh.vertices;
            var triangles = mesh.triangles;

            sb.AppendLine("ply");
            sb.AppendLine("format ascii 1.0");
            sb.AppendLine($"element vertex {vertices.Length}");
            sb.AppendLine("property float x");
            sb.AppendLine("property float y");
            sb.AppendLine("property float z");
            sb.AppendLine($"element face {triangles.Length / 3}");
            sb.AppendLine("property list uchar int vertex_index");
            sb.AppendLine("end_header");

            foreach (var v in vertices)
                sb.AppendLine($"{v.x} {v.y} {v.z}");

            for (var i = 0; i < triangles.Length; i += 3)
                sb.AppendLine($"3 {triangles[i]} {triangles[i+1]} {triangles[i+2]}");

            return sb.ToString();
        }
    }
}
