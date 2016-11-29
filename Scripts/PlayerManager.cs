using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class PlayerManager : NetworkBehaviour {

    [SerializeField]
    private int maxHealth = 100;
    [SyncVar] //assim todos os clientes recebe o valor da vida atualizado
    private int currentHealth;
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }
    Animator anim;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool [] wasEnabled;

   public void SetUp()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;

        }

        SetDefaults();
  
    }
    [ClientRpc] 
    public void RpcTakeDamage(int _amount)
    {
        if (isDead)
            return;
        currentHealth -= _amount;

        Debug.Log(transform.name + "tem" + currentHealth + " de vida");

        if (currentHealth <= 0)
            Die();
    }
    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];

        }
    }

    private void Die()
    {

        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;

        }
        //tirando alguns componentes
        anim = GetComponent<Animator>();

        anim.SetBool("isDead", true);
        Debug.Log(transform.name + "Está morto");

        //chamando spawn
        StartCoroutine(Respawn());

    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        anim.SetBool("isDead", false);
        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;

        transform.rotation = _spawnPoint.rotation;

      
    }

}

