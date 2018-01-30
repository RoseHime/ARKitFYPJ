using UnityEngine;
using UnityEngine.UI;

public class StopAllAction : MonoBehaviour
{

    private ButtonControl bc;

    // Use this for initialization
    void Start()
    {
        bc = GameObject.FindGameObjectWithTag("ControlButton").GetComponent<ButtonControl>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickCommand()
    {
        Debug.Log("Stop All Action");
        if (bc.GetListOfUnit().Count < 2)
        {
            bc.go_SelectUnit().GetComponent<PlayerFSM>().StopAllActions();
        }
        else if(bc.GetListOfUnit().Count > 1)
        {
            for (int i = 0; i < bc.GetListOfUnit().Count; i++)
            {
                bc.GetListOfUnit()[i].GetComponent<PlayerFSM>().StopAllActions();
            }
        }
    }
}