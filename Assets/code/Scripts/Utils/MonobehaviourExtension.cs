using UnityEngine;

public static class MonoBehaviourExtension
{
    public static void Invoke(this MonoBehaviour monoBehaviour, System.Action action, float time)
    {
        monoBehaviour.StartCoroutine(InvokeCoroutine(action, time));
    }

    private static System.Collections.IEnumerator InvokeCoroutine(System.Action action, float time)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}