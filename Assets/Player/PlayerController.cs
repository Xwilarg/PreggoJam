using PreggoJam.Manager;
using PreggoJam.SO;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PreggoJam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody2D _rb;
        private float _movX;
        private bool _canJump = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = new Vector2(_movX * _info.Speed, _rb.linearVelocity.y);
        }

        private void Update()
        {
            if (transform.position.y < -10f)
            {
                transform.position = Vector2.zero;
                _rb.linearVelocityY = 0f;
            }
        }

        private void OnDrawGizmos()
        {
            // Debug jump box
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position - Vector3.up, new Vector3(1f, .2f, 1f));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(-500f, -10f), new Vector2(500f, -10f));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Potion"))
            {
                ProgressionManager.Instance.GrabPotion();
                Destroy(collision.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("House"))
            {
                ProgressionManager.Instance.WarningText.SetActive(true);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("House"))
            {
                ProgressionManager.Instance.WarningText.SetActive(false);
            }
        }

        private IEnumerator PlayJumpCooldown()
        {
            _canJump = false;
            yield return new WaitForSeconds(_info.JumpCooldown);
            _canJump = true;
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _movX = value.ReadValue<Vector2>().x;
            if (_movX < 0f) _movX = -1f;
            else if (_movX > 0f) _movX = 1f;
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.phase == InputActionPhase.Started && _canJump)
            {
                // Check for floor under player
                var under = Physics2D.OverlapBox((Vector2)transform.position - Vector2.up, new Vector2(1f, .2f), 0f, LayerMask.GetMask("Map"));
                if (under != null)
                {
                    _rb.AddForce(Vector2.up * _info.JumpForce, ForceMode2D.Impulse);
                    StartCoroutine(PlayJumpCooldown());
                }
            }
        }
    }
}
