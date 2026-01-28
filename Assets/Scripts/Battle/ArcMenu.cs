using System.Collections.Generic;
using UnityEngine;

public class ArcMenu : MonoBehaviour
{
    [SerializeField] float radius = 0.30f;
    //[SerializeField] float start_angle = 58f;
    //[SerializeField] float end_angle = 205f;
    [SerializeField] Transform player_pivot;

    private Dictionary<int, (float start_angle, float end_angle)> buttonSpacing = new Dictionary<int, (float, float)>
    {
        {5, (58f, 205f)},
        {4, (58f, 205f)},
        {3, (45f, 180f)},
        {2, (40f, 100f)},
        {1, (20, 20)}
    };

    const float min_radius = 0.0f;
    const float max_scale = 1.0f;
    const float min_scale = 0.0f;
    public float duration = 1f;

    void Start()
    {
        TurnManager.instance.OnStateChange += OnTurnChange;
        transform.position = player_pivot.position;
        SetLayout(0.0f, 0.0f);
    }

    void Update()
    {
        //SetLayout(radius, 1.0f);
    }

    void OnTurnChange(TurnState state, TurnState last_state)
    {
        if (state == TurnState.PlayerTurn) {
            StartCoroutine(ChangeLayout(min_radius, radius, min_scale, max_scale));
        } else if (last_state == TurnState.PlayerTurn)  {
            StartCoroutine(ChangeLayout(radius, min_radius, max_scale, min_scale));
        } else if (last_state == TurnState.PlayerTurn && state == TurnState.PlayerTurn) {
            SetLayout(radius, max_scale);
        } else {
            SetLayout(min_radius, min_scale);
        }
    }

    System.Collections.IEnumerator ChangeLayout(float start_radius, float end_radius, float start_scale, float end_scale)
    {
        float t = 0f;

        while(t < 1f) {
            t += Time.deltaTime/duration;
            float r = Mathf.Lerp(start_radius, end_radius, t);
            float s = Mathf.Lerp(start_scale, end_scale, t);

            SetLayout(r, s);
            yield return null;
        }
    }

    void SetLayout(float radius, float scale)
    {
        float start_angle = 0f, end_angle = 0f;
        int count = transform.childCount;
        if (buttonSpacing.TryGetValue(count, out var angles)) {
            start_angle = angles.start_angle;
            end_angle = angles.end_angle;
        }
        
        float step = (end_angle-start_angle)/count;

        for (int i = 0; i < count; i++) {
            RectTransform child = transform.GetChild(i) as RectTransform;
            float angle = start_angle - step * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
            child.anchoredPosition = pos;
            child.localScale = new Vector3(scale, scale, scale);
        }
    }
}
