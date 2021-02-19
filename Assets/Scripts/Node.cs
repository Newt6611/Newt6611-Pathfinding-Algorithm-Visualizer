using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Node : MonoBehaviour
{
    [SerializeField] private GameObject[] buildings;
    [SerializeField] private GameObject ground;

    [SerializeField] private GameObject weight;

    [SerializeField] private LayerMask nodeLayer;
    [SerializeField] private Material node_m;
    [SerializeField] private Material node_red;
    [SerializeField] private Material node_blue;

    private List<Node> adjacency = new List<Node>();


    // weighted
    // AStar /////////////////
    private float g = 1;
    public float G { get { return g; } set { g = value; } }

    private float h;
    public float H { 
        get { 
            if(this.tag == "end")
                h = 0;
            else
                h = (Vector3.Distance(transform.position, GameManager.Instance.endNode.transform.position));

            return h;
        }
    }

    private float f;
    public float F { get { return G + H; }}
    ///////////////////////////

    ////// Dijastra //////////////
    private float d_cost = 1;
    public float D { get { return d_cost; } set { d_cost = value; } }



    private float shortest_path;
    public float Shortest_Path { get { return shortest_path; } set { shortest_path = value; } }



    ///////////////////////////////
    private Node cameFrom;
    public Node CameFrom { get { return cameFrom; } set { cameFrom = value; } }


    private MeshRenderer mesh;

    private void Awake() 
    {
        for(int i=0; i<buildings.Length; i++)
            buildings[i].SetActive(false);

        ground.SetActive(false);

        mesh = GetComponent<MeshRenderer>();
    }

    private void Start() 
    {
        Shortest_Path = Mathf.Infinity;
    }




    public void SetStart(Character c)
    {
        // Dijkstra 
        Shortest_Path = Mathf.Infinity;
        D = 1f;

        // AStar
        G = 1f;

        cameFrom = null;

        transform.tag = "start";
        mesh.material.color = Color.yellow;
        c.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        GameManager.Instance.startNode = this;
    }

    public void SetEnd()
    {   
        // Dijkstra 
        Shortest_Path = Mathf.Infinity;
        D = 1f;

        // AStar
        G = 1f;
        
        cameFrom = null;

        transform.tag = "end";
        mesh.material.color = Color.red;
        GameManager.Instance.endNode = this;
    }

    public void SetNormal()
    {
        for(int i=0; i<buildings.Length; i++)
            buildings[i].SetActive(false);

        ground.SetActive(false);
        weight.SetActive(false);

        // Dijkstra 
        Shortest_Path = Mathf.Infinity;
        D = 1f;

        // AStar
        G = 1f;

        cameFrom = null;

        transform.tag = "cube";
        mesh.material = node_m;
    }

    public void SetWall()
    {
        if(transform.CompareTag("wall"))
            return;

        this.SetNormal();

        transform.tag = "wall";

        int idx = Random.Range(0, 3);
        buildings[idx].SetActive(true);
        buildings[idx].transform.DOComplete();
        buildings[idx].transform.DOShakeScale(.5f, .2f, 10, 90, true);
        buildings[idx].transform.DOLocalMoveY(transform.position.y + 0.5f, 0.3f, false);
    }

    public void SetPath()
    {
        ground.SetActive(true);
        ground.transform.DOComplete();
        ground.transform.DOShakeScale(.5f, .2f, 10, 90, true);
        ground.transform.DOLocalMoveY(transform.position.y + 0.5f, 0.3f, false);
    }

    public void SetWeight()
    {
        // Dijkstra
        D = 50f;

        // AStar
        G = 50f;

        weight.gameObject.SetActive(true);
        weight.transform.DOComplete();
        weight.transform.DOShakeScale(.5f, .2f, 10, 90, true);
        weight.transform.DOLocalMoveY(transform.position.y + 0.5f, 0.3f, false);

        transform.tag = "weight";
    }




    public void SetBlue()
    {
        mesh.material = node_blue;
    }

    public void SetRed()
    {
        mesh.material = node_red;
    }




    public List<Node> GetAdjacencyCube()
    {
        adjacency.Clear();
        GetDirAdjacency(Vector3.left);
        GetDirAdjacency(Vector3.right);
        GetDirAdjacency(Vector3.back);
        GetDirAdjacency(Vector3.forward);

        return adjacency;
    }

    private void GetDirAdjacency(Vector3 dir)
    {
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, nodeLayer))
        {
            Node n = hit.transform.GetComponent<Node>();
            if(n.CompareTag("wall") || adjacency.Contains(n))
                return;
            adjacency.Add(n);
        }
    }



    // For Generate Maze
    public List<Node> GetAllAdjacencyCube()
    {
        adjacency.Clear();
        GetAllDirAdjacency(Vector3.forward);
        GetAllDirAdjacency(Vector3.left);
        GetAllDirAdjacency(Vector3.back);
        GetAllDirAdjacency(Vector3.right);

        return adjacency;
    }

    private void GetAllDirAdjacency(Vector3 dir)
    {
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, nodeLayer))
        {
            Node n = hit.transform.GetComponent<Node>();
            adjacency.Add(n);
        }
    }
}
