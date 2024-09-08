using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeshMaterialTextureCombiner : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] Vector2Int _atlasSize = new Vector2Int(2048, 2048);
    [SerializeField] int _maxAtlasSize = 2048;
    [SerializeField] int _padding = 0;
    [SerializeField] string _textureId = "_MainTex";

    [Header("Save")]
    [SerializeField] string _outputPath = "Art/Textures/supermarket/optimized/1";
    [SerializeField] string _meshFolderName = "meshes";
    [SerializeField] string _meshName = "mesh_";
    [SerializeField] string _textureName = "texture";
    [SerializeField] string _materialName = "material";
    [SerializeField] List<MeshRenderer> _meshRenderers;
    public List<MeshRenderer> MeshRenderers { get { return _meshRenderers; } }

    [Header("Result")]
    [SerializeField] Texture2D _result;
    [SerializeField] Material _resultMaterial;
    [SerializeField] List<Mesh> _resultMeshes;
    [SerializeField] List<GameObject> _resultObjects;


#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] bool _drawGizmos = true;
    [SerializeField] bool _drawMesh = true;
    [SerializeField] Color _gizmoColor = Color.red;
    Dictionary<MeshRenderer, Mesh> _caches = new Dictionary<MeshRenderer, Mesh>();

    void OnDrawGizmos()
    {
        if(!_drawGizmos) return;
        Gizmos.color = _gizmoColor;
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            Gizmos.DrawLine(transform.position, meshRenderer.transform.position);
        }


        if(!_drawMesh) return;
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            if(!_caches.ContainsKey(meshRenderer)) {
                _caches.Add(meshRenderer, meshRenderer.GetComponent<MeshFilter>().sharedMesh);
            }

            Gizmos.DrawMesh(_caches[meshRenderer], meshRenderer.transform.position, meshRenderer.transform.rotation, meshRenderer.transform.localScale);
        }
    }
#endif
    

    public void Combine() // Only support 1 material per renderer for now
    {
        if(!IsValid()) return;
        _resultObjects.Clear();

        Texture2D[] texture2Ds = new Texture2D[_meshRenderers.Count];
        for(int i = 0; i < _meshRenderers.Count; i++)
        {
            MeshRenderer mr = _meshRenderers[i];
            Texture2D tex = mr.sharedMaterial.GetTexture(_textureId) as Texture2D;
            texture2Ds[i] = tex;
        }

        Texture2D atlas = new Texture2D(_atlasSize.x, _atlasSize.y);
        Rect[] rects = PackTextures(atlas, texture2Ds, _padding, _maxAtlasSize);
        

        var firstMat = _meshRenderers[0].sharedMaterial;
        _resultMaterial = new Material(firstMat.shader);
        List<Mesh> newMeshes = new List<Mesh>();
        List<MeshFilter> newMeshFilters = new List<MeshFilter>();
        List<MeshRenderer> newMeshRenderers = new List<MeshRenderer>();

        for(int i = 0; i < _meshRenderers.Count; i++)
        {
            MeshRenderer meshRenderer = _meshRenderers[i];
            Mesh mesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
            Mesh newMesh = Instantiate(mesh);
            Rect rectNormalized = rects[i];

            Rect rect = new Rect(rectNormalized.x * _maxAtlasSize, rectNormalized.y * _maxAtlasSize, rectNormalized.width * _maxAtlasSize, rectNormalized.height * _maxAtlasSize);
            AdjustUVs(newMesh, rect, _maxAtlasSize, _maxAtlasSize);

            var newObj = Instantiate(meshRenderer.gameObject, transform);
            newObj.transform.position = meshRenderer.transform.position;

            MeshFilter newMeshFilter = newObj.GetComponent<MeshFilter>();
            MeshRenderer newMeshRenderer = newObj.GetComponent<MeshRenderer>();

            newMeshFilter.sharedMesh = newMesh;
            newMeshRenderer.sharedMaterial = _resultMaterial;

            _resultObjects.Add(newObj);
            newMeshes.Add(newMesh);
            newMeshFilters.Add(newMeshFilter);
            newMeshRenderers.Add(newMeshRenderer);
        }

#if UNITY_EDITOR
        _caches.Clear();
        _resultMeshes.Clear();
        // save texture
        if(!System.IO.Directory.Exists(Application.dataPath + "/" + _outputPath))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/" + _outputPath);
        }
        string path = Application.dataPath + "/" + _outputPath + "/" + _textureName + ".png";
        byte[] bytes = atlas.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, bytes);

        // save material
        if(!System.IO.Directory.Exists(Application.dataPath + "/" + _outputPath))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/" + _outputPath);
        }
        string materialPath = Application.dataPath + "/" + _outputPath + "/" + _materialName + ".mat";
        UnityEditor.AssetDatabase.CreateAsset(_resultMaterial, "Assets/" + _outputPath + "/" + _materialName + ".mat");

        // save meshes
        if(!System.IO.Directory.Exists(Application.dataPath + "/" + _outputPath + "/" + _meshFolderName))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath + "/" + _outputPath + "/" + _meshFolderName);
        }
        string meshPath = Application.dataPath + "/" + _outputPath + "/" + _meshFolderName;
        if (!System.IO.Directory.Exists(meshPath))
        {
            System.IO.Directory.CreateDirectory(meshPath);
        }
        for (int i = 0; i < newMeshes.Count; i++)
        {
            string meshName = _meshName + i + ".asset";
            UnityEditor.AssetDatabase.CreateAsset(newMeshes[i], "Assets/" + _outputPath + "/" + _meshFolderName + "/" + meshName);
        }


        UnityEditor.AssetDatabase.Refresh();
        
        // rebind texture
        Texture2D reloaded = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/" + _outputPath + "/" + _textureName + ".png");
        _result = reloaded;
        _resultMaterial.mainTexture = reloaded;


        // rebind material
        Material reloadedMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/" + _outputPath + "/" + _materialName + ".mat");
        _resultMaterial = reloadedMaterial;
        foreach (MeshRenderer meshRenderer in newMeshRenderers)
        {
            meshRenderer.sharedMaterial = reloadedMaterial;
        }


        // rebind meshes
        for (int i = 0; i < newMeshFilters.Count; i++)
        {
            string meshName = _meshName + i + ".asset";
            Mesh reloadedMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>("Assets/" + _outputPath + "/" + _meshFolderName + "/" + meshName);
            newMeshFilters[i].sharedMesh = reloadedMesh;
            _resultMeshes.Add(reloadedMesh);
        }
#endif

    }

    Rect[] PackTextures(Texture2D atlas, Texture2D[] textures, int padding, int maximumAtlasSize)
    {
        Rect[] rects = atlas.PackTextures(textures, padding, maximumAtlasSize);
        // Rect[] rects = PackTextures2(atlas, textures, padding, maximumAtlasSize);
        return rects;
    }

    public static Rect[] PackTextures2(Texture2D atlas, Texture2D[] textures, int padding, int maxSize)
    {
        List<Rect> packedRects = new List<Rect>();

        int currentX = 0;
        int currentY = 0;
        int rowHeight = 0;

        foreach (Texture2D texture in textures)
        {
            if (currentX + texture.width > maxSize)
            {
                currentX = 0;
                currentY += rowHeight + padding;
                rowHeight = 0;
            }

            if (currentY + texture.height > maxSize)
            {
                throw new System.Exception("Textures do not fit in the atlas.");
            }

            packedRects.Add(new Rect(currentX, currentY, texture.width, texture.height));

            currentX += texture.width + padding;
            rowHeight = Mathf.Max(rowHeight, texture.height);
        }

        foreach (Texture2D texture in textures)
        {
            Rect rect = packedRects[System.Array.IndexOf(textures, texture)];
            atlas.SetPixels((int)rect.x, (int)rect.y, texture.width, texture.height, texture.GetPixels());
        }
        atlas.Apply();

        return packedRects.ToArray();
    }



    public void DestroyAllResultObjects()
    {
        foreach (GameObject obj in _resultObjects)
        {
            DestroyImmediate(obj);
        }
        _resultObjects.Clear();
    }

#if UNITY_EDITOR
    [Header("Undos")]
    [SerializeField] List<MeshRenderer> _originalMeshRenderers = new List<MeshRenderer>();
    [SerializeField] List<Material> _originalMaterials = new List<Material>();
    public void NullsMaterials()
    {
        Undo.RecordObjects(_meshRenderers.ToArray(), "Nulls Materials");
        for(int i = 0; i < _meshRenderers.Count; i++)
        {
            MeshRenderer meshRenderer = _meshRenderers[i];
            _originalMaterials.Add(meshRenderer.sharedMaterial);
            _originalMeshRenderers.Add(meshRenderer);
            meshRenderer.sharedMaterial = null;
        }
    }
    public void RestoreMaterials()
    {
        Undo.RecordObjects(_originalMeshRenderers.ToArray(), "Restore Materials");
        for(int i = 0; i < _originalMeshRenderers.Count; i++)
        {
            MeshRenderer meshRenderer = _originalMeshRenderers[i];
            Material material = _originalMaterials[i];
            meshRenderer.sharedMaterial = material;
        }
        _originalMaterials.Clear();
        _originalMeshRenderers.Clear();
    }

    public void DisableRenderers()
    {
        Undo.RecordObjects(_meshRenderers.ToArray(), "Disable Renderers");
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    public void EnableRenderers()
    {
        Undo.RecordObjects(_meshRenderers.ToArray(), "Enable Renderers");
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            meshRenderer.enabled = true;
        }
    }

#endif



    private void AdjustUVs(Mesh mesh, Rect trimmingInfo, int originalWidth, int originalHeight)
    {
        Vector2[] uvs = mesh.uv;
        Vector2 scale = new Vector2(trimmingInfo.width / originalWidth, trimmingInfo.height / originalHeight);
        Vector2 offset = new Vector2(trimmingInfo.x / originalWidth, trimmingInfo.y / originalHeight);

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(uvs[i].x * scale.x + offset.x, uvs[i].y * scale.y + offset.y);
        }
        mesh.uv = uvs;
    }



    bool IsValid()
    {
        if(!IsAllMaterialSame()) {
            Debug.LogError("All materials need to have same shader");
            return false;
        }
        if(!IsTextureIdValid(_textureId, _meshRenderers[0].sharedMaterials[0])) {
            Debug.LogError("Texture id is not valid");
            return false;
        }
        return true;
    }

    bool IsAllMaterialSame()
    {
        List<Material> materials = new List<Material>();
        foreach (MeshRenderer meshRenderer in _meshRenderers)
        {
            materials.AddRange(meshRenderer.sharedMaterials);
        }
        if(materials.Count == 0) return false;
        
        // all material needs to have same shader
        Shader shader = materials[0].shader;
        if (materials.Any(m => m.shader != shader))
        {
            return false;
        }

        return true;
    }

    bool IsTextureIdValid(string textureId, Material material)
    {
        if (material.HasProperty(textureId) && material.GetTexture(textureId) is Texture2D)
        {
            return true;
        }
        return false;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(MeshMaterialTextureCombiner))]
public class MeshMaterialTextureCombinerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MeshMaterialTextureCombiner combiner = target as MeshMaterialTextureCombiner;


        if (GUILayout.Button("Combine"))
        {
            combiner.Combine();
        }
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Nulls Materials"))
        {
            combiner.NullsMaterials();
        }
        if(GUILayout.Button("Restore Materials"))
        {
            combiner.RestoreMaterials();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Disable Renderers"))
        {
            combiner.DisableRenderers();
        }
        if(GUILayout.Button("Enable Renderers"))
        {
            combiner.EnableRenderers();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Destroy All Result Objects"))
        {
            combiner.DestroyAllResultObjects();
        }


        if(GUILayout.Button("Destroy All Child"))
        {
            List<Transform> children = new();
            foreach (Transform child in combiner.transform) children.Add(child);

            foreach (Transform child in children)
            {
                DestroyImmediate(child.gameObject);
            }
        }
        GUILayout.EndHorizontal();
    }
}
#endif
