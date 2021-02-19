using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedState : MonoBehaviour, IGameState
{
    private GameManager manager;

    public WeightedState(GameManager manager)
    {
        this.manager = manager;
    }

    public void OnEnter()
    {
        manager.no_path_text.gameObject.SetActive(false);
        manager.hasWeightedNode = true;
        manager.maze_dropdown.gameObject.SetActive(true);
        manager.setup_wall_btn.gameObject.SetActive(true);
        manager.start_btn.gameObject.SetActive(true);
        manager.algo_dropdown.gameObject.SetActive(true);
        manager.setup_start_btn.gameObject.SetActive(true);
        manager.setup_end_btn.gameObject.SetActive(true);
        manager.wei_or_unwei_text.gameObject.SetActive(true);
        manager.setup_wei_btn.gameObject.SetActive(true);

        string algothrim_name = manager.algo_dropdown.options[manager.algo_dropdown.value].text;
        manager.wei_or_unwei_text.text = algothrim_name + " Is Weighted";

        manager.SetState(manager.stateCache["SetUpPlayer"]);
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
