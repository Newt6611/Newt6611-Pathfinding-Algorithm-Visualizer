using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetUpEndState : MonoBehaviour, IGameState
{
    private GameManager manager;

    public SetUpEndState(GameManager manager)
    {
        this.manager = manager;
    }

    public void OnEnter() 
    {

    }

    public void OnUpdate() 
    {
        if(!manager.IsClear)
            manager.ClearAllNode();

        if(manager.IsClear)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, manager.NodeLayer))
            {
                if(Input.GetMouseButtonDown(0) && hit.transform.tag != "end" && hit.transform.tag != "start" && hit.transform.tag != "weight")
                {
                    
                    if(EventSystem.current.IsPointerOverGameObject())
                        return; // For UI


                    manager.endNode.SetNormal();
                    hit.transform.GetComponent<Node>().SetEnd();
                }
            }
        }
    }
    
    public void OnExit() 
    {

    }
}
