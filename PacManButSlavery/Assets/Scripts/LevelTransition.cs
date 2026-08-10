using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelTransition : MonoBehaviour
{
    private Image fadeImage;
    void Start()
    {
        fadeImage = GetComponent<Image>();
        FadeIn();
    }

    void Update()
    {
        
    }

    public void FadeIn()
    {
        fadeImage.DOFade(0, 3);
    }

    public void FadeOutEndLevel(bool safeExit = true)
    {
        fadeImage.DOFade(1, 3).OnComplete(() => GameManager.Instance.LevelReset(safeExit));
    }
}
