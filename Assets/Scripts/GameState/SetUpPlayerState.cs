using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetUpPlayerState : MonoBehaviour, IGameState
{
    GameManager manager;

    public SetUpPlayerState(GameManager manager)
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
                if(Input.GetMouseButtonDown(0) && hit.transform.tag != "end" && hit.transform.tag != "start" && hit.transform.tag != "character" && hit.transform.tag != "weight")
                {
                    if(EventSystem.current.IsPointerOverGameObject())
                        return; // For UI

                    if(hit.transform.tag == "wall")
                        hit.transform.GetComponent<Node>().SetNormal();
                    manager.startNode.SetNormal();
                    hit.transform.GetComponent<Node>().SetStart(manager.m_Character);
                }
            }
        }
    }

    public void OnExit()
    {

    }
}
