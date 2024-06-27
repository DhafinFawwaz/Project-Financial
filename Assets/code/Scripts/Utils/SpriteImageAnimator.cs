using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteImageAnimator : MonoBehaviour
{
    [SerializeField] Sprite[] _sprites;
    [SerializeField] float _durationEachFrame = 0.03f;
    [SerializeField] bool _loop = true;

    Image _img;
    int _currentIndex = 0;

    void OnEnable()
    {
        _currentIndex = 0;
        _img = GetComponent<Image>();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while(true)
        {
            _img.sprite = _sprites[_currentIndex];
            yield return new WaitForSeconds(_durationEachFrame);
            _currentIndex++;
            if(_currentIndex >= _sprites.Length)
            {
                if(_loop)
                {
                    _currentIndex = 0;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
