using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratingMazeState : MonoBehaviour, IGameState
{


    //////////////////////////////////
    // this state basicly just limted user to do anythig

    private GameManager manager;

    public GeneratingMazeState(GameManager manager)
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
        manager.wei_or_unwei_text.gameObject.SetActive(false);
        manager.setup_wei_btn.gameObject.SetActive(false);
    }

    public void OnUpdate() { }

    public void OnExit() { }
}
