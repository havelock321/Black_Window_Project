using UnityEngine;

public class ShotEffectsManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem muzzleFlash;
    [Header("AUDIO")]
    public AudioSource aSource;
    public AudioClip soundDraw;
    public AudioClip soundFire;
    public AudioClip soundReload;
    public AudioClip soundEmpty;
    public AudioClip switchModeSound;
    public AudioClip soundPullOut;
    public AudioClip soundPullIn;

    [Header("ANIMATIONS")]
    public Animation am;
    public AnimationClip undrawA;
    public AnimationClip fireA;
    public AnimationClip reloadA;
    public AnimationClip scopeA;


    [SerializeField]
    GameObject impactPrefab;

    ParticleSystem impactEffect;

    //Create the impact effect for our shots
    public void Initialize()
    {
        impactEffect = Instantiate(impactPrefab).GetComponent<ParticleSystem>();
      
    }

    //Play muzzle flash and audio
    public void PlayShotEffects()
    {
        muzzleFlash.Stop(true);
        muzzleFlash.Play(true);
        aSource.PlayOneShot(soundFire);
        am.Stop(fireA.name);
        am.Play(fireA.name);

    }

    //Play impact effect and target position
    public void PlayImpactEffect(Vector3 impactPosition)
    {
        impactEffect.transform.position = impactPosition;
        impactEffect.Stop();
        impactEffect.Play();
    }
}