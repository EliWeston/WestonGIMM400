using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidCollider : MonoBehaviour {

    public GameObject BoidObject;

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Ember":
                EmberCollision(other);
                break;
            case "BoidPUGreen":
                BoidGreenCollision(other);
                break;
            case "BoidPUBlue":
                BoidBlueCollision(other);
                break;
            case "BoidPURed":
                BoidRedCollision(other);
                break;
            case "BoidPUYellow":
                BoidYellowCollision(other);
                break;
            case "Boid":
                break;
            default:
                CheckunTaggedCollision(other);
                break;
        }
    }
    //Collides with the pickup, we get the script the controlls the logic
    //for the pickup and call collect from it
    private void EmberCollision(Collider other)
    {
        Ember ember = other.GetComponent<Ember>();
        ember.Collect();
        GameManager.Instance.CollectEmber();
    }

    private void BoidYellowCollision(Collider other)
    {
        //Debug.Log("Collided Yellow");
        BoidPickUp boidPU = other.GetComponent<BoidPickUp>();
        boidPU.Collect();
        GameManager.Instance.CollectBoid();
        BoidsManager.Instance.SpawnYellowBoid();
    }

    private void BoidGreenCollision(Collider other)
    {
        //Debug.Log("Collided Green");
        BoidPickUp boidPU = other.GetComponent<BoidPickUp>();
        boidPU.Collect();
        GameManager.Instance.CollectBoid();
        BoidsManager.Instance.SpawnGreenBoid();
    }

    private void BoidBlueCollision(Collider other)
    {
        //Debug.Log("Collided Blue");
        BoidPickUp boidPU = other.GetComponent<BoidPickUp>();
        boidPU.Collect();
        GameManager.Instance.CollectBoid();
        BoidsManager.Instance.SpawnBlueBoid();
    }

    private void BoidRedCollision(Collider other)
    {
        //Debug.Log("Collided Red");
        BoidPickUp boidPU = other.GetComponent<BoidPickUp>();
        boidPU.Collect();
        GameManager.Instance.CollectBoid();
        BoidsManager.Instance.SpawnRedBoid();
    }


    //Check the collided object if it doesn;t have a tag to see if it's
    //something we're also looking for.
    private void CheckunTaggedCollision(Collider other)
    {
            EnemyCollision();
    }

    //Destroy the player and set the the curren state to the dead state.
    private void EnemyCollision()
    {
        
        GameManager.Instance.ResetCollection();
        PlayerManager.Instance.GameOver();
        //GameUIManager.Instance.GameOver(gameObject);
        Destroy(BoidObject);
    }
}
