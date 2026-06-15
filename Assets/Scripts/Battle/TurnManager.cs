using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum TurnState {
    PlayerTurn,
    EnemyTurn,
    None
}

public class TurnManager : MonoBehaviour
{
    Vector3 up_button_position = new(-0.7f, -2.5f, 0.0f);
    Vector3 down_button_position = new(-0.7f, -13.0f, 0.0f);
    public static TurnManager instance;
    public TurnState state {get; private set;}
    public TurnState last_state {get; private set;}
    public event Action<TurnState, TurnState> OnStateChange;
    private TurnState[] turn_order = {TurnState.None, TurnState.PlayerTurn, TurnState.None, TurnState.EnemyTurn};
    private int turn_count = 0;
    [SerializeField] GameObject next_turn_button;

    public InputAction first;
    public InputAction second;
    public InputAction third;
    public InputAction next;

    void Start() {
        first.Enable();
        second.Enable();
        third.Enable();
        next.Enable();
    }

    void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        state = TurnState.None;
        instance.OnStateChange += OnTurnChange;
    }

    void Update() {
        if (first.IsPressed())
            SetState(TurnState.PlayerTurn);
        if (second.IsPressed())
            SetState(TurnState.EnemyTurn);
        if (third.IsPressed())
            SetState(TurnState.None);  
        if (next.WasPressedThisFrame()){ //&& state == TurnState.PlayerTurn
            // turn_count++;
            // if (turn_count > 3)
            //     turn_count = 0;
            // SetState(turn_order[turn_count]); 
            NextTurn();
        }
    }

    public void SetState(TurnState new_state) {
        last_state = state;
        state = new_state;
        OnStateChange?.Invoke(state, last_state);
    }

    void OnTurnChange(TurnState state, TurnState last_state) {
        if (state == TurnState.PlayerTurn) {
            
        }
        if (state != TurnState.PlayerTurn) {
            
        }
        if (state == TurnState.PlayerTurn) {
            StartCoroutine(MoveButton(down_button_position.y, up_button_position.y));
        } else if (last_state == TurnState.PlayerTurn)  {
            StartCoroutine(MoveButton(up_button_position.y, down_button_position.y));
        }
    }

    IEnumerator MoveButton(float start, float end) {
        float t = 0.0f;

        while(t < 1f) {
            t += Time.deltaTime/1f;
            float s = Mathf.Lerp(start, end, t);
            SetButtonY(s);
            yield return null;
        }
    }

    void SetButtonY(float y) {
        var trans = next_turn_button.GetComponent<Transform>();
        trans.position = new(up_button_position.x, y, up_button_position.z);
    }

    public void NextTurn() {
        turn_count++;
        if (turn_count > 3)
            turn_count = 0;
        SetState(turn_order[turn_count]); 
    }
}
