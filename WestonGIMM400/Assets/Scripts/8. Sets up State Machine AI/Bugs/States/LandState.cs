using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets._Scripts.EliStuff.Bugs.Interfaces;
using Assets._Scripts.EliStuff.Bugs.States;

namespace Assets._Scripts.EliStuff.Bugs.States
{
    public class LandState : MonoBehaviour, IBugState
    {
        private Bug bug;

            public LandState(Bug bugRef)
        {
            bug = bugRef;
        }


        public void Enter()
        {
            Debug.Log("We're in LandState");
            bug.wanderDelay = 2.0f;
           
        }

        public void BugStateUpdate()
        {
            bug.wanderDelay -= Time.deltaTime;
            if(bug.wanderDelay < 0)
            {
                bug.SwitchState(new WanderState(bug));

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
