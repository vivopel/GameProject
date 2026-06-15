using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ButtonLetter {
    Z = 0, X = 1, C = 2, N = 3
};

public class RythmicButton : MonoBehaviour
{
    public Vector2 rest_position = new(0, 20);
    const float start_scale_ring = 2.0f;
    const float end_scale_ring = 0.4f;
    const float start_scale_center = 1f;
    const float end_scale_center = 2.0f;
    private float duration = 1.0f;
    public float click_window_duration;
    private bool time_ended = false;
    ButtonLetter letter;
    

    [SerializeField] private GameObject center;
    [SerializeField] private GameObject ring;
    private RythmicEventController rec;
    

    bool isClickWindowOpen = false;
    public bool successfullClick = false;

    void Awake() {
        rec = GetComponentInParent<RythmicEventController>();

        //click_window_duration = 0.4f;
    }

    void Update()
    {

        
    }

    public void SetDuration(float d) {
        duration = d;
        click_window_duration = 0.5f * duration;
    }

    public void SetLetter(ButtonLetter letter) {
        this.letter = letter;

        int center_id = (int)this.letter;
        int ring_id = center_id + 3;
       
        center.GetComponent<SpriteRenderer>().sprite = rec.sprites[center_id];
        ring.GetComponent<SpriteRenderer>().sprite = rec.sprites[ring_id];

        SetScaleRing(start_scale_ring);
    }

    public ButtonLetter GetLetter() {
        return this.letter;
    }

    public void SetPosition(Vector2 pos) {
        transform.localPosition = new(pos.x, pos.y, transform.position.z);
    }

    private void SetScaleRing(float scale) {
        ring.transform.localScale = new(scale, scale, scale);
    }

    private void SetScaleCenter(float scale) {
        center.transform.localScale = new(scale, scale, scale);
    }
    
    public void StartEvent() {
        StartCoroutine(MoveRing());
        StartCoroutine(CheckSuccess());
    }

    IEnumerator MoveRing() {
        float t = 0.0f;

        while(t < 1f) {
            t += Time.deltaTime/duration;
            float s = Mathf.Lerp(start_scale_ring, end_scale_ring, t);
            SetScaleRing(s);
            yield return null;
        }
    }

    public IEnumerator CheckSuccess(Action<bool> onComplete = null) {
        bool success = false;
        ButtonLetter choosen_letter = ButtonLetter.N;
        while (!isClickWindowOpen) {
            if (rec.ZAction.WasPressedThisFrame() || rec.XAction.WasPressedThisFrame() || rec.CAction.WasPressedThisFrame()){
                Fail();
                goto end;
            }
   
            yield return null;
        }
        while(isClickWindowOpen) {
            if (rec.ZAction.WasPressedThisFrame())
                choosen_letter = ButtonLetter.Z;
            if (rec.XAction.WasPressedThisFrame())
                choosen_letter = ButtonLetter.X;
            if (rec.CAction.WasPressedThisFrame())
                choosen_letter = ButtonLetter.C;

            if (choosen_letter != ButtonLetter.N) {
                if (choosen_letter == this.letter) {
                    success = true;
                    Success();
                    break;
                } else {
                    print("zla litera");
                    success = false;
                    Fail();
                    break;
                }
            }
            yield return null;
        }
        if (!success){
            Fail();
            time_ended = true;
            print("koniec czasu");
        }

        end:
        if (rec.active_event_buttons.Count > 0)
            rec.active_event_buttons.RemoveAt(0);
        onComplete?.Invoke(success);
    }

    // public bool CheckCollision(InputAction action, ButtonLetter choosen_letter) {
    //     bool success = false;
    //     if (isClickWindowOpen && choosen_letter == this.letter) {
    //         success = true;
    //         Success();
    //     } else {
    //         Fail();
    //     }
 
    //     rec.active_event_buttons.RemoveAt(0);
    //     return success;
    // }

    void Success() {
        center.GetComponent<SpriteRenderer>().sprite = rec.sprites[7];
        successfullClick = true;
        isClickWindowOpen = false;
        //SetPosition(rest_position);
    }
    
    public void Fail() {
        center.GetComponent<SpriteRenderer>().sprite = rec.sprites[6];
        isClickWindowOpen = false;
        //SetPosition(rest_position);
    }

    public IEnumerator OpenClickWindow(float time) {
        isClickWindowOpen = true;
        yield return new WaitForSeconds(time);
        isClickWindowOpen = false;
        if (time_ended) {
            yield return new WaitForSeconds(0.2f);
        }
        SetPosition(rest_position);
    }

}
