using UnityEngine;

public class cornGrab : MonoBehaviour
{
    public string cornID;

    public void Awake()
    {
        cornID = this.name + "-" + transform.position.ToString();
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided Object: " + other.gameObject.name);
        if(other.gameObject.tag == "Player")
        {
            cornGot();
        }
    }

    public void cornGot()
    {
        GameController.instance.cornCollect(cornID);
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<SphereCollider>().enabled = false;

    }
}
