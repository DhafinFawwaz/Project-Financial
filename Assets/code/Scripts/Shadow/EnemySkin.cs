using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySkin : MonoBehaviour
{
    [SerializeField] protected Transform SkinTrans;

    [SerializeField] AnimationCurve _hurtCurveX;
    [SerializeField] AnimationCurve _hurtCurveY;
    [SerializeField] float _hurtAnimationDuration = .3f;


    public void PlayHurtAnimation()
    {
        StartCoroutine(HurtAnimation());
    }
    byte _key;
    IEnumerator HurtAnimation()
    {
        byte requirement = ++_key;
        float t = 0;
        Color initialColor = new Color(0.5f, 0.5f, 0.5f, 1);
        Color finalColor = Color.white;
        while (t < 1 && requirement == _key)
        {
            float x = _hurtCurveX.Evaluate(t);
            float y = _hurtCurveY.Evaluate(t);
            SkinTrans.localScale = new Vector3(x, y, 0);
            // SpriteRenderer.color = Color.Lerp(initialColor, finalColor, Ease.OutQuart(t));
            t += Time.deltaTime/_hurtAnimationDuration;
            yield return null;
        }
        if(requirement == _key)
            SkinTrans.localScale = Vector3.one;
    }


}