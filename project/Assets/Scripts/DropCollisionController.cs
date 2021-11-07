using UnityEngine;

public class DropCollisionController : MonoBehaviour
{
    public PlayerStateManager player;

    void OnTriggerEnter(Collider other)
    {
        if (other != player.GetComponent<Collider>())
        {
            player.onDrop();
        }
    }
}