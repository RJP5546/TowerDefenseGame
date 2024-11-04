using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class WaveProgressTracker : Singleton<WaveProgressTracker>
{
    [Header("References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject Sun;
    
    [Header("Sunrise Settings")]
    [SerializeField] private Vector3 sunriseStartRotation = new Vector3(-60, -90, 180);
    [SerializeField] private Vector3 sunriseEndRotation = new Vector3(0, -150, -90);
    [SerializeField] private float sunriseBarLerpRate;
    [SerializeField] private float sunriseSunLerpRate;

    [Header("Daytime Settings")]
    [SerializeField] private Vector3 daytimeStartRotation = new Vector3(0, -150, -90);
    [SerializeField] private Vector3 daytimeEndRotation = new Vector3(0, -30, 90);
    [SerializeField] private float daytimeBarLerpRate;
    [SerializeField] private float daytimeSunLerpRate;

    [Header("Sunset Settings")]
    [SerializeField] private Vector3 sunsetStartRotation = new Vector3(-60, -90, 180);
    [SerializeField] private Vector3 sunsetEndRotation = new Vector3(0, -150, -90);
    [SerializeField] private float sunsetBarLerpRate;
    [SerializeField] private float sunsetSunLerpRate;

    /// <summary>
    /// Called before start(). used to initialize variables and clamp inputs
    /// </summary>
    protected override void AwakeOverride()
    {
        //Get the progress bar reference
        if (progressSlider == null) { progressSlider = GetComponent<Slider>(); }

        //Make sure the progress bar moves equal to or faster than the sun
        sunriseBarLerpRate = Mathf.Max(sunriseBarLerpRate, sunriseSunLerpRate);
        daytimeBarLerpRate = Mathf.Max(daytimeBarLerpRate, daytimeSunLerpRate);
        sunsetBarLerpRate = Mathf.Max(sunsetBarLerpRate, sunsetSunLerpRate);
    }

    /// <summary>
    /// Coroutine for making the sun rise from midnight
    /// </summary>
    /// <returns></returns>
    public IEnumerator SunRise()
    {
        Debug.Log("Sunrise Start");
        //Percent progress of the sunrise
        float _sunriseProgress = 0;
        //Percentage fill of the progress bar
        float _progressBarFill = 0;

        //while the sunrise isnt finished
        while (_sunriseProgress <= 1)
        {
            //Increase the progress bar's fill by the lerp rate
            _progressBarFill += sunriseBarLerpRate * Time.deltaTime;
            //updates the slider
            progressSlider.value = _progressBarFill;

            //Increase the sunrise's progress by it's lerp rate
            _sunriseProgress += sunriseSunLerpRate * Time.deltaTime;
            //Move the sun based on the sunrise progress
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(sunriseStartRotation), Quaternion.Euler(sunriseEndRotation), _sunriseProgress);

            yield return null;
        }

        //make sure animations are done
        progressSlider.value = 1;
        Sun.transform.localRotation = Quaternion.Euler(sunriseEndRotation);

        Debug.Log("Sunrise End");
    }

    /// <summary>
    /// Runs every frame while the wave is running. Updates the progress bar and sun position
    /// </summary>
    /// <returns></returns>
    public IEnumerator Daytime()
    {
        Debug.Log("Daytime Start");

        //Exact percentage the wave is complete
        float _waveProgress = 0;
        //percentage the sun has moved, smoothed by lerping value
        float _sunProgress = 0;
        //percentage the bar has moved, smoothed by lerping value
        float _barProgress = 0;

        //while the wave is running
        while (WaveManager.Instance.IsWaveRunning)
        {
            //Get the exact wave progress
            _waveProgress = WaveManager.Instance.WaveProgressPercent;

            //move the bar towards the wave progress at the lerping rate
            _barProgress = Mathf.Min(_waveProgress, _barProgress + (daytimeBarLerpRate * Time.deltaTime));
            progressSlider.value = 1 - _barProgress;

            //move the sun towards the wave progress at the lerping rate
            _sunProgress = Mathf.Min(_waveProgress, _sunProgress + (daytimeSunLerpRate * Time.deltaTime));
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(daytimeStartRotation), Quaternion.Euler(daytimeEndRotation), _sunProgress);

            yield return null;
        }
        Debug.Log("Daytime End");
    }

    /// <summary>
    /// Moves through the sunset animation
    /// </summary>
    /// <returns></returns>
    public IEnumerator SunSet()
    {
        Debug.Log("Sunset start");

        //progress of sunset animation
        float _sunsetProgress = 0;
        //percentage fill of bar
        float _barProgress = progressSlider.value;

        while (_sunsetProgress <= 1)
        {
            //bring the bar down to 0
            _barProgress -= sunsetBarLerpRate * Time.deltaTime;
            progressSlider.value = _barProgress;
            
            //move the sun
            _sunsetProgress += sunsetSunLerpRate * Time.deltaTime;
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(sunsetStartRotation), Quaternion.Euler(sunsetEndRotation), _sunsetProgress);
            
            yield return null;
        }

        //make sure animations are done
        progressSlider.value = 0;
        Sun.transform.localRotation = Quaternion.Euler(sunsetEndRotation);

        Debug.Log("Sunset end");
    }
}
