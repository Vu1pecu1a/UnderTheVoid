using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowLate : MonoBehaviour
{
    [SerializeField]    
    GameObject Target;
    [SerializeField]
    PlayerBase PlayerBasePlayer;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerBasePlayer.target != null)
            Target = PlayerBasePlayer.target.gameObject;

        if (Target != null)
            this.transform.position = Target.transform.position;
        //this.transform.position = Vector3.Lerp(transform.position,Target.transform.position,Time.deltaTime);
    }
}
