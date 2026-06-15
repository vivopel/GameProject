using UnityEngine;

public class EntityBattleController : MonoBehaviour {

    protected const float BASE_DAMAGE = 5.0f;
    protected virtual float MAX_HP => 10.0f;


    [SerializeField] protected GameObject value_display;
    [SerializeField] protected RythmicEventController re;

    public bool isDead = false;
    public float current_hp;

    protected virtual void Start() {
        TurnManager.instance.OnStateChange += OnTurnChange;
        current_hp = MAX_HP;
    }

    void Update()
    {
        
    }

    protected virtual void OnTurnChange(TurnState state, TurnState last_state) {}
}
