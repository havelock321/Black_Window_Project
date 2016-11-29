using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class WeaponManager : NetworkBehaviour {

    // Use this for initialization


    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Transform[] weaponHolder;

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;


    void Start () {
       
        EquipWeapon(primaryWeapon);

	}
	
    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

        for (int i = 0; i < weaponHolder.Length; i++)
        {
            GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics[i], weaponHolder[i].position, weaponHolder[i].rotation); //instanciando a arma
            _weaponIns.transform.SetParent(weaponHolder[i]);

            if (isLocalPlayer)
            {
                //SetLayerRecursively(GameObject obj, int newLayer);
                _weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);

            }

        }
    
    }
	// Update is called once per frame
	void Update () {
	
	}

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;

    }


    void SetLayerRecursively(GameObject obj, int newLayer)
    {

        obj.layer = newLayer;


        foreach (Transform child in obj.transform)
        {
            // child.gameObject.layer = newLayer;
            SetLayerRecursively(child.gameObject, newLayer); //chama ela mesma
        }
    }
}
