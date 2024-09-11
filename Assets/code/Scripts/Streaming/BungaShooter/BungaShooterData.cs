using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "BungaShooterData", menuName = "BungaShooterData")]
public class BungaShooterData : ScriptableObject
{
    public List<BungaData> InitialBungaDatas;
}

[System.Serializable]
public class BungaData
{
    public int Percentage;
    public int Price;
    public BungaData(int percentage, int price)
    {
        Percentage = percentage;
        Price = price;
    }
}

// inspector randomize button
#if UNITY_EDITOR
[CustomEditor(typeof(BungaShooterData))]
public class BungaShooterDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BungaShooterData bungaShooterData = (BungaShooterData)target;
        if(GUILayout.Button("Randomize"))
        {
            for(int i = 0; i < bungaShooterData.InitialBungaDatas.Count; i++)
            {
                bungaShooterData.InitialBungaDatas[i] = new BungaData(Random.Range(0, 50), Random.Range(10, 50) * 10000);
            }
            EditorUtility.SetDirty(bungaShooterData);
        }
    }
}
#endif
