using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IKillable
{
    [SerializeField] private int _maxHp;
    [SerializeField] private float _mass;
    private int _hp;
    private void Start()
    {
        _hp = _maxHp;
        gameObject.GetComponent<Rigidbody>().mass = _mass;
    }
    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hp <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        Destroy(gameObject);
    }
}