using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ArcMenu : MonoBehaviour
{
    [SerializeField] Transform player_pivot;
    [SerializeField] private PlayerBattle player;
    [SerializeField] private TurnManager tm;

    public InputAction up_action;
    public InputAction down_action;
    public InputAction space_action;

    //private List<Button> previous_buttons;
    private List<GameObject> active_buttons = new();
    //private List<Button> next_buttons;
    public GameObject current_button;
    private int current_button_index = 1;
    public Sprite[] sprites;

    private List<Vector3> buttons_offsets = new() {
        {new(7.0f, 10.0f, 1.0f)},
        {new(6.5f, 1.8f, 1.0f)},
        {new(9.0f, 0.0f, 1.0f)},
        {new(6.0f, -1.8f, 1.0f)},
        {new(7.0f, -10.0f, 1.0f)}
    };

    const float max_scale = 0.14f;
    const float min_scale = 0.0f;
    public float duration = 1f;

    void Start()
    {
        TurnManager.instance.OnStateChange += OnTurnChange;
        sprites = Resources.LoadAll<Sprite>("Sprites/Fight/menu_option_background_button");
        up_action.Enable();
        down_action.Enable();
        space_action.Enable();

        AddButton("Atak");
        AddButton("Przedmioty: " + player.GetPotionsLeft());
        AddButton("Ucieczka");

        active_buttons[current_button_index].GetComponentInChildren<SpriteRenderer>().sprite = sprites[3];
    }

    void Update() {

        if (space_action.WasPressedThisFrame()) {
            ProcessAction();
        } 

        if (up_action.WasPressedThisFrame()) {
            SetCurrentButton(true);
        }
        if (down_action.WasPressedThisFrame()) {
            SetCurrentButton(false);
        }
    }

    void OnTurnChange(TurnState state, TurnState last_state) {
        if (state == TurnState.PlayerTurn) {
            ShowActiveButtons();
        } else if (last_state == TurnState.PlayerTurn)  {
            HideActiveButtons();
        }
    }

    void AddButton(String text) {
        GameObject button_prefab = Resources.Load<GameObject>("Prefabs/Button");
        GameObject button = Instantiate(button_prefab, transform);

        TextMeshPro t = button.GetComponentInChildren<TextMeshPro>();
        t.SetText(text);
        t.color = new(0.13333f, 0.12549f, 0.20392f);
        SetButton(button, player_pivot.position, min_scale);
        active_buttons.Add(button);
    }

    void UpdateItemsButtonText() {
        foreach (GameObject b in active_buttons) {
            TextMeshPro text = b.GetComponentInChildren<TextMeshPro>();

            if (text.text.StartsWith("Przedmioty")) {
                text.SetText("Przedmioty: " + player.GetPotionsLeft());
            }
        }
    }

    void SetButton(GameObject b, Vector3 position, float scale) {
        position.z += 2f;
        b.transform.position = position;
        b.transform.localScale = new Vector3(scale, scale, 0.0f);
    }

    void ShowActiveButtons() {
        int i = 1;
        foreach(GameObject b in active_buttons) {
            //print("pivot: " + player_pivot.position + "button: " + b.transform.position + "local: " + b.transform.localPosition);
            StartCoroutine(ChangeLayout(b, player_pivot.position, player_pivot.position + buttons_offsets[i], min_scale, max_scale));
            i++;
        }
    }

    void HideActiveButtons() {
        int i = 1;
        foreach(GameObject b in active_buttons) {
            StartCoroutine(ChangeLayout(b, player_pivot.position + buttons_offsets[i], player_pivot.position, max_scale, min_scale));
            i++;
        }
    }

    //IEnumerator ChangeLayout(float start_radius, float end_radius, float start_scale, float end_scale)
    IEnumerator ChangeLayout(GameObject b, Vector2 start_position, Vector2 end_postion, float start_scale, float end_scale)
    {
        float t = 0f;

        while(t < 1f) {
            t += Time.deltaTime/duration;
            float x = Mathf.Lerp(start_position.x, end_postion.x, t);
            float y = Mathf.Lerp(start_position.y, end_postion.y, t);
            float s = Mathf.Lerp(start_scale, end_scale, t);

            SetButton(b, new(x, y), s);
            yield return null;
        }
    }

    void SetCurrentButton(bool next) {
        if (!next){
            current_button_index++;
            if (current_button_index >= active_buttons.Count) {
                current_button_index = 0;
            }
        } else {
            current_button_index--;
            if(current_button_index < 0) {
                current_button_index = active_buttons.Count;
            }
        }   

        foreach(GameObject b in active_buttons) {
            b.GetComponentInChildren<SpriteRenderer>().sprite = sprites[1];
        }
        current_button = active_buttons[current_button_index];
        current_button.GetComponentInChildren<SpriteRenderer>().sprite = sprites[3];
    }

    void ProcessAction() {
        if (current_button.GetComponentInChildren<TextMeshPro>().text == "Atak") {
            player.Attack(3, 1.0f);
        }
        if (current_button.GetComponentInChildren<TextMeshPro>().text.StartsWith("Przedmioty")) {
            player.UsePotion();
            UpdateItemsButtonText();
        }
        tm.NextTurn();
    }

    // void SetLayout(float radius, float scale) {
    //     float start_angle = 0f;
    //     int count = transform.childCount;
    //     if (buttonSpacing.TryGetValue(count, out var angles)) {
    //         start_angle = angles.start_angle;
    //     }

    //     //float step = 35.0f, button_offset = 5.0f;

    //     for (int i = 0; i < count; i++) {
    //             //RectTransform child = transform.GetChild(i) as RectTransform;
    //             Transform button = transform.GetChild(i);
    //             float angle = start_angle - step * i;
    //             //print(i + " " + angle + " " + step);
    //             float rad = angle * Mathf.Deg2Rad;

    //             Vector2 pos = new Vector2(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
    //             pos.y -= button_offset * i;
    //             button.position = pos;
    //             button.localScale = new(scale, scale, 1.0f);
    //             // child.anchoredPosition = pos;
    //             // child.localScale = new Vector3(scale, scale, scale);
    //     }
    // }
}