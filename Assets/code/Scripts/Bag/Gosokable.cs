using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gosokable : MonoBehaviour
{
    [SerializeField] RectTransform _rt;
    [SerializeField] RawImage _rawImage;
    
    [Range(0f, 1f)]
    float _clearThreshold = 0.98f;
    [SerializeField] int _itemID;
    public int ItemID => _itemID;
    void Start()
    {
        _rt = GetComponent<RectTransform>();

        // make a copy of the texture
        Color[] pixels = ((Texture2D)_rawImage.texture).GetPixels();
        Texture2D copy = new Texture2D(_rawImage.texture.width, _rawImage.texture.height);
        copy.SetPixels(pixels);
        copy.Apply();

        _rawImage.texture = copy;
    }
    

    public bool ContainsCursor() => RectTransformUtility.RectangleContainsScreenPoint(_rt, Input.mousePosition);

    // Erase part of the image
    public void Gosok(Vector2 screenPos, float radius = 20)
    {
        Vector2 localPos = screenPos - new Vector2(_rt.position.x, _rt.position.y);
        localPos = new Vector2(localPos.x / _rt.lossyScale.x, localPos.y / _rt.lossyScale.y);
        localPos += new Vector2(_rt.rect.width / 2, _rt.rect.height / 2);
        localPos = new Vector2(localPos.x / _rt.rect.width, localPos.y / _rt.rect.height);
        localPos = new Vector2(localPos.x * _rawImage.texture.width, localPos.y * _rawImage.texture.height);

        Texture2D tex = _rawImage.texture as Texture2D;

        // prevent out of bounds
        int xStart = Mathf.Max(0, (int)(localPos.x - radius));
        int xEnd = Mathf.Min(tex.width, (int)(localPos.x + radius));
        int yStart = Mathf.Max(0, (int)(localPos.y - radius));
        int yEnd = Mathf.Min(tex.height, (int)(localPos.y + radius));

        for(int x = xStart; x < xEnd; x++)
        {
            for(int y = yStart; y < yEnd; y++)
            {
                if(Vector2.Distance(localPos, new Vector2(x, y)) < radius)
                {
                    tex.SetPixel(x, y, Color.clear);
                }
            }
        }

        tex.Apply();
    }


    bool _isAllClearCache = false;
    public bool IsAllClear()
    {
        if(_isAllClearCache) return true;
        Color[] pixels = ((Texture2D)_rawImage.texture).GetPixels();
        int clearCount = 0;
        foreach(Color pixel in pixels)
        {
            if(pixel.a == 0) clearCount++;
        }
        _isAllClearCache = (float)clearCount / pixels.Length > _clearThreshold;
        return _isAllClearCache;
    }
}
