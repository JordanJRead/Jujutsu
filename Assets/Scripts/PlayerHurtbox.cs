using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public float InvincibleDuration;
    public float ParryInvincibleDuration;

    Timer _stunTimer = new Timer();
    Timer _invincibilityTimer = new Timer();
    Animator _animator;
    PlayerActions _actions;
    // Start is called before the first frame update
    void Start()
    {
        _actions = GetComponent<PlayerActions>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_stunTimer.Update(Time.deltaTime))
        {
            _animator.SetBool("IsStunned", false);
        }

        _invincibilityTimer.Update(Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        Hitbox hitbox;
        if (_invincibilityTimer.IsDone() && _stunTimer.IsDone() && other.TryGetComponent(out hitbox) && hitbox.Owner != gameObject)
        {
            if (_animator.GetBool("IsBlocking"))
            {
                _animator.SetBool("Parry", true);
                _actions.SpawnPunchHitbox();
                _invincibilityTimer.ResetTime(ParryInvincibleDuration);
            }
            else
            {
                _stunTimer.ResetTime(hitbox.HitstunDuration);
                _invincibilityTimer.ResetTime(InvincibleDuration);

                if (!_stunTimer.IsDone())
                {
                    _animator.SetBool("IsStunned", true);
                }
            }
        }
    }
}
