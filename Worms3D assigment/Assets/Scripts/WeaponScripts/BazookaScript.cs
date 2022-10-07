using UnityEngine;

public class BazookaScript : MonoBehaviour, IWeapon
{
    [SerializeField] private int _damage;
    [SerializeField] private float _projectailForce;
    [SerializeField] private float _radiusOfExplosion;
    [SerializeField] private float _chargingSpeed;
    [SerializeField] private GameObject _projectailPrefab;
    [SerializeField] private GameObject _charger;

    public void Shoot(float chargeTime)
    {
        GameObject bullet = Instantiate(_projectailPrefab, gameObject.transform.GetChild(0).position, gameObject.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * _projectailForce * chargeTime * 100);
        AreaDamageProjectile bulletDamgeScript = bullet.GetComponent<AreaDamageProjectile>();
        bulletDamgeScript.Damage = _damage;
        bulletDamgeScript.RadiusOfExplosion = _radiusOfExplosion;
        _charger.SetActive(false);
    }
    public void Aim(float input)
    {
        transform.localRotation *= Quaternion.Euler(new Vector3(input, 0, 0));
        _charger.SetActive(true);
    }
}