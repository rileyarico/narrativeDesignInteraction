using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class RequestBox : MonoBehaviour
{
    //given in the inspector. Managing what object we are observing/counting
    public string itemName;
    public GameObject itemToCount;

    //we find this by calling our RqManager (we locate at start)
    //when targetCount is reached, we should notify RqManager
    private int targetCount;

    //we build onto this inside this script to keep track of the count of ITEMTOCOUNT prefabs exist here
    [HideInInspector] public List<GameObject> boxList = new List<GameObject>();

    private RequestManager requestManager;
    [HideInInspector] public int thisCount;

    void Start()
    {
        //find our request manager
        requestManager = FindFirstObjectByType<RequestManager>();
        if (requestManager != null) 
        {
            Debug.Log("RqManager not found!");
        }
        else
        {
            Debug.Log("RqManager not found!");
        }
        //go thru strings and find right one, set to __Amt
    }

    private void Update()
    {
        targetCount = requestManager.GrabRequestAmt(itemName);
        if(thisCount >= targetCount)
        {
            Debug.Log("ThisCount of " + itemName + " is equal to our target count. Letting RqManager know...");
            requestManager.SetItemTrue(itemName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(itemName)) //if the tag matches itemName
        {
            //Debug.Log("Object matches search");
            //add to list
            boxList.Add(other.gameObject);
            //ArrayUtility.Add<GameObject>(ref boxElements, other.gameObject);
            thisCount++;
        }
        Debug.Log("Current List in " + itemName + ": " + boxList.Count);
        /*foreach (GameObject go in boxList)
        {
            Debug.Log(go.name);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(itemName))
        {
            //Debug.Log("Object matches search");
            //remove from list
            boxList.Remove(other.gameObject);
            //ArrayUtility.Remove<GameObject>(ref boxElements, other.gameObject);
            thisCount--;
        }
        Debug.Log("Current List in " + itemName + ": " + boxList.Count);
        /*foreach (GameObject go in boxList)
        {
            Debug.Log(go.name);
        }*/
    }

    public void DestroyRequested()
    {
        //destroy all items inside this trigger, but only the amount of the request
        for(int i = 0; /*i <= boxList.Count - 1 &&*/ i <= targetCount - 1; i++)
        {
            Destroy(boxList[0]);
            boxList.RemoveAt(0);
        }
        Debug.Log("Destroyed" + targetCount+ "from " + this.name + " box");
    }

    public void ResetRequestBoxes()
    {
        boxList = new List<GameObject>();
        thisCount = 0;
    }
}
