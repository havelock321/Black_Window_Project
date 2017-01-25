using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
public class Weapon : NetworkBehaviour {

	[Header("ANIMATIONS")]
	public Animation am;
	public AnimationClip undrawA;
	public AnimationClip fireA;
	public AnimationClip reloadA;
    public AnimationClip scopeA;


    [Header("AUDIO")]
    public AudioSource aSource;
    public AudioClip soundDraw;
    public AudioClip soundFire;
    public AudioClip soundReload;
    public AudioClip soundEmpty;
    public AudioClip switchModeSound;
    public AudioClip soundPullOut;
    public AudioClip soundPullIn;

    [Header("ATTRIBUTES")]
	public int totalAmmo = 120;
	public int clipSize = 30;
    [SerializeField]
    public int ammo;
	public int cooldown = 11;
    public float dellayPullOut = 0;
    public float dellayPullIn = 0;
    [SerializeField]
    Transform firePosition;

    public Weapon.fireMode mode;
	public int _cooldown = 0;

	[Header("RAYCAST")]
	public Camera cam;
	public ParticleSystem muzzleFlash;
    public GameObject hit;

    [Header("CROSSHAIR")]
    public float crossHairSize;

    private Recoil recoilComponent;
    public static Weapon instance;
	public enum fireMode
	{
		AUTOMATIC,
		SEMI_AUTOMATIC
	}

    public static Weapon GetInstance()
    {
        
        return instance;

    }
    void Start()
	{
		
		InvokeRepeating ("UpdateCooldown", 0.01f, 0.01f);
    }

    void OnEnable()
    {
      
        instance = this;
        ammo = clipSize;
        UIScripts.instance.UpdateAmmo(ammo);
        UIScripts.instance.UpdateTotalAmmo(totalAmmo);
    }


    void UpdateCooldown()
	{
		if (_cooldown > 0)
			_cooldown--;
	}

	void reload()
	{
        if (ammo == clipSize)
            return;
       

		am.CrossFade (reloadA.name);

		if(totalAmmo >= (clipSize - ammo))
		{
			totalAmmo -= (clipSize - ammo);
			ammo += (clipSize - ammo);

            if (soundReload != null)
                aSource.PlayOneShot(soundReload);

            else if (soundPullIn != null && soundPullOut != null)
            {
                StartCoroutine(MyDelay(dellayPullOut, soundPullOut));
                StartCoroutine(MyDelay(dellayPullIn, soundPullIn));
            }
        }
		else
		{
			ammo += totalAmmo;
			totalAmmo = 0;
        }

        UIScripts.instance.UpdateAmmo(ammo);
        UIScripts.instance.UpdateTotalAmmo(totalAmmo);
	}
    IEnumerator MyDelay(float _delay, AudioClip sound)
    {
        // Do something
        
        if (_delay != 0)
        yield return new WaitForSeconds(_delay);  // Wait three seconds
                                                  // Do something else
       
        aSource.PlayOneShot(sound);
    }

	void Update()
	{

        //Debug.Log (ammo + " " + clipSize + " " + totalAmmo);
        if (Input.GetMouseButton(1))
        {
            am.Stop();
            am.Play(scopeA.name);

        }
        if (mode == Weapon.fireMode.AUTOMATIC && Input.GetMouseButton (0) && _cooldown <= 0)
		{
            //PlayerShoot.instance.Cmdfire(firePosition.position, firePosition.forward);

		}
		else if(mode == Weapon.fireMode.SEMI_AUTOMATIC && Input.GetMouseButtonDown (0) && _cooldown <= 0)
		{
            //PlayerShoot.instance.Cmdfire(firePosition.position, firePosition.forward);
		}

		if(!am.IsPlaying (fireA.name) && ((undrawA != null && !am.IsPlaying (undrawA.name)) || undrawA == null) && !am.IsPlaying (reloadA.name) && Input.GetKeyDown (KeyCode.R))
		{
			reload ();
		}
	}
}
