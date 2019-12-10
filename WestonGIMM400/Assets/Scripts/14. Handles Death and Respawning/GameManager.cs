using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int _ember = 0;
    private int _boidPU = 0;

    private void Start()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Init();
    }

    private void Init()
    {
        Instance = this;
        _ember = 0;
    }

    public void ResetCollection()
    {
        _boidPU = 0;
        _ember = 0;
        GameUIManager.Instance.SetBoidText(_boidPU);
        GameUIManager.Instance.SetEmberText(_ember);
    }

    // ------ Ember Stuff ------ //

    //Called from outside function for when the player collects an Ember.
    public void CollectEmber()
    {
        _ember++;
        GameUIManager.Instance.SetEmberText(_ember);
    }

    //return the number of embers player has collected
    public int GetEmber()
    {
        return _ember;
    }

    // ------ BoidStuff ------ //

    //Called from outside function for when  the player collects a boidPU.
    public void CollectBoid()
    {
        _boidPU++;
        GameUIManager.Instance.SetBoidText(_boidPU);
    }

    public void RemoveBoid()
    {
        _boidPU--;
        GameUIManager.Instance.SetBoidText(_boidPU);
    }

    //return the number of boids player has collected
    public int GetBoid()
    {
        return _boidPU;
    }
}
