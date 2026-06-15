using UnityEngine;

public class ButtonCenter : MonoBehaviour
{
    private RythmicButton button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        button = GetComponentInParent<RythmicButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {
        //print("trigger" + collider);
        StartCoroutine(button.OpenClickWindow(button.click_window_duration)); 
    }
}

