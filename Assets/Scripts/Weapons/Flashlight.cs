using System;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public event EventHandler OnFlashlightOn;
    
    public void Attack()
    {
        OnFlashlightOn?.Invoke(this, EventArgs.Empty);
    }
}
