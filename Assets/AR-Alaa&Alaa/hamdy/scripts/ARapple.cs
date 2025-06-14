using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ARapple : MonoBehaviour
{
    Animator animator;
    int isFallingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isFallingHash = Animator.StringToHash("falling");
    }

    // Update is called once per frame
    void Update()
    {
        bool falling = animator.GetBool(isFallingHash);
        bool fallingPerssed = Input.GetKey("w");
        if (!falling && fallingPerssed)
        {
            animator.SetBool(isFallingHash, true);
        }
        if (falling && !fallingPerssed)
        {
            animator.SetBool(isFallingHash, false);
        }
    }
}
