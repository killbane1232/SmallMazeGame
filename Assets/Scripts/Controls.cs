using UnityEngine;
using UnityEngine.AI;

public class Controls : MonoBehaviour
{
    public int Speed;
    public Camera cam;
    public NavMeshAgent agent;
    public Vector3 Difference;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent.updateRotation = false;
        Difference = cam.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = Difference + transform.position;

        var direction = new Vector3(0, 0, 0);
        direction.z = Input.GetAxis("Horizontal") * Speed;
        direction.x = Input.GetAxis("Vertical") * -Speed;
        var rigid = this.GetComponent<Rigidbody>();
        rigid.velocity = direction;

        var velocity = rigid.velocity;
        if (velocity.magnitude > 0.1)
        {
            animator.SetBool("IsWalking", true);
            agent.isStopped = true;
            transform.LookAt(transform.position + velocity);
        }
        else
        if (agent.velocity.magnitude > 0.001)
        {
            transform.LookAt(transform.position + agent.velocity);
            animator.SetBool("IsWalking", true);
        }
        else
            animator.SetBool("IsWalking", false);

        if (Input.GetMouseButtonDown(0))
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.isStopped = false;
                agent.SetDestination(hit.point);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.gameObject.name.Contains("Grass"))
            {
                Debug.Log(contact.otherCollider.gameObject.name);
                Application.Quit();
            }
        }
    }
}
