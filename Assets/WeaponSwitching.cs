using UnityEngine;
using System.Collections;

public class WeaponSwitching : MonoBehaviour {

	public Animation am;
	public AnimationClip undraw;

	public GameObject[] weapons;
	int index = 0;
    int _cooldown = 0;
    int weaponToSelect = 0;

    void Start()
	{ 
		StartCoroutine(switchW (0f, 0));
	}

	void Update()
	{
        if (_cooldown > 0)
            _cooldown--;

        if (Input.GetAxis ("Mouse ScrollWheel") != 0 && _cooldown <= 0)
		{
            _cooldown = 10;
            weaponToSelect++;
            if (weaponToSelect > weapons.Length -1)
                weaponToSelect = 0;

            switchWeapon(weaponToSelect);
		}
	}

	void switchWeapon(int _index)
	{

		ShotEffectsManager _w = weapons [index].GetComponent <ShotEffectsManager> ();
		AnimationClip _undraw = _w.undrawA;
		if(_undraw == null)
		{
			am.CrossFade (undraw.name);
			_undraw = undraw;
		}
		else
			_w.am.CrossFade (_undraw.name);

		index = _index;
		StartCoroutine (switchW (_undraw.length, index));
	}

	IEnumerator switchW(float seconds, int _index)
	{
		yield return new WaitForSeconds (seconds);
		foreach(GameObject o in weapons)
		{
			o.SetActive (false);
		}
		weapons [index].SetActive (true);
        Player.currentWeapon = weapons[index];

    }
}
