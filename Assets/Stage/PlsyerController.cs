using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlsyerController : MonoBehaviour
{
    public float speed = 1.0f;
    private NavMeshAgent agent;
    private float velocity;
    private Vector3 direction;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        direction = transform.position;
        agent.speed = speed * 3.5f;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = ((transform.position - direction).magnitude) / Time.deltaTime;
        direction = transform.position;

        if (Input.touchCount == 1)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(screenRay, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        anim.SetFloat("characterSpeed", velocity);
    }
}
