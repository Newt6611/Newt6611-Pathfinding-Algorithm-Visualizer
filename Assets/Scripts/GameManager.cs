using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public enum AlgorithmEnum {
    BFS = 0, DFS, Dijkstra, AStar 
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Dropdown algo_dropdown;
    private List<string> algo_options = new List<string>() 
    {
        "BFS Search", // 0
        "DFS Search", // 1
        "Dijkstra's Algorithm", // 2
        "AStar Algorithm" // 3
    };

    public TMP_Dropdown maze_dropdown;
    private List<string> maze_options = new List<string>()
    {
        "Maze",
        "Basic Random Maze",
        "Basic Random Weight",
        "Simple Stair"
    };

    public Button start_btn;
    public Button setup_start_btn;
    public Button setup_end_btn;
    public Button setup_wall_btn;
    public Button setup_wei_btn;

    public TMP_Text wei_or_unwei_text;
    public TMP_Text no_path_text;



    private Character character;
    public Character m_Character { get { return character; } }

    [Header("")]
    [SerializeField] private GameObject nodePre;
    [SerializeField] private LayerMask nodeLayer;
    public LayerMask NodeLayer { get { return nodeLayer; } }

    [Header("Algorithms")]
    [SerializeField] Algorithms algo;


    private int width = 40;
    private int height = 20;
    /////////// Store Same things In Two Differents Container /////////////////
    private Node[,] maze; // 2d
    private List<Node> nodes = new List<Node>(); // one list 
    ////////////////////////////////////////////////////////////////
    

    public Node startNode;
    public Node endNode;
    [HideInInspector] public bool isFinding = false;

    private bool isClear = true;
    public bool IsClear { get { return isClear; } }

    public bool hasWeightedNode = false;

    public event Action StopAllActionEvent;


    public Dictionary<string, IGameState> stateCache;
    private IGameState currentState;
    
    public AlgorithmEnum currentAlgorithm = 0;

    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        stateCache = new Dictionary<string, IGameState>()
        {
            { "SetUpPlayer", new SetUpPlayerState(this) },
            { "SetUpEnd", new SetUpEndState(this) },
            { "SetUpWall", new SetUpWallState(this) },
            { "SetUpWeight", new SetUpWeightState(this) },
            { "Finding", new FindingState(this) },
            { "Weighted", new WeightedState(this) },
            { "UnWeighted", new UnWeightedState(this) }, 
            { "GenerateMaze", new GeneratingMazeState(this) }
        };

        currentState = stateCache["UnWeighted"];
    }

    private void Start() 
    {
        algo_dropdown.AddOptions(algo_options);
        maze_dropdown.AddOptions(maze_options);

        character = GameObject.FindGameObjectWithTag("character").GetComponent<Character>();

        maze = new Node[width, height];
        
        SpawnGrid();
        maze[5, 10].SetStart(character);
        maze[35, 10].SetEnd();

        currentState.OnEnter();

    }

    private void Update() 
    {
        currentState.OnUpdate();
    }

    private void SpawnGrid()
    {
        for(int i=0; i<40; i++)
        {
            for(int j=0; j<20; j++)
            {
                Node node = Instantiate(nodePre, new Vector3(i, 0, j), Quaternion.identity).GetComponent<Node>();
                node.transform.SetParent(this.transform);
                maze[i, j] = node;
                nodes.Add(node);

                if(node.tag == "start")
                    node.SetStart(character);
                else if(node.tag == "end")
                    node.SetEnd();
                else
                    node.SetNormal();
            }
        }
    }

    public void StartPathFinding()
    {
        if(startNode != null && endNode != null)
        {
            if(!isClear)
                ClearAllNode();
            
            isClear = false;
            
            isFinding = true;

            switch(currentAlgorithm)
            {
                case AlgorithmEnum.BFS:
                    algo.BFS(startNode, endNode);
                    break;
                case AlgorithmEnum.DFS:
                    algo.DFS(startNode, endNode);
                    break;
                case AlgorithmEnum.Dijkstra:
                    algo.Dijkstra(startNode, endNode);
                    break;
                case AlgorithmEnum.AStar:
                    algo.AStar(startNode, endNode);
                    break;
            }

            SetState(stateCache["Finding"]);
        }
    }



    public void ToUnWeightedState() 
    {
        SetState(stateCache["UnWeighted"]);
    }

    public void ToWeightedState()
    {
        SetState(stateCache["Weighted"]);
    }

    public void ToSetPlayerState()
    {
        SetState(stateCache["SetUpPlayer"]);
    }

    public void ToSetEndState()
    {
        SetState(stateCache["SetUpEnd"]);
    }

    public void ToSetWallState()
    {
        SetState(stateCache["SetUpWall"]);
    }

    public void ToSetWeightedState()
    {
        SetState(stateCache["SetUpWeight"]);
    }






    public void HandleDropDownList(int index)
    {
        switch(index)
        {
            case 0:
                currentAlgorithm = AlgorithmEnum.BFS;
                break;
            case 1:
                currentAlgorithm = AlgorithmEnum.DFS;
                break;
            case 2:
                currentAlgorithm = AlgorithmEnum.Dijkstra;
                break;
            case 3:
                currentAlgorithm = AlgorithmEnum.AStar;
                break;
        }

        if(index == 0 || index == 1)
        {
            SetState(stateCache["UnWeighted"]);
        }
        else
        {
            SetState(stateCache["Weighted"]);
        }
    }



    public void HandleMazeAlgorithms(int index)
    {
        switch(index)
        {
            case 1:
                ClearAllNode();
                SetState(stateCache["GenerateMaze"]);
                algo.BasicRandomMaze(maze);
                maze_dropdown.value = 0;
                break;
            case 2:
                ClearAllNode();
                SetState(stateCache["GenerateMaze"]);
                algo.BasicRandomWeight(maze);
                maze_dropdown.value = 0;
                if(currentAlgorithm == AlgorithmEnum.BFS || currentAlgorithm == AlgorithmEnum.DFS)
                {
                    currentAlgorithm = AlgorithmEnum.Dijkstra;
                    algo_dropdown.value = 2;
                }
                break;
            case 3:
                ClearAllNode();
                SetState(stateCache["GenerateMaze"]);
                algo.SimpleStair(maze);
                maze_dropdown.value = 0;
                break;
        }
    }




    public void ClearAllNode()
    {
        for(int i=0; i<40; i++)
        {
            for(int j=0; j<20; j++)
            {
                if(maze[i, j].tag != "start" && maze[i, j].tag != "end")
                    maze[i, j].SetNormal();
                else if(maze[i, j].tag == "start")
                    maze[i, j].SetStart(character);
                else if(maze[i, j].tag == "end")
                    maze[i, j].SetEnd();                    
            }
        }

        character.transform.position = new Vector3(startNode.transform.position.x, character.transform.position.y, startNode.transform.position.z);
        isClear = true;
    }

    public void ClearAllWeightedNode()
    {
        for(int i=0; i<40; i++)
        {
            for(int j=0; j<20; j++)
            {
                if(maze[i, j].tag != "start" && maze[i, j].tag != "end" && maze[i, j].tag == "weight")
                    maze[i, j].SetNormal();        
            }
        }
    }

    public void StopAllAction()
    {
        StopAllActionEvent?.Invoke();
    }


    public void SetState(IGameState state)
    {
        currentState.OnExit();
        this.currentState = state;
        currentState.OnEnter();
    }
}
