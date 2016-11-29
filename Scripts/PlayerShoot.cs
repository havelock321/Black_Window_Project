using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnitySampleAssets.Characters.ThirdPerson;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";


   
    public Camera player;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    public ThirdPersonCharacter character;


    // Use this for initialization
    void Start()
    {

        weaponManager = GetComponent<WeaponManager>();
        character = GetComponent<ThirdPersonCharacter>();

    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
    
        if (currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot",0f,1f /currentWeapon.fireRate); //um loop com ose fosse arma automatica
            }
            else if(Input.GetButtonUp("Fire1"))
            {
             
                CancelInvoke("Shoot"); //assim que para de clicar o blotão do mouse ele tem que cancelar o evento
                character.onMouse = false;
            }

        }


    }

    [Client] //lado do cliente
    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, currentWeapon.range, mask))
        {
            //a gente atirou em algo
            character.onMouse = true;
            new WaitForSeconds(10);
          Debug.Log("Acertou o Player!" + hit.collider.name);
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name, currentWeapon.damge);
            }
            character.onMouse = false;
        }
    }

    [Command] //lado do servidor
    void CmdPlayerShot(string _PLAYERID, int dmg)
    {
        Debug.Log(_PLAYERID + "foi atingido");
        PlayerManager _PlayerMN = GameManager.GetPlayer(_PLAYERID);

        _PlayerMN.RpcTakeDamage(dmg);
        //GameObject.Find(_ID);
    }


}
