using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnWeightedState : MonoBehaviour, IGameState
{
    private GameManager manager;

    public UnWeightedState(GameManager manager)
    {
        this.manager = manager;
    }

    public void OnEnter() 
    {
        if(manager.hasWeightedNode)
        {
            manager.ClearAllWeightedNode();
            manager.hasWeightedNode = false;
        }
        manager.no_path_text.gameObject.SetActive(false);
        manager.maze_dropdown.gameObject.SetActive(true);
        manager.setup_wall_btn.gameObject.SetActive(true);
        manager.start_btn.gameObject.SetActive(true);
        manager.algo_dropdown.gameObject.SetActive(true);
        manager.setup_start_btn.gameObject.SetActive(true);
        manager.setup_end_btn.gameObject.SetActive(true);
        manager.wei_or_unwei_text.gameObject.SetActive(true);
        manager.setup_wei_btn.gameObject.SetActive(false);

        string algothrim_name = manager.algo_dropdown.options[manager.algo_dropdown.value].text;
        manager.wei_or_unwei_text.text = algothrim_name + " Is UnWeighted";

        manager.SetState(manager.stateCache["SetUpPlayer"]);
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
