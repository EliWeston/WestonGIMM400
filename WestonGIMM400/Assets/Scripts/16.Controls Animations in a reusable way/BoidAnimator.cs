using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidAnimator: MonoBehaviour
{

    //Angle of joint movement
    public float _lWingAngle;
    public float _rWingAngle;
    public float _tailAngle;
    public float _neckAngle;
    //public float _lWingMidAngle;
    // public float _rWingMidAngle;

    //overall speed of main joint animations
    public float _animSpeed;
    private float _animSpeedSave;

    //perios of movement
    private float _lWingPeriod;
    private float _rWingPeriod;
    private float _tailPeriod;
    private float _neckPeriod;

    //Speed of joint movements
    public float _lWingSpeed;
    public float _rWingSpeed;
    public float _lWingSpeedSave;
    public float _rWingSpeedSave;
    public float _tailSpeed;
    public float _neckSpeed;

    //Transforms for wing joints
    public Transform rWingBaseJoint;
    public Transform lWingBaseJoint;
    public Transform tailBaseJoint;
    public Transform neckBaseJoint;
    public Transform rWingMid;
    public Transform lWingMid;

    private float _Time;

    //to and from object to set rotation of mid wing joint
    public Transform rLow;
    public Transform rHigh;
    public Transform rNormal;

    public Transform lLow;
    public Transform lHigh;
    public Transform lNormal;

    //Right wing
    private GameObject nTurn;
    private GameObject rTurn;
    private GameObject lTurn;

    //get boid controller script
    private BoidController boidControllerScript;

    public GameObject _playerBody;

    //for flapping at different times
    private bool _flapping = true;
    private bool _glidding = false;
    private int flapCounter = 0;
    private int glideCounter = 0;

    //right now midjoint freak out while bird is turning
    private bool isTurning = false;

    


    private void Start()
    {
        

        rTurn = new GameObject();
        nTurn = new GameObject();
        lTurn = new GameObject();


        rTurn.transform.rotation = Quaternion.Euler(-50f, -88.0f, 0);
        nTurn.transform.rotation = Quaternion.Euler(0, -88.0f, 0);
        lTurn.transform.rotation = Quaternion.Euler(50.0f, -88.0f, 0);

        boidControllerScript = this.GetComponent<BoidController>();

        _rWingSpeedSave = _rWingSpeed;
        _lWingSpeedSave = _lWingSpeed;
        _animSpeedSave = _animSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SetPeriods();
        CheckKeyChanges();
        BaseJointMovement();
        MidJointMovement();
        TurnController();


    }

    void SetPeriods()
    {
        _lWingPeriod = _lWingSpeed / _animSpeed;
        _rWingPeriod = _rWingSpeed / _animSpeed;
        _tailPeriod = _tailSpeed / _animSpeed;
        _neckPeriod = _neckSpeed / _animSpeed;
    }

    void CheckKeyChanges()
    {
        if (boidControllerScript.increaseSpeed == true)
        {
            //_glidding = false;
            _animSpeed = _animSpeedSave * 2;
            _flapping = true;
            //Debug.Log("Should be increasing anim speed");
           //Debug.Log(_animSpeed);
        } else
        {
            _animSpeed = _animSpeedSave;
        }
    }
    void BaseJointMovement()
    {
        _Time = _Time + Time.deltaTime;
        float rWPhase = Mathf.Sin(_Time / _rWingPeriod);
        float lWPhase = Mathf.Sin(_Time / _lWingPeriod);
        float tailPhase = Mathf.Sin(_Time / _tailPeriod);
        float neckPhase = Mathf.Sin(_Time / _neckPeriod);

        //Main Joint movement
        tailBaseJoint.localRotation = Quaternion.Euler(new Vector3(0, 0, tailPhase * _tailAngle));
        neckBaseJoint.localRotation = Quaternion.Euler(new Vector3(0, 0, neckPhase * _neckAngle));

        

        if (flapCounter > 300 && boidControllerScript.increaseSpeed == false)
        {
            glideCounter = 0;
            _flapping = false;
            _glidding = true;
            flapCounter = 0;
        }
        else if (glideCounter > 100)
        {
            flapCounter = 0;
            glideCounter = 0;
            _flapping = true;
            _glidding = false;
            rWingBaseJoint.localRotation = Quaternion.RotateTowards(rWingBaseJoint.transform.localRotation, rHigh.localRotation, Time.deltaTime * 80.0f);
            lWingBaseJoint.localRotation = Quaternion.RotateTowards(lWingBaseJoint.transform.localRotation, lHigh.localRotation, Time.deltaTime * 80.0f);
        }

        if (_flapping == true)
        {
            //Debug.Log("Flapping");
            rWingBaseJoint.localRotation = Quaternion.Euler(new Vector3(rWPhase * _rWingAngle, 0, 0));
            lWingBaseJoint.localRotation = Quaternion.Euler(new Vector3(lWPhase * -_lWingAngle, 0, 0));
            flapCounter++;
            //Debug.Log(flapCounter);

        } if (_glidding == true && boidControllerScript.increaseSpeed == false)
        {
            //Debug.Log("Gliding");
            glideCounter++;
            rWingBaseJoint.localRotation = Quaternion.Euler(new Vector3(rWPhase * _rWingAngle/6, 0, 0));
            lWingBaseJoint.localRotation = Quaternion.Euler(new Vector3(lWPhase * -_lWingAngle/6, 0, 0));
            //Debug.Log(glideCounter);
        }
       
    }

    void MidJointMovement()
    {
        if (isTurning == false)
        {
            //right wing mid joint movements
            if (rWingBaseJoint.localRotation.x <= -.30)
            {
                rWingMid.transform.localRotation = Quaternion.RotateTowards(rWingMid.transform.localRotation, rLow.localRotation, Time.deltaTime * 100.0f);
            }
            else if (rWingBaseJoint.localRotation.x >= .30)
            {
                rWingMid.transform.localRotation = Quaternion.RotateTowards(rWingMid.transform.localRotation, rHigh.localRotation, Time.deltaTime * 100.0f);
            }
            else
            {
                rWingMid.transform.localRotation = Quaternion.RotateTowards(rWingMid.transform.localRotation, rNormal.localRotation, Time.deltaTime * 100.0f);
            }
            //left wing mid joint movements
            if (lWingBaseJoint.localRotation.x <= -.30)
            {
                lWingMid.transform.localRotation = Quaternion.RotateTowards(lWingMid.transform.localRotation, lLow.localRotation, Time.deltaTime * 100.0f);
            }
            else if (rWingBaseJoint.localRotation.x >= .30)
            {
                lWingMid.transform.localRotation = Quaternion.RotateTowards(lWingMid.transform.localRotation, lHigh.localRotation, Time.deltaTime * 100.0f);
            }
            else
            {
                lWingMid.transform.localRotation = Quaternion.RotateTowards(lWingMid.transform.localRotation, lNormal.localRotation, Time.deltaTime * 100.0f);
            }
        }
    }

    void TurnController()
    {
        {
            if (boidControllerScript.positionChange >= .7f)
            {
                //Debug.Log("should be turning right");
                _playerBody.transform.localRotation = Quaternion.RotateTowards(_playerBody.transform.localRotation, rTurn.transform.rotation, Time.deltaTime * 200.0f);
                _lWingSpeed = .5f;
                _rWingSpeed = 1;
                isTurning = true;
            }

            else if (boidControllerScript.positionChange <= -.7f)
            {
                //Debug.Log("should be turning left");
                _playerBody.transform.localRotation = Quaternion.RotateTowards(_playerBody.transform.localRotation, lTurn.transform.rotation, Time.deltaTime * 200.0f);
                _lWingSpeed = 1;
                _rWingSpeed = .5f;
                isTurning = true;
            }
            else if (boidControllerScript.positionChange > -.7f && boidControllerScript.positionChange < .7f )
            {
                _playerBody.transform.localRotation = Quaternion.RotateTowards(_playerBody.transform.localRotation, nTurn.transform.rotation, Time.deltaTime * 100.0f);
                _lWingSpeed = _lWingSpeedSave;
                _rWingSpeed = _rWingSpeedSave;
                isTurning = false;
            }
        }
    }
}
