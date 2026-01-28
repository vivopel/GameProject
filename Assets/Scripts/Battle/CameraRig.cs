using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public Transform enemy;
    [SerializeField] public Transform center;
    public float duration = 1f;
    public float arc_height = 2f;
    void Start()
    {
        TurnManager.instance.OnStateChange += OnTurnChange;
    }

    void OnDisable()
    {
        if(TurnManager.instance != null)
            TurnManager.instance.OnStateChange -= OnTurnChange;
    }

    void OnTurnChange(TurnState state, TurnState last_state)
    {
        Transform start;
        switch (last_state)
        {
            case TurnState.PlayerTurn: start = player; break;
            case TurnState.EnemyTurn: start = enemy; break;
            default: start = center; break;
        }
        Transform target;
        switch (state)
        {
            case TurnState.PlayerTurn: target = player; break;
            case TurnState.EnemyTurn: target = enemy; break;
            default: target = center; break;
        }

        StartCoroutine(MoveCameraLinearXZ(start.position, target.position));
    }

    System.Collections.IEnumerator MoveCameraLinearXZ(Vector3 start, Vector3 end)
    {
        float t = 0f;
        float startY = transform.position.y;
        
        while(t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 pos = Vector3.Lerp(start, end, t);
            pos.y = startY;
            transform.position = pos;
            yield return null;
        }
        
        Vector3 finalPos = end;
        finalPos.y = startY;
        transform.position = finalPos;
    }
}
