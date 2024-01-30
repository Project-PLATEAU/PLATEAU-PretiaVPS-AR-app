using System.IO;
using UnityEngine;

namespace Pretia.RelocChecker.Editor.Exporters
{
    public abstract class AbstractExporter
    {
        protected abstract string Serialize(string name, Mesh mesh);
        
        protected void Export(GameObject gameObject, string path)
        {
            if (gameObject == null)
            {
                Debug.LogWarning("No Game Object selected. Execution paused.");
                return;
            }
        
            var meshFilter = gameObject.GetComponent<MeshFilter>();
            Mesh sharedMesh = null;
            if (meshFilter == null)
            {
                Debug.LogWarning("No MeshFilter found in selected GameObject.", gameObject);
                var meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
                var combine = new CombineInstance[meshFilters.Length];

                for (var i = 0; i < meshFilters.Length; i++)
                {
                    combine[i].mesh = meshFilters[i].sharedMesh;
                    meshFilters[i].transform.localScale = new Vector3(-1, 1, 1);
                    combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                    meshFilters[i].transform.localScale = new Vector3(1, 1, 1);
                }

                var mesh = new Mesh();
                mesh.CombineMeshes(combine);
                sharedMesh = mesh;
            }
            else
            {
                sharedMesh = meshFilter.sharedMesh;
            }
        
            if (sharedMesh == null)
            {
                Debug.LogWarning("No mesh found in selected GameObject.", gameObject);
                return;
            }
        
            var writer = new StreamWriter(path);
            writer.Write(Serialize(gameObject.name, sharedMesh));
            writer.Close();
            
            Debug.Log($"The {gameObject.name} mesh has been successfully exported into {path}");
        }
    }
}