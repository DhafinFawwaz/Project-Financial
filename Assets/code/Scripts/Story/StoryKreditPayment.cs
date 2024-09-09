using UnityEngine;

public class StoryKreditPayment : MonoBehaviour
{
    [SerializeField] GameObject _backButton;
    [SerializeField] AnimationUI _salaryAnimation;
    [SerializeField] GameObject _salaryPage;
    [SerializeField] GameObject _kreditPage;
    void Awake()
    {
        if((Save.Data.CurrentDay + 1) % 3 == 0) {
            _backButton.SetActive(true);
            _salaryAnimation.Play();
        } else {
            _backButton.SetActive(false);
            _salaryPage.SetActive(false);
            _kreditPage.SetActive(true);
        }


        
    }
}
