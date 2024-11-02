using System.Collections;
using UnityEngine;

public class GameStateManager : Singleton<GameStateManager>
{
    [SerializeField] private GameState gameState = GameState.PREWAVE;
    private bool isBusy = false;

    // Update is called once per frame
    void Update()
    {
        if (!isBusy)
        {
            
            switch (gameState)
            {
                case GameState.PREWAVE:
                    isBusy = true;
                    StartCoroutine(PreWave());
                    break;
                case GameState.WAVE:
                    isBusy = true;
                    StartCoroutine(Wave());
                    break;
                case GameState.WAVECOMPLETE:
                    isBusy = true;
                    StartCoroutine(WaveComplete());
                    break;
                case GameState.UPGRADE:
                    break;
                case GameState.PAUSE:
                    break;
                case GameState.DEATH:
                    break;
                case GameState.WIN:
                    break;

            }
        }
        
    }

    private IEnumerator PreWave()
    {
        //do the sunrise and bar initialization
        yield return StartCoroutine(WaveProgressTracker.Instance.SunRise());
        //switch game state to wave
        gameState = GameState.WAVE;
        isBusy = false;
    }

    private IEnumerator Wave()
    {
        //set the wave manager to start counting and set the wave as running in WaveManager
        if (!WaveManager.Instance.IsWaveRunning)
        {
            WaveManager.Instance.StartCounting();
            yield return new WaitForSeconds(WaveManager.Instance.ReturnTimeBetweenWaves());
            StartCoroutine(WaveProgressTracker.Instance.Daytime());
        }
        //if the wave is running, do nothing
        while (WaveManager.Instance.IsWaveRunning) { yield return null; }
        gameState = GameState.WAVECOMPLETE;
        isBusy = false;
    }

    private IEnumerator WaveComplete()
    {
        //when the wave is done, set the sun
        yield return StartCoroutine(WaveProgressTracker.Instance.SunSet());

        //temporary stand in for between wave activities
        yield return new WaitForSeconds(5);

        //start next wave
        gameState = GameState.PREWAVE;
        isBusy = false;
    }

    

    public void UpdateGameState(GameState _gameState)
    {
        gameState = _gameState;
    }

}
