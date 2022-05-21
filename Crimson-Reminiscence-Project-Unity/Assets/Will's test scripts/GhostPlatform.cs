using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("How long does this ghost platform stay solid?")] 
    float timeActive;

    [SerializeField]
    [Tooltip("How much time left when it starts warning player it's about to despawn?")]
    float timeDespawnWarning;

    float counter = 0f;
    bool counterIsActive = false;

    bool isWarningActivated = false;

    [SerializeField] bool isSolid = false;

    [SerializeField] Material defaultMaterial;
    [SerializeField] Material solidPlatformMaterial;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchState()
    {
        var collider = GetComponent<BoxCollider>();
        var mesh = GetComponentInChildren<Renderer>();
        if (collider == null)
        {
            print("Missing a collider!!");
            return;
        }
        if(mesh == null)
        {
            print("Missing mesh in child!!");
            return;
        }
        if (!isSolid)
        {
            isSolid = true;
            collider.isTrigger = false;
            mesh.material = solidPlatformMaterial;
            print("Is Solid");
        }
        else
        {
            isSolid = false;
            collider.isTrigger = true;
            mesh.material = defaultMaterial;
            isWarningActivated = false;
            //animator.SetTrigger("TrDefault");
            animator.SetBool("IsDespawning", false);
            print("NOT Solid");
        }
    }

    public void Activate()
    {
        if (isSolid) return;
        SwitchState();
        counter = timeActive;
        counterIsActive = true;
    }

    public void Update()
    {
        if (counter > 0f && counterIsActive)
        {
            counter -= Time.deltaTime;
        }
        if (counter <= timeDespawnWarning && counterIsActive && !isWarningActivated)
        {
            print("WARNING!! Despawning!!");
            //animator.SetTrigger("TrDespawning");
            animator.SetBool("IsDespawning", true);
            isWarningActivated = true;
        }
        if (counter <= 0f)
        {
            counterIsActive = false;
            counter = 0f;
            if (isSolid) 
                SwitchState();
        }
        //print(counter);
    }
}
