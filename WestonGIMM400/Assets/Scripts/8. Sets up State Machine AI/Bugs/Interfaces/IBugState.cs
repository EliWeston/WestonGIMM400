using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets._Scripts.EliStuff.Bugs.Interfaces
{
    public interface IBugState
    {
        //Interface implementable methods
        //base declarations should be placed to derive out to classes inheriting from IbugState
        void Enter();
        void BugStateUpdate();
        void Exit();
        void OnTriggerEnter(Collider col);
    }
}

