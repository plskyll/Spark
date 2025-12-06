using System;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public static ActiveWeapon Instance  { get; private set; }
    
    [SerializeField] private Flashlight flashlight;

    private void Awake()
    {
        Instance = this;
    }

    public Flashlight GetActiveWeapon()
    {
        return flashlight;
    }
}
