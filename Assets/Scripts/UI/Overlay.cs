using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Overlay : MonoBehaviour
{
    [SerializeField] private float m_fadeSpeed = 2f;
    private Image _overlayImage;

    private Coroutine _fadeInCoroutine;
    private Coroutine _fadeOutCoroutine;


    private Color _workbench;

    private void Awake()
    {
        _overlayImage = GetComponent<Image>();
    }

    public void FadeIn(Action fadeInComplete = null)
    {
        StopFadeInCoroutine();
        StopFadeOutCoroutine();

        gameObject.SetActive(true);
        _fadeInCoroutine = StartCoroutine(IE_FadeIn(fadeInComplete));
    }

    private void StopFadeInCoroutine()
    {
        if (_fadeInCoroutine != null)
        {
            StopCoroutine(_fadeInCoroutine);
        }
    }

    private void StopFadeOutCoroutine()
    {
        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
        }
    }

    public void FadeOut(Action fadeOutComplete = null)
    {
        StopFadeInCoroutine();
        StopFadeOutCoroutine();

        _fadeOutCoroutine = StartCoroutine(IE_FadeOut(fadeOutComplete));
    }

    private IEnumerator IE_FadeIn(Action fadeInComplete)
    {
        _workbench = _overlayImage.color;
        _workbench.a = 0;
        _overlayImage.color = _workbench;

        while(_overlayImage.color.a < 1)
        {
            _workbench.a = Mathf.MoveTowards(_workbench.a, 1, Time.deltaTime * 5 * m_fadeSpeed);
            _overlayImage.color = _workbench;
            yield return null;
        }

        _workbench.a = 1;
        _overlayImage.color = _workbench;
        yield return new WaitForSeconds(1);
        fadeInComplete?.Invoke();
    }

    private IEnumerator IE_FadeOut(Action fadeoutComplete)
    {
        _workbench = _overlayImage.color;
        _workbench.a = 1;
        _overlayImage.color = _workbench;

        while (_overlayImage.color.a > 0)
        {
            _workbench.a = Mathf.MoveTowards(_workbench.a, 0, Time.deltaTime * 5 * m_fadeSpeed);
            _overlayImage.color = _workbench;
            yield return null;
        }

        _workbench.a = 0;
        _overlayImage.color = _workbench;
        gameObject.SetActive(false);

        fadeoutComplete?.Invoke();
    }
}
