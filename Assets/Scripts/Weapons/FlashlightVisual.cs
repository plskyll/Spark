using System;
using UnityEngine;

public class FlashlightVisual : MonoBehaviour
{
    [SerializeField] private Flashlight flashlight;
    private Animator animator;
    private const string ATTACK = "Attack";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        flashlight.OnFlashlightOn += Flashlight_OnFlashlightOn;
    }

    private void Flashlight_OnFlashlightOn(object sender, EventArgs e)
    {
        animator.SetTrigger(ATTACK);
    }

    public void TriggerEndAttackAnimation()
    {
        flashlight.AttackColliderTurnOff();
    }
}
