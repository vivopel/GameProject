using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    None
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager instance;
    public TurnState state {get; private set;}
    public TurnState last_state {get; private set;}
    public event Action<TurnState, TurnState> OnStateChange;

    public InputAction first;
    public InputAction second;
    public InputAction third;

    void Start()
    {
        first.Enable();
        second.Enable();
        third.Enable();
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (first.IsPressed())
            SetState(TurnState.PlayerTurn);
        if (second.IsPressed())
            SetState(TurnState.EnemyTurn);
        if (third.IsPressed())
            SetState(TurnState.None);   
    }

    public void SetState(TurnState new_state)
    {
        last_state = state;
        state = new_state;
        
        OnStateChange?.Invoke(state, last_state);
        Debug.Log($"[BattleManager] State changed to {state}");
    }
}
