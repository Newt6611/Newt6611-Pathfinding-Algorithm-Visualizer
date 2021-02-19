using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Algorithms : MonoBehaviour
{
    [SerializeField] private Character character;

    private void Start() 
    {
        GameManager.Instance.StopAllActionEvent += StopAllAction;
    }

    private void StopAllAction() 
    {
        StopAllCoroutines();
    }

    ////////////////////////////////////////////
    //////////////////// UnWeighted ///////////
    //////////////////////////////////////////

    public void BFS(Node start, Node end)
    {   
        StartCoroutine(BFSAnimation(start, end));
    }

    private IEnumerator BFSAnimation(Node start, Node end)
    {
        yield return new WaitForSeconds(0.03f);
        Queue<Node> queue = new Queue<Node>();
        List<Node> visited = new List<Node>();

        List<Node> path = new List<Node>();

        bool isFind = false;

        queue.Enqueue(start);
        start.CameFrom = null;
        
        while(queue.Count > 0)
        {
            Node current = queue.Dequeue();

            if(!visited.Contains(current))
                visited.Add(current);

            if(current == end)
            {
                path = GetShortestPath(current);
                isFind = true;
                break;
            }

            yield return new WaitForSeconds(0.005f);
            
            if(current.tag != "end" && current.tag != "start")
                current.SetRed();

            foreach(Node n in current.GetAdjacencyCube())
            {
                if(!visited.Contains(n))
                {
                    if(n.tag != "end" && n.tag != "start")
                        n.SetBlue();

                    n.CameFrom = current;

                    queue.Enqueue(n);
                    visited.Add(n);
                }
            }
        }

        if(isFind)
        {
            foreach(Node n in path)
            {
                yield return new WaitForSeconds(0.003f);
                if(n.tag != "start" && n.tag != "end")
                    n.SetPath();
            }
            
            yield return new WaitForSeconds(0.03f);
            
            character.Run(path);
        }
        else
            HandleNoPathSituation();
    }

    public void DFS(Node start, Node end)
    {   
        StartCoroutine(DFSAnimation(start, end));
    }

    private IEnumerator DFSAnimation(Node start, Node end)
    {
        yield return new WaitForSeconds(0.03f);
        Stack<Node> stack = new Stack<Node>();
        List<Node> visited = new List<Node>();

        List<Node> path = new List<Node>();

        bool isFind = false;

        stack.Push(start);

        while(stack.Count > 0)
        {
            Node current = stack.Pop();

            if(!visited.Contains(current))
                visited.Add(current);

            if(current == end)
            {
                path = GetShortestPath(current);
                isFind = true;
                break;
            }

            if(current.tag != "end" && current.tag != "start")
                current.SetBlue();

            foreach(Node n in current.GetAdjacencyCube())
            {
                if(!visited.Contains(n))
                {
                    yield return new WaitForSeconds(0.005f);
                    stack.Push(n);
                    n.CameFrom = current;
                }
            }
        }

        if(isFind)
        {
            foreach(Node n in path)
            {
                yield return new WaitForSeconds(0.003f);
                if(n.tag != "start" && n.tag != "end")
                    n.SetPath();
            }
            
            yield return new WaitForSeconds(0.03f);
            
            character.Run(path);
        }
        else
            HandleNoPathSituation();
    }




    ////////////////////////////////////////////
    //////////////////// Weighted /////////////
    //////////////////////////////////////////

    public void AStar(Node start, Node end) 
    {
        StartCoroutine(AStarAnimation(start, end));
    }

    private IEnumerator AStarAnimation(Node start, Node end) 
    {
        yield return new WaitForSeconds(0.03f);

        List<Node> open = new List<Node>();
        List<Node> close = new List<Node>();
        
        List<Node> path = new List<Node>();
        bool isFind = false;

        start.G = 0;
        start.Shortest_Path = 0;

        open.Add(start);

        while(open.Count > 0)
        {
            Node current = GetLowerFCostNode(open);

            if(current == end)
            {
                isFind = true;
                path = GetShortestPath(current);
                break;
            }

            open.Remove(current);
            close.Add(current);

            foreach(Node n in current.GetAdjacencyCube())
            {
                if(close.Contains(n))
                    continue;
                
                float tentativeG = current.G + Vector3.Distance(current.transform.position, n.transform.position);
                if(tentativeG < n.Shortest_Path)
                {
                    n.G = tentativeG;
                    n.Shortest_Path = tentativeG;

                    n.CameFrom = current;
                    if(!open.Contains(n))
                        open.Add(n);
                }
                
                yield return new WaitForSeconds(0.005f);
                
                if(current.tag != "start" && current.tag != "end")
                    current.SetRed();
                
                if(n.tag != "start" && n.tag != "end" && n.tag != "weight")
                    n.SetBlue();
                
                if(n.tag == "weight")
                {
                    n.transform.DOComplete();
                    n.transform.DOShakeScale(.5f, .2f, 10, 90, true);
                }
            }            
        }



        if(isFind)
        {
            foreach(Node p in path)
            {
                yield return new WaitForSeconds(0.003f);
                if(p.tag != "start" && p.tag != "end")
                    p.SetPath();
            }

            character.Run(path);
        }
        else
            HandleNoPathSituation();
    }

    // AStar Tool
    private Node GetLowerFCostNode(List<Node> nodes)
    {
        Node current = nodes[0];
        for(int i=1; i<nodes.Count; i++)
        {
            if(nodes[i].F < current.F)
                current = nodes[i];
        }

        return current;
    }

    /////////////////////////////////////////////////

    public void Dijkstra(Node start, Node end)
    {
        StartCoroutine(DijkstraAnimation(start, end));
    }

    private IEnumerator DijkstraAnimation(Node start, Node end)
    {
        yield return new WaitForSeconds(0.03f);

        List<Node> visited = new List<Node>();
        
        List<Node> priorty = new List<Node>();
        
        List<Node> path = new List<Node>();
        bool isFind = false;

        start.D = 0;
        start.Shortest_Path = 0;

        priorty.Add(start);

        while(priorty.Count > 0)
        {
            // Get Smallest Shortest_Path Value
            Node currentNode = priorty.OrderBy(n => n.Shortest_Path).FirstOrDefault();
            priorty.Remove(currentNode);

            if(currentNode == end)
            {
                isFind = true;
                path = GetShortestPath(currentNode);
                break;
            }

            if(currentNode.tag != "start" && currentNode.tag != "end")
                currentNode.SetBlue();
            if(currentNode.tag == "weight")
            {
                currentNode.transform.DOComplete();
                currentNode.transform.DOShakeScale(0.9f, 0.8f, 10, 90, true);
            }


            foreach(Node n in currentNode.GetAdjacencyCube())
            {
                if(currentNode.Shortest_Path + n.D  < n.Shortest_Path && !visited.Contains(n))
                {
                    yield return new WaitForSeconds(0.005f);
                    
                    if(n.tag != "start" && n.tag != "end")
                        n.SetRed();

                    n.Shortest_Path = currentNode.Shortest_Path + n.D;
                    n.CameFrom = currentNode;

                    priorty.Add(n);
                }

                visited.Add(currentNode);
            }
        }

        if(isFind)
        {
            foreach(Node p in path)
            {
                yield return new WaitForSeconds(0.003f);
                if(p.tag != "start" && p.tag != "end")
                    p.SetPath();
            }

            character.Run(path);
        }
        else
            HandleNoPathSituation();
    }


    /// TOOlS
    private List<Node> GetShortestPath(Node node)
    {
        Node current = node;
        List<Node> o = new List<Node>();
        
        while(current.CameFrom != null)
        {
            o.Add(current);
            current = current.CameFrom;
        }

        return o;
    }

    private void HandleNoPathSituation()
    {
        GameManager.Instance.no_path_text.gameObject.SetActive(true);

        GameManager.Instance.no_path_text.transform.DOComplete();
        GameManager.Instance.no_path_text.transform.DOShakeScale(0.9f, 0.8f, 10, 90, true);

        GameManager.Instance.isFinding = false;
    }

    //////////////////////////////////////////////////////////
    //////////////////////// Maze ///////////////////////////
    /////////////////////////////////////////////////////////
    public void BasicRandomMaze(Node[,] nodes)
    {
        StartCoroutine(BasicRandomMazeAnimation(nodes));
    }

    private IEnumerator BasicRandomMazeAnimation(Node[,] nodes)
    {
        yield return new WaitForSeconds(0.03f);
        for(int i=0; i<nodes.GetLength(0); i++)
        {
            for(int j=0; j<nodes.GetLength(1); j++)
            {
                int wall = Random.Range(0, 4);
                if(wall == 0 && nodes[i, j].tag != "end" && nodes[i, j].tag != "start" && nodes[i, j].tag != "weight" && nodes[i, j].tag != "wall")
                {
                    yield return new WaitForSeconds(0.002f);
                    nodes[i, j].SetWall();
                }
            }
        }

        yield return new WaitForSeconds(0.03f);
        if(GameManager.Instance.currentAlgorithm == AlgorithmEnum.BFS || GameManager.Instance.currentAlgorithm == AlgorithmEnum.DFS)
            GameManager.Instance.SetState(GameManager.Instance.stateCache["UnWeighted"]);
        else
            GameManager.Instance.SetState(GameManager.Instance.stateCache["Weighted"]);
    }

    public void BasicRandomWeight(Node[,] nodes)
    {
        StartCoroutine(BasicRandomWeightAnimation(nodes));
    }

    private IEnumerator BasicRandomWeightAnimation(Node[,] nodes)
    {
        yield return new WaitForSeconds(0.03f);
        for(int i=0; i<nodes.GetLength(0); i++)
        {
            for(int j=0; j<nodes.GetLength(1); j++)
            {
                int wall = Random.Range(0, 2);
                if(wall == 0 && nodes[i, j].tag != "end" && nodes[i, j].tag != "start" && nodes[i, j].tag != "weight" && nodes[i, j].tag != "wall")
                {
                    yield return new WaitForSeconds(0.002f);
                    nodes[i, j].SetWeight();
                }
            }
        }

        yield return new WaitForSeconds(0.03f);
        if(GameManager.Instance.currentAlgorithm == AlgorithmEnum.BFS || GameManager.Instance.currentAlgorithm == AlgorithmEnum.DFS)
            GameManager.Instance.SetState(GameManager.Instance.stateCache["UnWeighted"]);
        else
            GameManager.Instance.SetState(GameManager.Instance.stateCache["Weighted"]);
    }

    public void SimpleStair(Node[,] maze)
    {
        StartCoroutine(SimpleStairAnimation(maze));
    }

    private IEnumerator SimpleStairAnimation(Node[,] maze)
    {
        yield return new WaitForSeconds(0.03f);

        int pathIndex = Random.Range(0, 20);
        
        for(int i=0; i<20; i++)
        {
            for(int j=0; j<20; j++)
            {
                if(i == j && i != pathIndex && j != pathIndex && maze[i, j].tag != "start" && maze[i, j].tag != "end")
                {
                    yield return new WaitForSeconds(0.01f);
                    maze[i, j].SetWall();
                }
            }
        }

        pathIndex = Random.Range(0, 20);
        int y = 20;
        for(int i=20; i<40; i++)
        {
            y--;
            for(int j=19; j>=0; j--)
            {
                if(j == y && j != pathIndex && maze[i, j].tag != "start" && maze[i, j].tag != "end")
                {
                    yield return new WaitForSeconds(0.01f);
                    maze[i, j].SetWall();
                }
            }
        }

        yield return new WaitForSeconds(0.03f);
        if(GameManager.Instance.currentAlgorithm == AlgorithmEnum.BFS || GameManager.Instance.currentAlgorithm == AlgorithmEnum.DFS)
            GameManager.Instance.SetState(GameManager.Instance.stateCache["UnWeighted"]);
        else
            GameManager.Instance.SetState(GameManager.Instance.stateCache["Weighted"]);
    }


















    // I can't fix it QQQQQQQQQQQ;
    // Still Working On it....
    // @@@@@@@@@@@@@@@@@@@@@@@@@@@
    public void RecursiveDivisionMaze(Node[,] maze, int width, int height, int max_width, int max_height, bool horizontal)
    {
        //StartCoroutine(SpawSideWall(maze));
        StartCoroutine(RecursiveDivisionAnimation(maze, width, height, max_width, max_height, ChooseOrientation(width, height)));
    }

    private IEnumerator SpawSideWall(Node[,] maze)
    {
        yield return new WaitForSeconds(0.03f);
        for(int i=0; i<40; i++)
        { // Draw Width
            if(maze[i, 0].tag != "start" && maze[i, 19].tag != "end")
            {
                yield return new WaitForSeconds(0.001f);
                maze[i, 0].SetWall();
                maze[i, 19].SetWall();
            }
        }

        for(int i=19; i>=0; i--)
        {
            if(maze[0, i].tag != "start" && maze[39, i].tag != "end")
            {
                yield return new WaitForSeconds(0.001f);
                maze[0, i].SetWall();
                maze[39, i].SetWall();
            }
        }
    }

    private IEnumerator RecursiveDivisionAnimation(Node[,] maze, int x, int y, int width, int height, bool horizontal)
    {
        yield return new WaitForSeconds(0.001f);
        if(width < 2 || height < 2)
        {
            yield break;
        }

        Debug.Log("in");
        
        int wallIndex = 0;
        int pathIndex = 0;


        if(horizontal)
        {
            wallIndex = y + Random.Range(0, height - 2);
            pathIndex = Random.Range(x, width);
        }
        else
        {
            wallIndex = x + Random.Range(0, width - 2); 
            pathIndex = Random.Range(y, height);
        }
        
        // Build Walls
        // build horizontal wall
        if(horizontal)
        {
            for(int i=x; i< (x + width); i++)
            {
                yield return new WaitForSeconds(0.001f);
                if(i != pathIndex && maze[i, wallIndex].tag != "start" && maze[i, wallIndex].tag != "end") /// Set Path
                    maze[i, wallIndex].SetWall();
            }
        }
        else // build verticle wall
        {
            for(int i=y; i< (y + height); i++)
            {
                yield return new WaitForSeconds(0.001f);
                if(i != pathIndex && maze[wallIndex, i].tag != "start" && maze[wallIndex, i].tag != "end") /// Set Path
                    maze[wallIndex, i].SetWall();
            }
        }

        int newX = 0;
        int newY = 0;
        int new_width = 0;
        int new_height = 0;

        if(horizontal)
        {
            newX = x;
            newY = y;

            new_width = width;
            new_height = wallIndex - y + 1;
        }
        else
        {
            newX = x;
            newY = y;

            new_width = wallIndex - x + 1;
            new_height = height;
        }

        yield return StartCoroutine(RecursiveDivisionAnimation(maze, newX, newY, new_width, new_height, ChooseOrientation(new_width, new_height)));

        if(horizontal)
        {
            newX = x;
            newY = wallIndex + 1;
            
            new_width = width;
            new_height = y + height - wallIndex - 1;
        }
        else
        {
            newX = wallIndex + 1;
            newY = y;

            new_width = x + width - wallIndex - 1;
            new_height = height;
        }

        yield return StartCoroutine(RecursiveDivisionAnimation(maze, newX, newY, new_width, new_height, ChooseOrientation(new_width, new_height)));
    }

    public bool ChooseOrientation(int width, int height) 
    {
        if(width < height)
            return true;
        else if(width > height)
            return false;
        else if(width == height)
        {
            int index = Random.Range(0, 2);
            
            switch(index)
            {
                case 0:
                    return true;
                case 1:
                    return false;
            }
        }
        return true;
    }
}
