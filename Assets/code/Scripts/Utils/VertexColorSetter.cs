using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColorSetter : MonoBehaviour
{
    [SerializeField] Renderer _rend;
    [SerializeField] Color _color;
    void OnValidate()
    {
        if (_rend == null)
            _rend = GetComponent<Renderer>();
        SetColor(_color);
    }
    void SetColor(Color color)
    {
        if (_rend == null)
            return;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        _rend.GetPropertyBlock(block);
        block.SetColor("_Color", color);
        _rend.SetPropertyBlock(block);
    }
}
