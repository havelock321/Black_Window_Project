using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScripts : MonoBehaviour {

    [Header("ATH")]
    public RectTransform healthBar;
    public Text health;
    public Text totalAmmo;
    public Text ammo;

    [Header("CROSSHAIR")]
    public RectTransform[] crosshairs;

    float walkSize;
    float size;
    public static UIScripts instance;

    void Start()
    {
        instance = this;
        walkSize = crosshairs[0].localPosition.y;
    }

    public void UpdateAmmo(int _ammo)
    {
        ammo.text = _ammo.ToString();
    }

    public void UpdateTotalAmmo(int _ammo)
    {
        totalAmmo.text = "/" + _ammo;
    }

    public void UpdateHealth(int _health)
    {
        float healthRatio = _health / 100f;

        healthBar.localScale = new Vector3(healthRatio, 0f, 0f);
        health.text = _health.ToString();
    }

    void Update()
    {
        UpdateCrosshair();
    }

    public void UpdateCrosshair()
    {
        float crossHairSize = calculateCrossHair();

        crosshairs[0].localPosition = Vector3.Slerp(crosshairs[0].localPosition, new Vector3(0f, crossHairSize, 0f), Time.deltaTime * 8f);
        crosshairs[1].localPosition = Vector3.Slerp(crosshairs[1].localPosition, new Vector3(crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[2].localPosition = Vector3.Slerp(crosshairs[2].localPosition, new Vector3(-crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[3].localPosition = Vector3.Slerp(crosshairs[3].localPosition, new Vector3(0f, -crossHairSize, 0f), Time.deltaTime * 8f);
    }

    public float calculateCrossHair()
    {
       

        if (Weapon.instance != null)
            size = walkSize * Weapon.instance.crossHairSize;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                size *= 2;
        }
        else
            size /= 2;

        return size;
    }
}
