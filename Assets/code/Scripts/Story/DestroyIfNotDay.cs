using UnityEngine;

public class DestroyIfNotDay : MonoBehaviour
{
    [SerializeField] int _day = 0;
    [SerializeField] int _streamingCount = 0;
    void Awake()
    {
        if(Save.Data.CurrentDay != _day || Save.Data.CurrentDayData.StreamingCounter != _streamingCount)
        {
            Destroy(gameObject);
        }
    }
}