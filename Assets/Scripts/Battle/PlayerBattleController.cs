using TMPro;
using UnityEngine;

public class PlayerBattle : EntityBattleController
{
    protected override float MAX_HP => 60.0f;
    [SerializeField] private BossController boss;
    protected override void Start() {
        base.Start();
    }

    void Update() {
        
    }

    public void SetHp(float value) {
        var new_hp = current_hp += value; 
        print("new hp: " + new_hp);
        if (new_hp > MAX_HP) {
            new_hp = MAX_HP;
        } else if (new_hp < 0.0f) { 
            new_hp = 0.0f;
            isDead = true;
        }
        current_hp = new_hp;

        TextMeshProUGUI t = value_display.GetComponentInChildren<TextMeshProUGUI>();
        t.SetText(current_hp + "hp");
    }

    protected override void OnTurnChange(TurnState state, TurnState last_state) {
        if (state == TurnState.PlayerTurn && last_state == TurnState.None) {
            
        }
        if (state == TurnState.None && last_state == TurnState.PlayerTurn) {
            
        }
    }

    public void Attack(int attacks, float duration) {
        re.TriggerEvent(duration, attacks, result => {
            float dmg = -BASE_DAMAGE * (attacks - result.Item2);
            if (result.Item1 > 0.9f) dmg *= 2.0f;
            print("Hits:" + (attacks - result.Item2) + "Dmg to enemy: " + dmg + "Rate: " + result.Item1);
            boss.SetHp(dmg);
        });
    }
}
