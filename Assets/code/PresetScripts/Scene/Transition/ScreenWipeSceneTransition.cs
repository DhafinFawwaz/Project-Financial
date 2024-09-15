using UnityEngine;
using System.Collections;
using System;

public class ScreenWipeSceneTransition : SceneTransition
{
    [Header("Animation Properties")]
    [SerializeField] RectTransform _orangeSquareRT;
    [SerializeField] float _startXScale = 0;
    [SerializeField] float _endXScale = 1;

    public static Action s_OnScreenWipeTransitionStart;

    /// <summary>
    /// The out animation itself
    /// </summary>
    /// <returns></returns>
    public override IEnumerator OutAnimation()
    {
        s_OnScreenWipeTransitionStart?.Invoke();
        _orangeSquareRT.pivot = new Vector2(0.5f, 1);
        float t = 0;
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_outDuration;
            float newX = Mathf.Lerp(_startXScale, _endXScale, Ease.OutQuart(t));
            _orangeSquareRT.localScale = new Vector3(1, newX, 1);
            yield return null;
        }
        _orangeSquareRT.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// The in animation itself
    /// </summary>
    /// <returns></returns>
    public override IEnumerator InAnimation()
    {
        s_OnScreenWipeTransitionStart?.Invoke();
        _orangeSquareRT.pivot = new Vector2(0.5f, 0);
        float t = 0;
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_inDuration;
            float newX = Mathf.Lerp(_endXScale, _startXScale, Ease.OutQuart(t));
            _orangeSquareRT.localScale = new Vector3(1, newX, 1);
            yield return null;
        }
        _orangeSquareRT.localScale = new Vector3(1, 0, 1);
    }

}
