using UnityEngine;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour {

    [SerializeField] Image image;

    private void Start()
    {
        image.gameObject.SetActive(true);
    }

    private void PerformFading(float toAlpha, float duration)
    {
        image.CrossFadeAlpha(toAlpha, duration, false);
    }

    public void FadeIn(float duration)
    {
        PerformFading(0f, duration);
    }

    public void FadeOut(float duration)
    {
        PerformFading(1f, duration);
    }
}
