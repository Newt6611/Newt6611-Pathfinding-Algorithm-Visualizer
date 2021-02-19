using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private LayerMask characterLayer;

    private bool isRun;
    private int index = 0;
    private List<Node> m_path;

    private Animator ani;

    private void Start() 
    {
        ani = GetComponent<Animator>();
        GameManager.Instance.StopAllActionEvent += StopRun;
    }

    private void FixedUpdate()
    {
        if(isRun)
        {
            Vector3 target = new Vector3(m_path[index].transform.position.x, transform.position.y, m_path[index].transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, 15 * Time.fixedDeltaTime);

            transform.LookAt(target);

            if(Vector3.Distance(transform.position, target) < 0.3f)
                index++;

            if(index == m_path.Count)
            {
                isRun = false;
                ani.SetBool("isRun", false);
                GameManager.Instance.isFinding = false;
            }
        }
    }

    public void Run(List<Node> path)
    {
        index = 0;
        StartCoroutine(RunAnimation(path));
    }
    
    private IEnumerator RunAnimation(List<Node> path)
    {
        ani.SetTrigger("dance");

        path.Reverse();
        m_path = path;

        yield return new WaitForSeconds(1.0f);
        
        isRun = true;
        ani.SetBool("isRun", true);
    }

    private void StopRun()
    {
        isRun = false;
        ani.SetBool("isRun", false);
        GameManager.Instance.isFinding = false;
    }
}
