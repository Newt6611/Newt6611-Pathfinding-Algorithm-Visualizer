using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetUpWallState : MonoBehaviour, IGameState
{
    GameManager manager;

    public SetUpWallState(GameManager manager)
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
                if(Input.GetMouseButton(0) && hit.transform.tag != "end" && hit.transform.tag != "start" && hit.transform.tag != "character" && hit.transform.tag != "weight")
                {
                    if(EventSystem.current.IsPointerOverGameObject())
                        return; // For UI

                    if(hit.transform.tag == "wall")
                        return;
                    
                    hit.transform.GetComponent<Node>().SetWall();
                }

                if(Input.GetMouseButton(1) && hit.transform.tag == "wall")
                    hit.transform.GetComponent<Node>().SetNormal();
            }
        }
    }

    public void OnExit()
    {

    }
}
