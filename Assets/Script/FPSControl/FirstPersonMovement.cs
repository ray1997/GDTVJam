using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    Vector2 velocity;

    private void OnEnable()
    {
        PlayerInput.OnPlayerMovementPerformed += MovePlayer;
        PlayerInput.OnPlayerMovementCanceled += StopMovePlayer;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerMovementPerformed -= MovePlayer;
        PlayerInput.OnPlayerMovementCanceled -= StopMovePlayer;
    }

    private void MovePlayer(Vector2 direction)
    {
        InputDirection = direction;
    }

    private void StopMovePlayer()
    {
        InputDirection = Vector2.zero;
    }

    Vector2 InputDirection;
    void FixedUpdate()
    {
        velocity.y = InputDirection.y * speed * Time.deltaTime;
        velocity.x = InputDirection.x * speed * Time.deltaTime;
        //velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        //velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
    }
}
