using UnityEngine;

public class BaseballBatScript : MonoBehaviour, IWeapon
{
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    [SerializeField] private Collider _hitBox;
    [SerializeField] private Animator _animator;
    private IKillable _interfaceOfTarget;
    private bool _isHitting;
    public void Start()
    {
        _hitBox.enabled = false;
    }
    public void Shoot(float chargeTime)
    {
        _animator.SetBool("Hitting", true);
        _hitBox.enabled = true;
        //the higher charhe time the faster animation goes and the bigger is push power
    }
    public void Aim(float input)
    {
        //You don't aim with a Baseball bat
        //change x rotation to go for higerballs
    }
    private void OnCollisionEnter(Collision collision)
    {
        _interfaceOfTarget = collision.gameObject.GetComponent<IKillable>();
        if (_interfaceOfTarget != null)
        {
            _interfaceOfTarget.TakeDamage(_damage);
            Vector3 direction = collision.GetContact(0).point - transform.position;
            direction = direction.normalized;
            direction.z = -direction.z;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(direction*_pushPower);
        }
    }
    private void Update()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BaseballBatShoot"))
        {
            _isHitting = true;
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("BaseballBatIdle") && _isHitting)
        {
            _isHitting = false;
            _animator.SetBool("Hitting", false);
            _hitBox.enabled = false;
        }
    }
}