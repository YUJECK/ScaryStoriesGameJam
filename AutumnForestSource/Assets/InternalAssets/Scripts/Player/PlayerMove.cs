using UnityEngine;
using UnityEngine.Events;

namespace AutumnForest
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 2.5f;
        private Vector2 movement;
        new private Rigidbody2D rigidbody2D;
        private PlayerInput playerInput;
        
        public UnityEvent<Vector2> OnMove { get; private set; } = new();
        public Vector2 Movement => movement;

        private void Awake()
        {
            playerInput = ServiceLocator.GetService<PlayerInput>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            movement = playerInput.Player.Move.ReadValue<Vector2>();
            OnMove.Invoke(movement);
            rigidbody2D.velocity = movement * moveSpeed;
        }
    }
}