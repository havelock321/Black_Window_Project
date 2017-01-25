using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class WeaponAnimtions : MonoBehaviour {

	public Animation am;
	public AnimationClip idle;
	public AnimationClip walk;
	public AnimationClip run;
	public AnimationClip undraw;
    public  static WeaponAnimtions instance;
    public bool blnIsMoving;
    public bool blnIsRunning;


    void Awake()
    {

        instance = this;
    }

   
	void Update()
	{
		if (am.IsPlaying (undraw.name))
			return;

		if(Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D))
		{
            blnIsMoving = true;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                am.CrossFade(run.name);
                blnIsRunning = true;
            }
            else
                am.CrossFade(walk.name);
		}
		else
			am.CrossFade (idle.name);

        blnIsMoving = false;
        blnIsRunning = false;
    }

}
