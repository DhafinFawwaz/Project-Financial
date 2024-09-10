using System;
using System.Collections;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public static Action<float> s_OnRecharging;
    public static Action s_OnNotEnoughEnergy;
    [SerializeField] float _energy = 0;
    [SerializeField] float _rechargeDuration = 10;


    [SerializeField] GameObject _flashLight;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LayerMask _rakLayer;
    [SerializeField] float _flashLightDuration;
    [SerializeField] Material _material;

    // For fading
    [SerializeField] [ColorUsage(true, true)] Color initColor;
    Color transparentColor;

    [SerializeField] HitRequest _hitRequest = new HitRequest{
        Knockback = 120,
        StunDuration = 3
    };

    public HitRequest HitRequest => _hitRequest;


    bool _isInSceneBelanja;
    void Start()
    {
        transparentColor = initColor; transparentColor.a = 0;

        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        _isInSceneBelanja = sceneName == "Belanja" || sceneName == "BelanjaTutorial" || sceneName == "BelanjaTutorial2";
    }

    void Update()
    {
        if(!_isInSceneBelanja) return;
        if(InputManager.GetMouseButtonDown(0)) ToggleFlash();
        AdjustFlashRotationBasedOnMouse();

        _energy += Time.deltaTime / _rechargeDuration;
        _energy = Mathf.Clamp01(_energy);
        s_OnRecharging?.Invoke(_energy);
    }
    void ToggleFlash()
    {
        if(_energy < 0.33f) {
            s_OnNotEnoughEnergy?.Invoke();
            return;
        }
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
        else if (( _rakLayer & (1 << col.gameObject.layer)) != 0) 
        {
            if(col.TryGetComponent<Rak>(out Rak rak)) {
                rak.SetDarken(false);
            }
        }
    }

    IEnumerator StartFlashlight()
    {
        if(_flashLight.activeSelf) {} else {
            _energy -= 0.33f;
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
