using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    //weight settings
    //how much total weight is needed to activate the plate
    public float weightThreshold = 5f;

    //if true, the plate stays activated, even after the object is removed (optional remove)
    public bool lockOnActivate = false;

    //event
    //fired when total weight exceeds the threshold
    //different from event action which we were previously using.
    //Unity event needs to be wired in the inspector, and is more like buttons
    //Static event action is just code, doesn't need ref to sender bc static, not as designer friendly
    public UnityEvent onActivated;

    //fired when weight drops below the threshold (ignored if lockedOnActivate is true)
    public UnityEvent onDeactivated;

    //visual feedback
    //optional, the plate mesh moves down when pressed
    public Transform plate;

    //how far the plate depresses when activated (world units)
    public float pressDepth = 0.05f;

    float currentWeight = 0f;
    bool isActivated = false;
    bool isLocked = false;
    Vector3 plateResetPos;
    Vector3 platePressedPause;

    HashSet<PhysicsObject> objectsOnPlate = new HashSet<PhysicsObject>(); //similar to an array, less code
    HashSet<PhysicsObject> countedObjects = new HashSet<PhysicsObject>();

    void Start()
    {
        if(plate != null)
        {
            //storing where our plate is
            plateResetPos = plate.localPosition;
            //taking that and moving it down
            platePressedPause = plateResetPos + Vector3.down * pressDepth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //called whenever weight changes, acivate if threshold is met
    void CheckActivation()
    {
        if(!isActivated && currentWeight >= weightThreshold)
        {
            isActivated = true;
            if(lockOnActivate) isLocked = true;

            //calls it for whatever is listening to it
            onActivated.Invoke();
            Debug.Log("Pressure plate is activated");

            if (plate != null)
            {
                //after its activated, move the plate
                plate.localPosition = platePressedPause;
            }

        }
    }

    //fires when any collider enters the trigger zone
    //we check for physicsObject to get the weight
    private void OnTriggerEnter(Collider other)
    {
        PhysicsObject physObj = other.GetComponent<PhysicsObject>();
        if (physObj == null) return;

        //if (physObj.isHeld) return; //so it doesn't go off when you're holding it in the trigger area, must be dropped

        //first simple version:
        /*currentWeight += physObj.puzzleWeight;
        Debug.Log($"{other.gameObject.name} entered plate. Total weight: {currentWeight}");
        CheckActivation();*/

        //this is instead adding it to a list at first just to make sure nothing gets double activated
        /*if (objectsOnPlate.Add(physObj))
        {
            currentWeight += physObj.puzzleWeight; 
            CheckActivation();
        }*/

        objectsOnPlate.Add(physObj);
    }

    private void OnTriggerStay(Collider other)
    {
        PhysicsObject physicsObject = other.GetComponent<PhysicsObject>();
        if(physicsObject == null) return;

        if(!physicsObject.isHeld && countedObjects.Add(physicsObject))
        {
            currentWeight += physicsObject.GetWeight();
            CheckActivation();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(isLocked) return;
        PhysicsObject physicsObject = other.GetComponent<PhysicsObject>();
        if (physicsObject == null) return;

        if(objectsOnPlate.Remove(physicsObject))
        {
            currentWeight -= physicsObject.puzzleWeight;
            currentWeight = Mathf.Max(0f, currentWeight);
            CheckDeactivation();
        }

        objectsOnPlate.Remove(physicsObject);
    }

    //call this when weight is removed. Deactivates if below threshold
    void CheckDeactivation()
    {
        if(isActivated && !isLocked && currentWeight < weightThreshold)
        {
            isActivated = false;
            onDeactivated.Invoke();
            Debug.Log("Pressure plate is deactivated");

            if(plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }
    
}
