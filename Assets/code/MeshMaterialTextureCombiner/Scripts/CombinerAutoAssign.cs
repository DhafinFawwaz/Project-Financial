using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CombinerAutoAssign : MonoBehaviour
{
    [SerializeField] MeshMaterialTextureCombiner _combiner;
    [SerializeField] Transform _parent;

    void Reset()
    {
        _combiner = FindObjectOfType<MeshMaterialTextureCombiner>();
        _parent = transform;
    }

    public void Assign()
    {
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        foreach(Transform child in _parent)
        {
            var mr = child.GetChild(0).GetComponent<MeshRenderer>();
            if (mr != null)
            {
                meshRenderers.Add(mr);
            }
        }

        foreach (var meshRenderer in meshRenderers)
        {
            _combiner.MeshRenderers.Add(meshRenderer);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CombinerAutoAssign))]
public class CombinerAutoAssignEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CombinerAutoAssign script = (CombinerAutoAssign)target;

        if (GUILayout.Button("Assign"))
        {
            script.Assign();
        }
    }
}
#endif