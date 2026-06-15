using System.Collections;
using TMPro;
using UnityEditor.Build;
using UnityEngine;

public class BossController : EntityBattleController
{
    protected override float MAX_HP => 666.0f;
    [SerializeField] private PlayerBattle player;
    int current_action;

    protected override void Start() {
        base.Start();
    }

    void Update() {
        
    }

    protected override void OnTurnChange(TurnState state, TurnState last_state) {
        if (state == TurnState.EnemyTurn && last_state == TurnState.None) {
            ChooseAction();
            print(current_action);
        }
        if (state == TurnState.None && last_state == TurnState.EnemyTurn) {
            StartCoroutine(DoAction());
        }
    }

    void ChooseAction() {
        if (current_hp < MAX_HP/2)
            current_action = Random.Range(0, 3);
        else {
            current_action = Random.Range(0, 2);
        }
    }

    IEnumerator DoAction() {
        yield return new WaitForSecondsRealtime(1.5f);
        switch(current_action) {
            case 0:
                print("Enemy Light Attacks");
                Attack(Random.Range(1, 4), 0.8f);
                break;
            case 1: 
                print("Enemy Heavy Attacks");
                Attack(Random.Range(4, 9), 1.0f);
                break;
            case 2:
                print("Enemy Heals");
                Heal();
                break;
        }
    }

    void Attack(int attacks, float duration) {
        re.TriggerEvent(duration, attacks, result => {
            var dmg = -BASE_DAMAGE * result.Item2;
            print("Faile count:" + result.Item2 + "Dmg to player: " + dmg);
            player.SetHp(dmg);
        });
    }

    void Heal() {
        var heal = Random.Range(1, 6);
        current_hp += heal;
    }

    public void SetHp(float value) {
        var hp = current_hp += value; 
        if (hp > MAX_HP) {
            hp = MAX_HP;
        } else if (hp < 0.0f) {
            hp = 0.0f;
            isDead = true;
        }
        current_hp = hp;

        TextMeshProUGUI t = value_display.GetComponentInChildren<TextMeshProUGUI>();
        t.SetText(current_hp + "hp");
    }
}
