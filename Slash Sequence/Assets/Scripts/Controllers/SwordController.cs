using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    //sets swing variables
    //[SerializeField] allows private variables to show up in the inspector
    [SerializeField] private float swingSpeed = 12f;
    [SerializeField] private float swingStart = 180f;
    [SerializeField] private float swingEnd = 0f;
    Animator animator;

    //orient sword's rotation properly, then start the swing
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.eulerAngles = new Vector3(transform.parent.rotation.x, transform.parent.rotation.y + swingStart, transform.parent.rotation.z);
        transform.position = new Vector3(transform.position.x, 1.24f, transform.position.z);
        StartCoroutine(doSwing());
    }

    //rotate sword around player
    private IEnumerator doSwing()
    {
        animator.SetBool("isSwinging", true);
        for (int i = (int)swingStart; i > (int)swingEnd; i -= (int)swingSpeed)
        {
            if (!GMController.Instance.cameraZoom)
            {
                yield return null;
                float nexty = transform.parent.eulerAngles.y + swingStart - i;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, nexty, transform.eulerAngles.z);
            }
            else
            {
                i = (int)swingEnd;
            }
        }
        animator.SetBool("isSwinging", false);
        Destroy(gameObject);
    }
}
