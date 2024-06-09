using System;
using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject _flashLight;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] float _flashLightDuration;
    [SerializeField] Material _material;

    // For fading
    [SerializeField] [ColorUsage(true, true)] Color initColor;
    Color transparentColor;

    [SerializeField] HitRequest _hitRequest = new HitRequest{
        Knockback = 120,
        StunDuration = 3
    };
    void Start()
    {
        transparentColor = initColor; transparentColor.a = 0;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) ToggleFlash();
        AdjustFlashRotationBasedOnMouse();
    }
    void ToggleFlash()
    {
        StartCoroutine(StartFlashlight());
    }
    void AdjustFlashRotationBasedOnMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 direction = (mousePos - screenCenter).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _flashLight.transform.rotation = Quaternion.Euler(-9, -angle+90, 0);
    }

    float _flashLightRange => transform.localScale.z*1.4f;
    float _flashLightRadius => transform.localScale.x;

    void OnTriggerEnter(Collider col)
    {  
        if (( _enemyLayer & (1 << col.gameObject.layer)) != 0) 
        {
            HitResult _hitResult = new HitResult();
            _hitRequest.Direction = (col.transform.position - transform.position).normalized;
            col.GetComponent<Shadow>().OnHurt(_hitRequest, ref _hitResult);
        }
    }

    IEnumerator StartFlashlight()
    {
        if(_flashLight.activeSelf) {} else {
            _flashLight.SetActive(true);
            float t = 0;
            while(t < 1)
            {
                t += Time.deltaTime / (_flashLightDuration/2);
                _material.color = Color.Lerp(transparentColor, initColor, Ease.OutCubic(t));
                yield return null;
            }
            t = 0;
            while(t < 1)
            {
                t += Time.deltaTime / (_flashLightDuration/2);
                _material.color = Color.Lerp(initColor, transparentColor, Ease.OutCubic(t));
                yield return null;
            }

            _flashLight.SetActive(false);
        }
    }
}
