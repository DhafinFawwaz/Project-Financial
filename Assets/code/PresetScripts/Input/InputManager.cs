using UnityEngine;
public class InputManager : MonoBehaviour
{
    static bool _canMouseButtonDown = true;
    static bool _canGetKeyDown = true;
    static bool _canGetAxisRaw = true;
    public static bool GetMouseButtonDown(int button)
    {
        if(!_canMouseButtonDown) return false;
        return Input.GetMouseButtonDown(button);
    }

    public static bool GetKeyDown(KeyCode key)
    {
        if(!_canGetKeyDown) return false;
        return Input.GetKeyDown(key);
    }

    public static float GetAxisRaw(string axisName)
    {
        if(!_canGetAxisRaw) return 0;
        return Input.GetAxisRaw(axisName);
    }

    public static void SetActiveMouseAndKey(bool active)
    {
        _canMouseButtonDown = active;
        _canGetKeyDown = active;
        _canGetAxisRaw = active;
    }
    
    void OnDisable()
    {
        SetActiveMouseAndKey(true);
    }
}
