using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasFader : MonoBehaviour {

    [SerializeField] Image image;

    private void PerformFading(float toAlpha)
    {
        image.CrossFadeAlpha(toAlpha, 1f, false);
    }

    public void FadeIn()
    {
        PerformFading(0f);
    }

    public void FadeOut()
    {
        PerformFading(1f);
    }
}
