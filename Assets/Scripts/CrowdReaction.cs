using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrowdReaction : MonoBehaviour
{
    public Animator[] crowdAnim;
    private AudioSource source;

    private void Start()
    {
        if(GetComponent<AudioSource>() != null)
        {
            source = GetComponent <AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(Animator anim in crowdAnim)
            {
                anim.SetBool("Reacting", true);
            }

            if(source != null)
            {

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
