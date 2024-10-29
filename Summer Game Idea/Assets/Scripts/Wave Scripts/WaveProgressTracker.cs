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

    private Coroutine lastRunCoroutine = null;

    /// <summary>
    /// Called before start(). used to initialize variables and clamp inputs
    /// </summary>
    private void Awake()
    {
        //Get the progress bar reference
        if (progressSlider == null) { progressSlider = GetComponent<Slider>(); }

        //Make sure the progress bar moves equal to or faster than the sun
        sunriseBarLerpRate = Mathf.Max(sunriseBarLerpRate, sunriseSunLerpRate);
        daytimeBarLerpRate = Mathf.Max(daytimeBarLerpRate, daytimeSunLerpRate);
        sunsetBarLerpRate = Mathf.Max(sunsetBarLerpRate, sunsetSunLerpRate);
    }

    /// <summary>
    /// Called at beginning of a new wave. Ends any current sun coroutine and starts the sunrise
    /// </summary>
    public void StartOfWave()
    {
        if(lastRunCoroutine == null) { lastRunCoroutine = StartCoroutine(SunRise()); }
        else
        {
            StopCoroutine(lastRunCoroutine);
            lastRunCoroutine = StartCoroutine(SunRise());
        }
    }

    /// <summary>
    /// Called after sunrise finishes. Starts coroutine to move sun across the screen
    /// </summary>
    public void Wave()
    {
        StopCoroutine(lastRunCoroutine);
        lastRunCoroutine = StartCoroutine(Daytime());
    }

    /// <summary>
    /// Called when wave finishes. Starts coroutine for the sunset
    /// </summary>
    public void EndtOfWave()
    {
        StopCoroutine(lastRunCoroutine);
        lastRunCoroutine = StartCoroutine(SunSet());
    }

    /// <summary>
    /// Coroutine for making the sun rise from midnight
    /// </summary>
    /// <returns></returns>
    private IEnumerator SunRise()
    {
        Debug.Log("Sunrise Start");
        //Percent progress of the sunrise
        float _sunriseProgress = 0;
        //Percentage fill of the progress bar
        float _progressBarFill = 0;

        //while the sunrise isnt finished
        while (_sunriseProgress < 1)
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

        //start the daytime coroutine
        Wave();

        Debug.Log("Sunrise End");
    }

    /// <summary>
    /// Runs every frame while the wave is running. Updates the progress bar and sun position
    /// </summary>
    /// <returns></returns>
    private IEnumerator Daytime()
    {
        Debug.Log("Daytime Start");

        //Exact percentage the wave is complete
        float _waveProgress = 1;
        //percentage the sun has moved, smoothed by lerping value
        float _sunProgress = 1;
        //percentage the bar has moved, smoothed by lerping value
        float _barProgress = 1;

        //while the wave is running
        while (true)
        {
            //Get the exact wave progress
            _waveProgress = WaveManager.Instance.WaveProgressPercent;

            //move the bar towards the wave progress at the lerping rate
            _barProgress = Mathf.Max(_waveProgress, _barProgress - (daytimeBarLerpRate * Time.deltaTime));
            progressSlider.value = _barProgress;

            //move the sun towards the wave progress at the lerping rate
            _sunProgress = Mathf.Min(1 - _waveProgress, _sunProgress + (daytimeSunLerpRate * Time.deltaTime));
            Sun.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(daytimeStartRotation), Quaternion.Euler(daytimeEndRotation), _sunProgress);

            yield return null;
        }
    }

    /// <summary>
    /// Moves through the sunset animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator SunSet()
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
