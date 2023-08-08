using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string name;
    public string description;
    public string extras;
    public GameObject objectplayer;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // GetPlacePos();
        //objectplayer.transform.position = hit.point;
       // print(hit.point);
    }

    public Vector3 GetPlacePos()
    {
        return objectplayer.transform.position; 
        /*TODO add a way  to get the lowest point of the mesh
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
           // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
           

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }

        */
    }

    public void PlaceItem(RaycastHit hit)
    {
        Vector3 pos = GetPlacePos();
        transform.SetParent(null);
        transform.position = (-objectplayer.transform.position+ transform.position )+ hit.point;

        gameObject.layer = 0;
        BoxCollider c = gameObject.GetComponent<BoxCollider>();
        c.enabled = true;


    }

}
