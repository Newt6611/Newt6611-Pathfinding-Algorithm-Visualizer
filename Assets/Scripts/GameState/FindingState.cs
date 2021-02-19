using UnityEngine;
using System;

public class FindingState : MonoBehaviour, IGameState
{
    private GameManager manager;


    public FindingState(GameManager manager)
    {
        this.manager = manager;
    }

    public void OnEnter() 
    {
        manager.maze_dropdown.gameObject.SetActive(false);
        manager.setup_wall_btn.gameObject.SetActive(false);
        manager.start_btn.gameObject.SetActive(false);
        manager.algo_dropdown.gameObject.SetActive(false);
        manager.setup_start_btn.gameObject.SetActive(false);
        manager.setup_end_btn.gameObject.SetActive(false);
        manager.setup_wei_btn.gameObject.SetActive(false);
        manager.wei_or_unwei_text.gameObject.SetActive(true);

        manager.isFinding = true;
    }

    public void OnUpdate() 
    {
        if(!manager.isFinding && Input.anyKeyDown)
        {
            if(manager.currentAlgorithm == AlgorithmEnum.BFS || manager.currentAlgorithm == AlgorithmEnum.DFS)
                manager.SetState(manager.stateCache["UnWeighted"]);
            else
                manager.SetState(manager.stateCache["Weighted"]);
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            manager.StopAllAction();
            if(manager.currentAlgorithm == AlgorithmEnum.BFS || manager.currentAlgorithm == AlgorithmEnum.DFS)
                manager.SetState(manager.stateCache["UnWeighted"]);
            else
                manager.SetState(manager.stateCache["Weighted"]);
        }
    }

    public void OnExit() 
    {
        manager.ClearAllNode();
    }
}
