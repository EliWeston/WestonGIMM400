using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets._Scripts.EliStuff.Bugs.Interfaces;
using Assets._Scripts.EliStuff.Bugs.States;

namespace Assets._Scripts.EliStuff.Bugs.States
{
    public class BeginBugState : MonoBehaviour, IBugState
    {
        private Bug bug;
        int bugAttractorIndex;

        public BeginBugState(Bug bugRef) //constructor
        {
            //assign tthe argument delivered through bugRef to the private instance of our Bug class
            bug = bugRef;
        }

        public void Enter()
        {

            bug.speed = Random.Range(2, 10);
            //pick a random atrract point to go to *this always chooses the same point.....*
            bugAttractorIndex = Random.Range(0, bug.bugAttractors.Count);
        }

        public void BugStateUpdate()
        {
            bug.transform.LookAt(bug.bugAttractors[bugAttractorIndex]);

            //move code
            bug.transform.position += bug.transform.TransformDirection(Vector3.forward) * bug.speed * Time.deltaTime;

            //when they get close enough, swtch to wander state
            if ((bug.transform.position - bug.bugAttractors[bugAttractorIndex]).magnitude < 4)
            {
                bug.SwitchState(new WanderState(bug));
            }
        }

        public void OnTriggerEnter(Collider col)
        {
            
        }

        public void Exit()
        {
           
        }
    }
}
