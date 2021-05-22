using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPanel : GameEventListener
{
    [SerializeField] private Color _color;
    [SerializeField] private Color _hitColor;
    [SerializeField] private float _flashSpeed;
    [SerializeField] private float _flashStay;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void Hit()
    {
        StartCoroutine(HitTimer());
    }

    private IEnumerator HitTimer()
    {
        _image.color = _hitColor;

        yield return new WaitForSeconds(_flashStay);

        _image.color = _color;
    }
}
