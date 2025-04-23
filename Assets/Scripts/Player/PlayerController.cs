using DG.Tweening;
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

        private float _externalX;
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            float xVel;
            if (CanJump)
            {
                xVel = _movX;
            }
            else // Air control
            {
                var modXDir = _rb.linearVelocity.normalized.x;
                if (modXDir > .1f) modXDir = 1f;
                else if (modXDir < -.1f) modXDir = -1f;
                xVel = (_movX * _info.AirtimeControl) + (modXDir * (1 - _info.AirtimeControl));
            }
            _rb.linearVelocity = GameManager.Instance.CanPlay ? new Vector2((xVel + (_externalX * _info.ExternalForce)) * _info.Speed, _rb.linearVelocity.y) : Vector2.up * _rb.linearVelocity.y;
        }

        private void Update()
        {
            if (transform.position.y < -10f)
            {
                LooseHealth();
            }
            if (_externalX > 0f) _externalX = Mathf.Clamp(_externalX - Time.deltaTime / 2f, 0f, _externalX);
            else _externalX = Mathf.Clamp(_externalX + Time.deltaTime / 2f, _externalX, 0f);
        }

        private void OnDrawGizmos()
        {
            // Debug jump box
            Gizmos.color = CanJump ? Color.blue : Color.red; Gizmos.DrawWireCube(transform.position - Vector3.up, new Vector3(.75f, .3f, 1f));
            Gizmos.color = CanWallJumpLeft ? Color.blue : Color.red; Gizmos.DrawWireCube(transform.position + Vector3.right * -.5f, new Vector3(.2f, .75f, 1f));
            Gizmos.color = CanWallJumpRight ? Color.blue : Color.red; Gizmos.DrawWireCube(transform.position + Vector3.right * .5f, new Vector3(.2f, .75f, 1f));

            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(-500f, -10f), new Vector2(500f, -10f));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Potion"))
            {
                ProgressionManager.Instance.GrabPotion();
                Tween potionTween = transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), .3f, 2, 0.1f);
                if(!potionTween.IsPlaying()) potionTween.Play();
                Destroy(collision.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("House"))
            {
                if (ProgressionManager.Instance.IsProgressionFull)
                {
                    ResetPlayer();
                    GameManager.Instance.CanPlay = false;
                    ProgressionManager.Instance.UpdateBelly();
                }
                else
                {
                    ProgressionManager.Instance.WarningText.SetActive(true);
                }
            }
            else if (collision.gameObject.CompareTag("PlayerReset"))
            {
                _externalX = 0f;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("House"))
            {
                ProgressionManager.Instance.WarningText.SetActive(false);
            }
        }

        public void LooseHealth()
        {
            ResetPlayer();

            // Out of health
            GameManager.Instance.CanPlay = false;
            ProgressionManager.Instance.UpdateBelly();
        }

        public void ResetPlayer()
        {
            transform.position = Vector2.zero;
            _externalX = 0f;
        }

        private bool CanJump => Physics2D.OverlapBox((Vector2)transform.position - Vector2.up, new Vector2(.75f, .3f), 0f, LayerMask.GetMask("Map")) != null;
        private bool CanWallJumpLeft => Physics2D.OverlapBox((Vector2)transform.position - Vector2.right * .5f, new Vector3(.2f, .75f, 1f), 0f, LayerMask.GetMask("Map")) != null;
        private bool CanWallJumpRight => Physics2D.OverlapBox((Vector2)transform.position + Vector2.right * .5f, new Vector3(.2f, .75f, 1f), 0f, LayerMask.GetMask("Map")) != null;

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
            if (value.phase == InputActionPhase.Started && _canJump && GameManager.Instance.CanPlay)
            {
                // Check for floor under player
                if (CanJump)
                {
                    _rb.linearVelocity = Vector2.up * _info.JumpForce;
                    StartCoroutine(PlayJumpCooldown());
                }
                else if (CanWallJumpLeft)
                {
                    var dir = new Vector2(1f, 1f).normalized * 1.2f;
                    _rb.linearVelocity = Vector2.up * dir.y * _info.JumpForce;
                    _externalX = dir.x;
                    StartCoroutine(PlayJumpCooldown());
                }
                else if (CanWallJumpRight)
                {
                    var dir = new Vector2(-1f, 1f).normalized * 1.2f;
                    _rb.linearVelocity = Vector2.up * dir.y * _info.JumpForce;
                    _externalX = dir.x;
                    StartCoroutine(PlayJumpCooldown());
                }
            }
        }
    }
}
