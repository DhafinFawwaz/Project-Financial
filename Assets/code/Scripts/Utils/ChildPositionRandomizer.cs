using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class ChildPositionRandomizer : MonoBehaviour
{
    [SerializeField] Vector3 _boxSize = new Vector3(20, 0, 20);
    public void RandomizeChildPosition() {
        foreach(Transform child in transform) {
            child.localPosition = new Vector3(
                Random.Range(-_boxSize.x/2, _boxSize.x/2),
                Random.Range(-_boxSize.y/2, _boxSize.y/2),
                Random.Range(-_boxSize.z/2, _boxSize.z/2)
            );
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, _boxSize);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ChildPositionRandomizer))]
public class ChildPositionRandomizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ChildPositionRandomizer script = (ChildPositionRandomizer)target;
        if(GUILayout.Button("Randomize Child Position")) {
            script.RandomizeChildPosition();
        }
    }
}
#endif