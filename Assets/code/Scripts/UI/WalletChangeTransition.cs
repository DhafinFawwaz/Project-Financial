using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WalletChangeTransition : SceneTransition
{
    [Header("Animation Properties")]
    [SerializeField] Image _black;
    [SerializeField] AnimationUI _walletAnimation;
    [SerializeField] float _walletAnimationDuration = 0.5f;
    [SerializeField] GameObject _needsWallet;
    [SerializeField] GameObject _desireWallet;
    [SerializeField] GameObject _debitWallet;
    [SerializeField] bool _toDesire = true;
    [SerializeField] bool _desireToDebit = false;
    [SerializeField] bool _debitToDesire = false;

    public void SwitchWallet()
    {
        _needsWallet.SetActive(!_toDesire);
        _desireWallet.SetActive(_toDesire);
        if(_desireToDebit)
        {
            _needsWallet.SetActive(false);
            _desireWallet.SetActive(false);
            _debitWallet.SetActive(true);
        } else if(_debitToDesire)
        {
            _needsWallet.SetActive(false);
            _desireWallet.SetActive(true);
            _debitWallet.SetActive(false);
        }
    }

    void InitWallet()
    {
        _needsWallet.SetActive(_toDesire);
        _desireWallet.SetActive(!_toDesire);
        if(_desireToDebit)
        {
            _needsWallet.SetActive(false);
            _desireWallet.SetActive(true);
            _debitWallet.SetActive(false);
        } else if(_debitToDesire)
        {
            _needsWallet.SetActive(false);
            _desireWallet.SetActive(false);
            _debitWallet.SetActive(true);
        }
    }

    public static Action s_onWalletSceneTransition;

    /// <summary>
    /// The out animation itself
    /// </summary>
    /// <returns></returns>
    public override IEnumerator OutAnimation()
    {
        InitWallet();
        float t = 0;
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_outDuration;
            _black.color = new Color(0, 0, 0, Ease.InOutCubic(t));
            yield return null;
        }
        _black.color = new Color(0, 0, 0, 1);
        
        Time.timeScale = 1;
        _walletAnimation.Play();
        s_onWalletSceneTransition?.Invoke();
        yield return new WaitForSeconds(_walletAnimationDuration);
        Time.timeScale = 0;
    }

    /// <summary>
    /// The in animation itself
    /// </summary>
    /// <returns></returns>
    public override IEnumerator InAnimation()
    {
        float t = 0;
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_inDuration;
            _black.color = new Color(0, 0, 0, Ease.InOutCubic(1-t));
            yield return null;
        }
        _black.color = new Color(0, 0, 0, 0);
        Time.timeScale = 1;
    }

}
