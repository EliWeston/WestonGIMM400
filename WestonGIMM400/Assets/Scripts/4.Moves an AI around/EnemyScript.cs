using UnityEngine;

public class EnemyScript : MonoBehaviour {

    //rigid body component
    Rigidbody rb;

    //Animator
    Animator anim;

    //chase distance
    public float chasingDistance = 20f;

    //Walking Speed
    public float speed = 0.5f;

    //available states
    enum State { idle, attacking, dead };

    //currnt state
    State currentState = State.idle;

    //player
    PlayerController player;

    //angular speed
    public float angSpeed = 1;

    //health
    public float health = 10;

    public bool dead = false;


    private void Awake()
    {
        //get component
        rb = GetComponent<Rigidbody>();

        //animator
        anim = GetComponentInChildren<Animator>();

        //get player
        player = FindObjectOfType<PlayerController>();

        if (player == null) Debug.LogError("there needs to be a player");

        //search for the player at an interval of .5 seconds
        InvokeRepeating("LookForPlayer", 0, 0.5f);
    }

    void LookForPlayer()
    {
        //only if idle
        if (currentState != State.idle) return;

        //Check distance
        if(Vector3.Distance(player.transform.position, transform.position) <= chasingDistance)
        {
            //chaneg state to attacking
            currentState = State.attacking;

            //activate animation
            anim.SetBool("sawPlayer", true);

            //cancel looking
            CancelInvoke();

            print("Saw player!");
        }
    }

    void FixedUpdate()
    {
        // move the parent to the position of the child (the model)
        transform.position = transform.GetChild(0).position;

        // set the child to be in the origin of the parent
        transform.GetChild(0).localPosition = Vector3.zero;

        //only chase if we are attacking
        if (currentState != State.attacking) return;

        if (health <= 0)
        {
            Die();
        }

        /* animation issue makes all of this not work
         * 
        //direction
        Vector3 dir = player.transform.position - transform.position;
        dir.Normalize();

        //velocity
        Vector3 vel = dir * speed;

        //set rb vel
        rb.velocity = vel;

        //rotation
        //transform.LookAt(player.transform.position);

        //rotate with ang vel
        //'flat differencee between player and enemy
        Vector3 flatDiff = player.transform.position - transform.position;
        flatDiff.y = 0;

        //rotation needed, in Quaternion
        Quaternion targetRotation = Quaternion.LookRotation(flatDiff, Vector3.up);

        // ang rot velocity vector
        Vector3 eulerAngleVelocity = new Vector3(0, angSpeed, 0);

        // delta rotation (v = d/ t ==> d = v * t)
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);

        //rigid body rotation
        rb.MoveRotation(targetRotation * deltaRotation);*/

        // instant rotation of the transform:
        transform.LookAt(player.transform.position);


    }

    private void OnTriggerEnter(Collider other)
    {
        //check if bullet
        if (other.CompareTag("Bullet"))
        {
            health--;

        }
    }

   public void ApplyDamage(float damage)
   {

    if (health - damage > 0)
     {

        //if (playHitSound)
       // this.audio.PlayOneShot(getHitSound);

                health -= damage;

            }
            else
            {

                health = 0;

                Die();
            }
        }

    public void Die()
    {

        dead = true;
        //change state to dead
        currentState = State.dead;

        //activate animation
        anim.SetBool("isAlive", false);

        //disable collider
        GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        Invoke("Destroy", 2);
    }

    void Destroy()
    {
        { Destroy(this.gameObject); }
    }
}
