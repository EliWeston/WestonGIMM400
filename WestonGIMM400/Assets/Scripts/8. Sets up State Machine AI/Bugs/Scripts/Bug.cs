using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//inherit State classes
using Assets._Scripts.EliStuff.Bugs.States;
//inherit interface class
using Assets._Scripts.EliStuff.Bugs.Interfaces;


public class Bug : MonoBehaviour
{
    private IBugState iActiveBugState;
    private GoHomeState goHomeState;

    private static Bug instanceRef;

    public Vector3 _wayPoint;
    int _randomHeight;
    public List<Vector3> bugAttractors;
    public List<Vector3> spawnPoints;
    BugManager bugMan;
    public GameObject bugManager;
    public Vector3 vectorAdjuster;

    //Vars for setting different color materials
    public Material yellowSkin;
    public Material whiteSkin;
    public Material purpSkin;

    //string for other objects to check color
    public string colorString;

    //variable to get light
    public Light bugLight;

    //Delays that enable states to not immediately trigger back into previous state
    public float landDelay;
    public float wanderDelay;

    //bool so that goHome stuff isn't triggered a bunch
    bool goingHome;

    //get the index number of the bug for proper removal
    public int bugIndex;


    [SerializeField]
    int _range = 4;

    int _speed;
    bool _nearPlant;

    public bool nearPlant
    {
        get { return _nearPlant; }
        set { _nearPlant = value; }
    }

    public int speed
    {
        get { return _speed; }
        set { _speed = value; }
    }
    public int range
    {
        get { return _range; }
        set { _range = value; }
    }
    public int randomHeight
    {
        get { return _randomHeight; }
        set { _randomHeight = value; }
    }


    void Start()
    {

        goingHome = false;

        //gets our first state as begin state
        iActiveBugState = new BeginBugState(this);

        //so we can change it from WanderState
        bugLight = this.GetComponent<Light>();


        if (iActiveBugState != null)
        {
            iActiveBugState.Enter();
        }

        //get bug manager primarily to refrence bug attract points
        bugManager = GameObject.FindGameObjectWithTag("BugManager");
        bugMan = bugManager.GetComponent<BugManager>();
        //copying our vector3 arrays from bug manager, wish I knew how to do this earlier
        bugAttractors.AddRange(bugMan.bugAttractors);
        spawnPoints.AddRange(bugMan.spawnPointLocations);

    }

    // Update is called once per frame
    void Update()   
    {
        //sets a random height value
        _randomHeight = Random.Range(-1, 1);

        //triggers the update function for all states
        if (iActiveBugState != null)
        {
            iActiveBugState.BugStateUpdate();
        }

        //for making the bugs go away during the day
        if (bugMan.goHomeTime == true && goingHome == false)
        {
            goingHome = true;
            GoHome();
        } 
    }

    void GoHome()
    {
        SwitchState(new GoHomeState(this));
    }

    public void SendDestroyMessage()
    {
        bugMan.DestroyBug(this.gameObject);
    }

    private void OnTriggerEnter(Collider col)
    {
        iActiveBugState.OnTriggerEnter(col);
    }

    public void SwitchState(IBugState newState)
    {
        iActiveBugState.Exit();
        iActiveBugState = newState;
        iActiveBugState.Enter();
    }
}
