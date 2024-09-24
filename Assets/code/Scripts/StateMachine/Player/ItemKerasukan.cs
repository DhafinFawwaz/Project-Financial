using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemKerasukan : MonoBehaviour
{
    [SerializeField] ItemData _itemData;
    public ItemData ItemData => _itemData;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 1.5f);
    }
}
