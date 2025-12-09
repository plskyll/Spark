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
    private void Update()
    {
        FollowMousePosition();
    }
    public Flashlight GetActiveWeapon()
    {
        return flashlight;
    }
    
    private void FollowMousePosition()
    {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        transform.rotation = Quaternion.Euler(0, mousePos.x < playerPosition.x ? 180 : 0, 0);
    }
}
