using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets._Scripts.EliStuff.Bugs.Interfaces;
using Assets._Scripts.EliStuff.Bugs.States;

namespace Assets._Scripts.EliStuff.Bugs.States
{
    public class GoHomeState : MonoBehaviour, IBugState
    {
        private Bug bug;
        int spawnPointIndex;
        bool bugIsDead = false;

        public GoHomeState(Bug bugRef)
        {
            bug = bugRef;
        }


        public void Enter()
        {
            bug.speed = Random.Range(2, 10);
            spawnPointIndex = Random.Range(0, bug.spawnPoints.Count);
        }

    
        public void BugStateUpdate()
        {
            bug.transform.LookAt(bug.spawnPoints[spawnPointIndex]);

            //move code
            bug.transform.position += bug.transform.TransformDirection(Vector3.forward) * bug.speed * Time.deltaTime;

            //when they get close enough, swtch to wander state
            if ((bug.transform.position - bug.spawnPoints[spawnPointIndex]).magnitude < 4 && !bugIsDead)
            {
                bug.SendDestroyMessage();
                bugIsDead = true;
            }
        }

        public void Exit()
        {

        }

        public void OnTriggerEnter(Collider col)
        {

        }

    }
}