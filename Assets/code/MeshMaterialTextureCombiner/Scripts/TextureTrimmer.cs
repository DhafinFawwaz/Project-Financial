#if UNITY_EDITOR
using UnityEngine;

public class TextureTrimmer : MonoBehaviour
{
    public void TrimAndResizeMeshTexture(MeshRenderer meshRenderer, out Rect trimmingInfo, int padding = 2)
    {
        // 1. Get the first shared material and its main texture
        Material originalMaterial = meshRenderer.sharedMaterials[0];
        Texture2D originalTexture = originalMaterial.mainTexture as Texture2D;

        if (originalTexture == null)
        {
            trimmingInfo = Rect.zero;
            Debug.LogError("The main texture is null or not a Texture2D.");
            return;
        }

        // 2. Trim the texture
        Texture2D trimmedTexture = TrimTexture(originalTexture, out trimmingInfo);

        // 3. Pad the trimmed texture to the nearest power of 2
        Texture2D paddedTexture = PadToPowerOfTwo(trimmedTexture, padding);

        // 4. Copy the mesh and adjust UVs to fit the new texture size
        Mesh originalMesh = meshRenderer.GetComponent<MeshFilter>().sharedMesh;
        Mesh newMesh = Instantiate(originalMesh); // Copy the mesh to avoid memory leaks
        AdjustUVs(newMesh, trimmingInfo, originalTexture.width, originalTexture.height);

        // 5. Create a new material using the same shader
        Material newMaterial = new Material(originalMaterial.shader);
        newMaterial.mainTexture = paddedTexture;

        // 6. Instantiate a new GameObject and apply the new mesh and material
        GameObject newObj = Instantiate(meshRenderer.gameObject);
        newObj.transform.position = meshRenderer.transform.position;

        // rescale the object it to the original meshRenderer scale either with to x scale only or y scale only
        // if the width is bigger than the height, scale the object to the width
        // else scale the object to the height

        float width = trimmingInfo.width;
        float height = trimmingInfo.height;
        if(width > height)
        {
            newObj.transform.localScale = new Vector3(meshRenderer.transform.localScale.x, meshRenderer.transform.localScale.x, meshRenderer.transform.localScale.x);
        }
        else
        {
            newObj.transform.localScale = new Vector3(meshRenderer.transform.localScale.y, meshRenderer.transform.localScale.y, meshRenderer.transform.localScale.y);
        }


        MeshFilter newMeshFilter = newObj.GetComponent<MeshFilter>();
        MeshRenderer newMeshRenderer = newObj.GetComponent<MeshRenderer>();

        newMeshFilter.sharedMesh = newMesh;
        newMeshRenderer.sharedMaterial = newMaterial;


        // save the texture
        string path = Application.dataPath + "/";
        System.IO.Directory.CreateDirectory(path);
        System.IO.File.WriteAllBytes(path + "trimmedTexture.png", paddedTexture.EncodeToPNG());
        UnityEditor.AssetDatabase.Refresh();
    }

    private Texture2D TrimTexture(Texture2D original, out Rect trimmingInfo)
    {
        // Find the non-transparent area of the texture
        int xMin = original.width, xMax = 0, yMin = original.height, yMax = 0;
        Color[] pixels = original.GetPixels();

        for (int y = 0; y < original.height; y++)
        {
            for (int x = 0; x < original.width; x++)
            {
                Color pixel = pixels[x + y * original.width];
                if (pixel.a > 0.01f) // If the pixel is not fully transparent
                {
                    xMin = Mathf.Min(xMin, x);
                    xMax = Mathf.Max(xMax, x);
                    yMin = Mathf.Min(yMin, y);
                    yMax = Mathf.Max(yMax, y);
                }
            }
        }

        // Create a new transparent texture with the trimmed area
        int trimmedWidth = xMax - xMin + 1;
        int trimmedHeight = yMax - yMin + 1;
        Texture2D trimmedTexture = new Texture2D(trimmedWidth, trimmedHeight, TextureFormat.RGBA32, false);
        Color[] trimmedPixels = new Color[trimmedWidth * trimmedHeight];

        // Initialize all pixels to transparent
        for (int i = 0; i < trimmedPixels.Length; i++)
        {
            trimmedPixels[i] = Color.clear;
        }

        trimmedTexture.SetPixels(trimmedPixels);
        trimmedTexture.SetPixels(0, 0, trimmedWidth, trimmedHeight, original.GetPixels(xMin, yMin, trimmedWidth, trimmedHeight));
        trimmedTexture.Apply();

        // Set trimming info
        trimmingInfo = new Rect(xMin, yMin, trimmedWidth, trimmedHeight);
        return trimmedTexture;
    }

    private Texture2D PadToPowerOfTwo(Texture2D trimmedTexture, int padding)
    {
        int newWidth = Mathf.NextPowerOfTwo(trimmedTexture.width + padding * 2);
        int newHeight = Mathf.NextPowerOfTwo(trimmedTexture.height + padding * 2);

        // Create a new transparent padded texture
        Texture2D paddedTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, false);
        Color[] paddedPixels = new Color[newWidth * newHeight];

        for (int i = 0; i < paddedPixels.Length; i++)
        {
            paddedPixels[i] = Color.clear;
        }

        paddedTexture.SetPixels(paddedPixels);

        int xOffset = (newWidth - trimmedTexture.width) / 2;
        int yOffset = (newHeight - trimmedTexture.height) / 2;
        paddedTexture.SetPixels(xOffset, yOffset, trimmedTexture.width, trimmedTexture.height, trimmedTexture.GetPixels());
        paddedTexture.Apply();

        return paddedTexture;
    }

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

    [SerializeField] MeshRenderer _meshRenderer;
    [ContextMenu("TrimAndResizeMeshTexture")]
    public void TrimAndResizeMeshTexture()
    {
        TrimAndResizeMeshTexture(_meshRenderer, out Rect trimmingInfo);
    }
}

#endif