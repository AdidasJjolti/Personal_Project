using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform returnPoint;      //field
    public Transform ReturnPoint        //property
    {
        get { return returnPoint; }
        //set { returnPoint = value; }
    }

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
