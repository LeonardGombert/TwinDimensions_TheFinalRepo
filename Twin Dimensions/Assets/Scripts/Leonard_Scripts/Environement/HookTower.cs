using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class HookTower : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> portalLighting = new List<ParticleSystem>();

    void Update()
    {
        if (this.gameObject.tag == "Inactive Hook Tower 1" || this.gameObject.tag == "Inactive Hook Tower 2")
        {
            foreach (ParticleSystem particle in portalLighting)
            {
                var em = particle.emission;
                em.enabled = false;
            }
        }

        if (this.gameObject.tag == "Hook Tower 1" || this.gameObject.tag == "Hook Tower 2")
        {
            foreach (ParticleSystem particle in portalLighting)
            {
                var em = particle.emission;
                em.enabled = true;
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (this.gameObject.tag == "Inactive Hook Tower 1")
            {
                if (PlayerInputManager.instance.GetKey("interactionKey"))
                {
                    GameObject manager;
                    manager = GameObject.FindGameObjectWithTag("Manager");
                    manager.SendMessage("GetAllHooks", this.gameObject);
                }
            }

            if (this.gameObject.tag == "Inactive Hook Tower 2")
            {
                if (PlayerInputManager.instance.GetKey("interactionKey"))
                {
                    GameObject manager;
                    manager = GameObject.FindGameObjectWithTag("Manager");
                    manager.SendMessage("GetAllHooks", this.gameObject);
                }
            }
        }
    }
}