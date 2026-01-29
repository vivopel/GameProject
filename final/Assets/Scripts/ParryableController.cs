using System.Collections;
using UnityEngine;

public class ParryableController : MonoBehaviour {
    Animator animator;
    bool isStuned = false;
    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Stun(float duration) {
        isStuned = true;
        animator.speed = 0.0f;
        StartCoroutine(StunTimer(duration));
    }

    private IEnumerator StunTimer(float time) {
        yield return new WaitForSecondsRealtime(time);
        isStuned = false;
        animator.speed = 1.0f;
    }
}
