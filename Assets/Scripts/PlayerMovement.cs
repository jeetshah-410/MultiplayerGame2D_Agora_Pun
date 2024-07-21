using Photon.Pun;

using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    public float speed = 5f;
  
    void Update()
    {
        if (photonView.IsMine)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new(moveHorizontal, moveVertical);

            transform.Translate(speed * Time.deltaTime * movement);
        }
    }
}
