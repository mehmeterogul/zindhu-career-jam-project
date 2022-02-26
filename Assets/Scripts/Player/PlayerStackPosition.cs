using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStackPosition : MonoBehaviour
{
    [SerializeField] Transform stackPosition;
    [SerializeField] Transform playerModel;
    Animator animator;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = stackPosition.position;
        animator = GetComponent<Animator>();
    }

    public void FinishLevel()
    {
        Destroy(animator);
        transform.SetParent(playerModel);
        Destroy(GetComponent<PlayerStackPosition>());
    }

    public void Hit()
    {
        animator.SetTrigger("hit");
    }
}
