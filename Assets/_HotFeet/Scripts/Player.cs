using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    [SerializeField] private List<Vector3> trailPos;
    public Vector3 prevPos;
    [SerializeField] private int trailLenght;

    [SerializeField] private LineRenderer line;

    [SerializeField] private GameObject prefab;
    [SerializeField] private List<GameObject> prefabs;

    [SerializeField] private int setIndex;
    [SerializeField] private bool isSlicing;

    public static Action OnDeath;
    private const float isSlicingDelay = 1;

    private void OnEnable()
    {
        instance = this;
        ManagerUI.OnNextLevel += Clear;
    }

    private void OnDisable()
    {
        ManagerUI.OnNextLevel -= Clear;
    }

    void Start()
    {
        line = GetComponent<LineRenderer>();
        prevPos = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(prevPos, transform.position) > 1)
        {
            prevPos = transform.position;
            trailPos.Add(transform.position);
            var inst = Instantiate(prefab, transform.position, Quaternion.identity);
            inst.name = setIndex.ToString();
            setIndex++;
            prefabs.Add(inst);
            line.positionCount = trailPos.Count;
            line.SetPosition(trailPos.Count - 1,trailPos[trailPos.Count - 1]);
            if (trailPos.Count > trailLenght)
            {
                trailPos.RemoveAt(0);
                
                Destroy(prefabs[0].gameObject);
                prefabs.RemoveAt(0);

                int newPositionCount = line.positionCount - 1;
                Vector3[] newPositions = new Vector3[newPositionCount];
 
                for (int i = 0; i < newPositionCount; i++){
                    newPositions[i] = line.GetPosition(i + 1);
                }
 
                line.SetPositions(newPositions);
            }
        }

        if (line.positionCount > 0)
            line.SetPosition(line.positionCount -1,transform.position);

        if (transform.position.y < -1)
            OnDeath?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Segment"))
        {
            if (!isSlicing && prefabs.Count > 2 && other.gameObject.name != prefabs[prefabs.Count - 1].name)
            {
                List<Vector2> vertices2D = new List<Vector2>();
                isSlicing = true;
                int num = int.Parse(other.gameObject.name);
                bool loadAll = false;
                for (int i = 0; i < prefabs.Count; i++)
                {
                    if (int.Parse(prefabs[i].name) == num)
                        loadAll = true;
                    if (loadAll)
                        vertices2D.Add(new Vector2(prefabs[i].transform.position.x,prefabs[i].transform.position.z));
                }
                MeshGeneration.instance.MeshGen(vertices2D.ToArray(),new Vector3(0,0.255f,0));
                Clear();
                StartCoroutine(CanGenerateMeshAgain());
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            OnDeath?.Invoke();
        }
        if (other.gameObject.CompareTag("GeneratedMesh"))
        {
            gameObject.SetActive(false);
            OnDeath?.Invoke();
        }
    }

    IEnumerator CanGenerateMeshAgain()
    {
        yield return new WaitForSeconds(isSlicingDelay);
        isSlicing = false;
    }

    public void Clear()
    {
        StartCoroutine(WaitClear());
    }
    IEnumerator WaitClear()
    {
        yield return new WaitForSeconds(0.3f);
        line.positionCount = 0;
        foreach (var prefab in prefabs)
        {
            // TODO: Object pooling
            Destroy(prefab);
        }
        prefabs.Clear();
        trailPos.Clear();
        setIndex = 0;
    }
}