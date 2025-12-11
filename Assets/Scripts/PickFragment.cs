using UnityEngine;

public class PickFragment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.GetComponent<Player>() != null)
        {
            GameManager.Instance.FragmentCollected();

            Destroy(gameObject);
        }
    }
}