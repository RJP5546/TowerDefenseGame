using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class WaveProgressTracker : Singleton<WaveProgressTracker>
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject Sun;
    private float storedWavePercentRemaining = 0;
    [SerializeField] private float barLerpRate;
    [SerializeField] private float sunLerpRate;

    private Coroutine lastRunCoroutine = null;

    private void Start()
    {
        if (progressSlider == null) { progressSlider = GetComponent<Slider>(); }
    }

    public void StartOfWave()
    {
        if(lastRunCoroutine == null) { lastRunCoroutine = StartCoroutine(SunRise()); }
        else
        {
            StopCoroutine(lastRunCoroutine);
            lastRunCoroutine = StartCoroutine(SunRise());
        }
    }

    public void Wave()
    {
        StopCoroutine(lastRunCoroutine);
        lastRunCoroutine = StartCoroutine(WaveRunning());
    }

    public void EndtOfWave()
    {
        StopCoroutine(lastRunCoroutine);
        lastRunCoroutine = StartCoroutine(SunSet());
    }

    private void UpdateProgressSlider(float targetValue, float _barLerp)
    {
        storedWavePercentRemaining = Mathf.MoveTowards(storedWavePercentRemaining, targetValue, _barLerp);
        progressSlider.value = storedWavePercentRemaining;
    }

    private IEnumerator SunRise()
    {
        float _sunriseLerp = 0;
        float _barLerp = 0;
        while (_sunriseLerp < 1)
        {
            _barLerp += barLerpRate * Time.deltaTime;
            UpdateProgressSlider(100, _barLerp);
            _sunriseLerp += sunLerpRate * Time.deltaTime;
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(-60, -90, 180), Quaternion.Euler(0, -150, -90), _sunriseLerp);
            Debug.Log("Sunrise running");
            yield return null;
        }
        Wave();
    }

    private IEnumerator WaveRunning()
    {
        float _barLerp = 0;
        while (true)
        {
            var temp = WaveManager.Instance.WavePercentRemaining;
            if (storedWavePercentRemaining != temp)
            {
                _barLerp += barLerpRate * Time.deltaTime;
                UpdateProgressSlider(temp, _barLerp);
                Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -30, 90), Quaternion.Euler(0, -150, -90), storedWavePercentRemaining / 100);
                yield return null;
            }
            _barLerp = 0;
            Debug.Log("WaveSun running");
            yield return null;
        }
    }

    private IEnumerator SunSet()
    {
        float _nightfallLerp = 0;
        float _barLerp = 0;
        while (storedWavePercentRemaining > 0)
        {
            _barLerp += barLerpRate * Time.deltaTime;
            UpdateProgressSlider(0,_barLerp);
            yield return null;
        }

        while (_nightfallLerp <= 1)
        {

            _nightfallLerp += sunLerpRate * Time.deltaTime;
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, -30, 90), Quaternion.Euler(-60, -90, 180), _nightfallLerp);
            Debug.Log("Sunset running");
            yield return null;
        }
        yield return null;
    }
}
