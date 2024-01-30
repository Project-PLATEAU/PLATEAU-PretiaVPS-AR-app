using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Pretia.RelocChecker.Editor.Exporters
{
    public class ObjToPlyConverter
    {
        [MenuItem("Assets/Convert To Ply...", isValidateFunction: false, priority: 31)]
        private static void ExportToObj()
        {
            var exporter = new ObjToPlyConverter();
            var obj = Selection.activeObject;
            if (obj == null) return;

            var objPath = Path.GetFullPath(AssetDatabase.GetAssetPath(obj));
            var plyPath = EditorUtility.SaveFilePanel("Convert To Ply...", "", obj.name, "ply");
            exporter.ConvertObjToPly(objPath, plyPath);
        }

        private void ConvertObjToPly(string objPath, string plyPath)
        {
            var objLines = File.ReadAllLines(objPath);

            var vertices = new List<Vector3>();
            var faces = new List<int>();

            // Parse OBJ data
            foreach (var line in objLines)
            {
                var vertexData = line.Substring(2).Split(' ');
                if (line.StartsWith("v "))
                {
                    var vertex = new Vector3(
                        float.Parse(vertexData[0]),
                        float.Parse(vertexData[1]),
                        float.Parse(vertexData[2]));
                    vertices.Add(vertex);
                }
                else if (line.StartsWith("f "))
                {
                    var faceData = line.Substring(2).Split(' ');
                    foreach (var vertex in faceData)
                    {
                        faces.Add(int.Parse(vertex.Split('/')[0]));
                    }
                }
            }

            // Write PLY file in binary format
            using (FileStream fs = new FileStream(plyPath, FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("ply");
                sw.WriteLine("format binary_little_endian 1.0");
                sw.WriteLine($"element vertex {vertices.Count}");
                sw.WriteLine("property float x");
                sw.WriteLine("property float y");
                sw.WriteLine("property float z");
                sw.WriteLine($"element face {faces.Count / 3}");
                sw.WriteLine("property list uchar int vertex_indices");
                sw.WriteLine("end_header");
                sw.Flush(); // Ensure the header is written to file

                // Write vertices
                foreach (Vector3 vertex in vertices)
                {
                    fs.Write(BitConverter.GetBytes(vertex.x), 0, sizeof(float));
                    fs.Write(BitConverter.GetBytes(vertex.y), 0, sizeof(float));
                    fs.Write(BitConverter.GetBytes(vertex.z), 0, sizeof(float));
                }

                // Write faces
                for (int i = 0; i < faces.Count; i += 3)
                {
                    fs.WriteByte(3); // Number of vertices in the face
                    fs.Write(BitConverter.GetBytes(faces[i] - 1), 0, sizeof(int));
                    fs.Write(BitConverter.GetBytes(faces[i + 1] - 1), 0, sizeof(int));
                    fs.Write(BitConverter.GetBytes(faces[i + 2] - 1), 0, sizeof(int));
                }
            }

            Debug.Log("OBJ to PLY conversion completed.");
        }
    }
}