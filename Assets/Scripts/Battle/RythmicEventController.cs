using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RythmicEventController : MonoBehaviour
{
    public InputAction ZAction;
    public InputAction XAction;
    public InputAction CAction;

    [SerializeField] Canvas canvas;

    public Sprite[] sprites;
    public List<RythmicButton> active_event_buttons;
    //public ButtonLetter[] current_sequence;
    public List<ButtonLetter> current_sequence;
    float success_count;

    void Start() {
        sprites = Resources.LoadAll<Sprite>("Sprites/Fight/rythimic_event_ui2");

        ZAction.Enable();
        XAction.Enable();
        CAction.Enable();
    }

    void Update() {
        CheckSequence();
    }

    
    void CheckSequence() {
        if (active_event_buttons.Count == 0)
            return;

        var b = active_event_buttons[0];
        b.GetLetter();

        // if (ZAction.WasPressedThisFrame()) {
        //     if (b.CheckCollision(ZAction, ButtonLetter.Z)) success_count++;
        // }
        // if (XAction.WasPressedThisFrame()) {
        //     if (b.CheckCollision(XAction, ButtonLetter.X)) success_count++;
        // }
        // if (CAction.WasPressedThisFrame()) {
        //     if (b.CheckCollision(CAction, ButtonLetter.C)) success_count++;
        // }
    }

    public void TriggerEvent(float duration, int size, Action<(float, float)> onComplete) {
        SetSequence(size);
        StartCoroutine(EventSequence(duration, onComplete));
    }

    void SetSequence(int size) {
        success_count = 0;
        current_sequence.Clear();
        for (int i = 0; i < size; i++) {
            var letter = UnityEngine.Random.Range(0, 3);
            current_sequence.Add((ButtonLetter)letter);
        }
    }

    IEnumerator EventSequence(float duration, Action<(float, float)> onComplete = null) {
        var range = canvas.GetComponent<RectTransform>().rect.size/2;
        //print(range);
        foreach(ButtonLetter letter in current_sequence) {
            var b = GetNewButton(letter, duration);
            var posX = UnityEngine.Random.Range(-range.x, range.x+1);
            var posY = UnityEngine.Random.Range(-range.y, range.y+1);
            b.SetPosition(new(posX, posY));
            active_event_buttons.Add(b);
            b.StartEvent();
            yield return new WaitForSeconds(duration);
            if (b.successfullClick) success_count++;
            //if (!b.successfullClick) b.Fail();
        }

        float success_rate = success_count / current_sequence.Count;
        float fail_count = current_sequence.Count - success_count;
        //print("count" + current_sequence.Count + "success" + success_count); 
        active_event_buttons.Clear();
        onComplete?.Invoke((success_rate, fail_count));
    }

    // void AddButton(ButtonLetter letter, float duration = 0.6f) {
    //     GameObject event_button = Resources.Load<GameObject>("Prefabs/RythmicButton");
    //     RythmicButton button = Instantiate(event_button, transform).GetComponent<RythmicButton>();
    //     button.GetComponent<RythmicButton>().SetLetter(letter);
    //     button.GetComponent<RythmicButton>().SetDuration(duration);
    //     button.SetPosition(button.rest_position);
    //     active_event_buttons.Add(button);
    // }

    RythmicButton GetNewButton(ButtonLetter letter, float duration) {
        GameObject event_button = Resources.Load<GameObject>("Prefabs/RythmicButton");
        RythmicButton button = Instantiate(event_button, transform).GetComponent<RythmicButton>();
        button.GetComponent<RythmicButton>().SetLetter(letter);
        button.GetComponent<RythmicButton>().SetDuration(duration);
        button.SetPosition(button.rest_position);
        return button;
    }

}
