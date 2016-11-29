using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{

    public string name = "Glock";

    public int damge = 10;
    public float range = 100f;
    // Use this for initialization

    public float fireRate = 0f;

    public GameObject[] graphics;

}
