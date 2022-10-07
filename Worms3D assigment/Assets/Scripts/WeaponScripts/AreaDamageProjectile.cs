using UnityEngine;

public class AreaDamageProjectile : MonoBehaviour
{
    [SerializeField] public int Damage;
    [SerializeField] public float RadiusOfExplosion;
    [SerializeField] private GameObject _explosionPrefab;
    private IKillable _interfaceOfTarget;
    private void OnCollisionEnter(Collision collision)
    {
        ExpolsionMethod();
        Destroy(gameObject);
    }
    private void ExpolsionMethod()
    {
        Collider[] tempColiders = Physics.OverlapSphere(gameObject.transform.position, RadiusOfExplosion);
        for (int j = 0; j < tempColiders.Length; j++)
        {
            _interfaceOfTarget = tempColiders[j].GetComponent<IKillable>();
            if (_interfaceOfTarget != null)
            {
                _interfaceOfTarget.TakeDamage(Damage);
            };
        }
        GameObject explosion = Instantiate(_explosionPrefab, gameObject.transform.position, Quaternion.identity);
        explosion.transform.localScale = Vector3.one * RadiusOfExplosion * 2/* default radius of sphere is 0.5*/;
        Destroy(explosion, 0.5f);
    }
}
