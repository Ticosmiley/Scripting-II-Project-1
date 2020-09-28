using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeMeter : MonoBehaviour
{
    private Slider _chargeMeter;
    private Image _chargeMeterImage;
    private Image _meterBorder;
    private Player _player;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _chargeMeter = GetComponent<Slider>();
        _chargeMeterImage = transform.GetChild(0).GetComponent<Image>();
        _meterBorder = transform.GetChild(1).GetComponent<Image>();

        _chargeMeterImage.enabled = false;
        _meterBorder.enabled = false;

        _chargeMeter.maxValue = 600;
    }

    private void Update()
    {
        _chargeMeter.value = _player.Power;
        if (_chargeMeter.value > 100)
        {
            _chargeMeterImage.enabled = true;
            _meterBorder.enabled = true;
        }
        else
        {
            _chargeMeterImage.enabled = false;
            _meterBorder.enabled = false;
        }
    }
}
