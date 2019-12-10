using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets._Scripts.EliStuff.Bugs.Interfaces;
using Assets._Scripts.EliStuff.Bugs.States;

namespace Assets._Scripts.EliStuff.Bugs.States
{
    public class WanderState : MonoBehaviour, IBugState
    {
        private Bug bug;

            public WanderState(Bug bugRef)
        {
            bug = bugRef;
        }


        public void Enter()
        {
            
            bug.landDelay = 7.0f;
            bug.nearPlant = false;
            Debug.Log("wanderState");
            Wander(); 
        }

        public void BugStateUpdate()
        {
            bug.landDelay -= Time.deltaTime;

            bug.transform.position += bug.transform.TransformDirection(Vector3.forward) * bug.speed * Time.deltaTime;
            if ((bug.transform.position - bug._wayPoint).magnitude < 3 && !bug.nearPlant)
            {
                // when the distance between us and the target is less than 3
                // create a new way point target
                Wander();
            }
            if ((bug.transform.position - bug._wayPoint).magnitude < .5f && bug.nearPlant)
            {
                Debug.Log("We're close to the flower");
                bug.SwitchState(new LandState(bug));
            }
        }

        void Wander()
        {
            // does nothing except pick a new destination to go to

            bug.speed = Random.Range(2, 10);
            bug._wayPoint = new Vector3(Random.Range(bug.transform.position.x - bug.range, bug.transform.position.x + bug.range),
            bug.randomHeight, Random.Range(bug.transform.position.z - bug.range, bug.transform.position.z + bug.range));
            bug._wayPoint.y = bug.randomHeight;
            // don't need to change direction every frame seeing as you walk in a straight line only
            bug.transform.LookAt(bug._wayPoint);
        }

        public void OnTriggerEnter(Collider col)
        {
            if (bug.landDelay <= 0)
            {
                if (!bug.nearPlant)
                {
                    if (col.gameObject.tag == "PurpPlant" || col.gameObject.tag == "Plant")
                    {
                        bug._wayPoint = new Vector3(col.transform.position.x, col.transform.position.y + 1, col.transform.position.z);
                        bug.transform.LookAt(bug._wayPoint);
                        bug.nearPlant = true;
                        col.GetComponent<Collider>().enabled = false;
                        if (col.gameObject.tag == "PurpPlant")
                        {
                            Debug.Log("Purple");
                            bug.GetComponent<Renderer>().material = bug.purpSkin;
                            bug.bugLight.color = Color.magenta;
                            bug.colorString = "Purple";
                        }
                        if (col.gameObject.tag == "Plant")
                        {
                            Debug.Log("White");
                            bug.GetComponent<Renderer>().material = bug.whiteSkin;
                            bug.bugLight.color = Color.white;
                            bug.colorString = "White";

                        }
                    }
                }
            }
            /*if (col.gameObject.tag == "FlowerLandPoint")
            {
                Debug.Log("made it to flower");
                bug.SwitchState(new LandState(bug));

            }*/
        }



        public void Exit()
        {
           // Vector3 lookUpRandom = new Vector3(Random.Range(0, 360), 2.8f, Random.Range(0, 360));
          //  bug.transform.LookAt(lookUpRandom);
        }
    }

}
