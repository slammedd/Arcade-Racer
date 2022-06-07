using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdReaction : MonoBehaviour
{
    public Animator[] crowdAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(Animator anim in crowdAnim)
            {
                anim.SetBool("Reacting", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Animator anim in crowdAnim)
            {
                anim.SetBool("Reacting", false);
            }
        }
    }

}
