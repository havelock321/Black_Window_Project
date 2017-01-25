using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField]
    float shotCooldown = .3f;
    [SerializeField]
    Transform firePosition;
    [SerializeField]
    ShotEffectsManager shotEffects;
    [SerializeField] int killToWin = 2;

    Player player;

    [SyncVar (hook = "OnScoreChanged")] int score;

    float ellapsedTime;
    bool canShoot;
    [SerializeField]
    PlayerShoot.fireMode _fireMode;

    [Header("RAYCAST")]
    public Camera cam;

    public enum fireMode
    {
        AUTOMATIC,
        SEMI_AUTOMATIC
    }

    void Start()
    {
        player = GetComponent<Player>();
        shotEffects.Initialize();

        if (isLocalPlayer)
            canShoot = true;
    }

    [ServerCallback]
    void OnEnabled()
    {
        score = 0;
    }

    void Update()
    {
        if (!canShoot)
            return;

        ellapsedTime += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && ellapsedTime > shotCooldown && PlayerShoot.fireMode.SEMI_AUTOMATIC == _fireMode) 
        {
            ellapsedTime = 0f;
            CmdFireShot(firePosition.position, firePosition.forward);
        }
        if (Input.GetButton("Fire1") && ellapsedTime > shotCooldown && PlayerShoot.fireMode.AUTOMATIC == _fireMode)
        {
            ellapsedTime = 0f;
            CmdFireShot(firePosition.position, firePosition.forward);
        }
    }

    [Command]
    void CmdFireShot(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;

         Ray ray = new Ray(origin, direction);
        
         //Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2 + Random.Range(-PlayerCanvas.instance.calculateCrossHair() / 1.4f, PlayerCanvas.instance.calculateCrossHair() / 1.4f), Screen.height / 2 + Random.Range(-PlayerCanvas.instance.calculateCrossHair() / 1.4f, PlayerCanvas.instance.calculateCrossHair() / 1.7f), 0f));

        Debug.DrawRay(ray.origin, ray.direction * 3f, Color.red, 1f);

        bool result = Physics.Raycast(ray, out hit, 50f);

        if (result)
        {
            PlayerHealth enemy = hit.transform.GetComponent<PlayerHealth>();

            if (enemy != null)
            {
                bool wasKillShot = enemy.TakeDamage();

                if (wasKillShot && ++score >= killToWin)
                    player.Won();


            }
        }

        RpcProcessShotEffects(result, hit.point);
    }

    [ClientRpc]
    void RpcProcessShotEffects(bool playImpact, Vector3 point)
    {
        shotEffects.PlayShotEffects();

        if (playImpact)
            shotEffects.PlayImpactEffect(point);
    }


    void OnScoreChanged(int value)
    {
        score = value;
        if (isLocalPlayer)
            PlayerCanvas.canvas.SetKills(value);
    }

    public void FireAsBot()

    {
        CmdFireShot(firePosition.position, firePosition.forward);
    }
}

