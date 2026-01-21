using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Playing,
    Dialogue,
    Paused,
    GameOver
}
public class GameStateController : MonoBehaviour
{
    public static GameStateController Instance {get; private set;}

    

    [SerializeField] private GameState _currentState = GameState.Playing;
    
    [Header("Managers")]
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GamePauseManager _gamePauseManager;
    [SerializeField] private GameOverManager _gameOverManager;
    
    public GameState CurrentState => _currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void SetState(GameState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Dialogue:
            case GameState.Paused:
            case GameState.GameOver:
                Time.timeScale = 0f;
                break;
        }
    }
    // ----------------------------- //
    // ----------REQUESTS----------- //
    // ----------------------------- //

    public bool RequestPause()
    {
        if (_currentState != GameState.Playing)
            return false;
        SetState(GameState.Paused);
        return true;
    }
    public void ResumeFromPause()
    {
        if (_currentState != GameState.Paused)
            return;
        SetState(GameState.Playing);
        Time.timeScale = 1f;
    }

    public bool StartDialogue()
    {
        if (_currentState != GameState.Playing)
            return false;
        SetState(GameState.Dialogue);
        _dialogueManager.BeginDialogue();
        return true;
    }

    public void EndDialogue()
    {
        if (_currentState != GameState.Dialogue)
            return;
        SetState(GameState.Playing);
    }
    public void TriggerGameOver()
    {
        if (_currentState == GameState.GameOver)
            return;
        SetState(GameState.GameOver);
        _gameOverManager.ActivateGameOver();
        Time.timeScale = 0;
    }
    public void ResumeFromGameOver()
    {
        if (_currentState != GameState.GameOver)
            return;

        SetState(GameState.Playing);
    }

}
