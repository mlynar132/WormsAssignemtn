using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GradientScript : MonoBehaviour
{
    private Gradient _gradient;
    private GradientColorKey[] _colorKey;
    private GradientAlphaKey[] _alphaKey;
    private float _magnitude = 0;
    private bool _IsCharging = false;
    [SerializeField] private Renderer _cyl;
    [SerializeField] private Renderer _sph;
    [SerializeField] private float _chargingSpeed = 0.5f;
    void Start()
    {
        _gradient = new Gradient();

        _colorKey = new GradientColorKey[3];
        _colorKey[0].color = Color.green;
        _colorKey[0].time = 0.0f;
        _colorKey[1].color = Color.yellow;
        _colorKey[1].time = 0.5f;
        _colorKey[2].color = Color.red;
        _colorKey[2].time = 1.0f;

        _alphaKey = new GradientAlphaKey[3];
        _alphaKey[0].alpha = 1.0f;
        _alphaKey[0].time = 0.0f;
        _alphaKey[1].alpha = 0.0f;
        _alphaKey[1].time = 1.0f;
        _alphaKey[2].alpha = 0.0f;
        _alphaKey[2].time = 1.0f;

        _gradient.SetKeys(_colorKey, _alphaKey);
    }
    private void OnEnable()
    {
        StartCharging();
    }
    private void Update()
    {
        if (_IsCharging)
        {
            _magnitude += Time.deltaTime * _chargingSpeed;
            Mathf.Ceil(_magnitude);
        }

        _cyl.material.color = _gradient.Evaluate(_magnitude);
        _sph.material.color = _gradient.Evaluate(_magnitude);
    }
    private void StartCharging()
    {
        _magnitude = 0;
        _IsCharging = true;
    }
}
