using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generate a field of GameObjects
/// can have padding between them
/// </summary>
public class ObjectFieldGenerator : MonoBehaviour
{

    #region inspector values

    [SerializeField]
    [Tooltip("Object to generate a field from")]
    GameObject Prefab;

    [SerializeField]
    public int FieldSizeX = 2;

    [SerializeField]
    public int FieldSizeY = 2;

    [SerializeField]
    [Tooltip("Turn this to not generate on Start()")]
    public bool CanGenerateSelf = true;

    [SerializeField]
    [Tooltip("Spacing between objects")]
    Vector2 Padding = new Vector2(.5f, .5f);

    [Space(5f)]
    [Header("In Editor Generation")]

    [SerializeField]
    bool GenerateInEditor = false;

    [SerializeField]
    bool ClearInEditor = false;

    #endregion

    #region public values

    public List<GameObject> ObjectList;

    #endregion


    #region private values

    Vector2 PrefabHalfSize = new Vector2();

    #endregion

    void Start()
    {
        if(CanGenerateSelf)
            StartGeneration();
    }

    public void StartGeneration()
    {
        if (FieldSizeX <= 1 || FieldSizeY <= 1 || Prefab == null || ObjectList.Count > 0)
            return;

        ObjectList = new List<GameObject>();


        // Calculate Object size in 2D Space
        CalcObjectSize();

        // Generate a Field of Objects 
        GenerateField();
    }

    private void OnValidate()
    {
        if (GenerateInEditor)
        {
            if (FieldSizeX <= 1 || FieldSizeY <= 1 || Prefab == null)
                return;

            Clear();

            ObjectList = new List<GameObject>();

            CalcObjectSize();
            GenerateField();

            GenerateInEditor = false;
        }

        if (ClearInEditor)
        {
            Clear();
            ClearInEditor = false;
        }
    }

    void CalcObjectSize()
    {
        RectTransform transform;

        if (transform = Prefab.GetComponentInChildren<RectTransform>())
        {
            // Is UI
            PrefabHalfSize = new Vector2(transform.localScale.x * transform.sizeDelta.x, transform.localScale.y * transform.sizeDelta.y);
        }
        else
        {
            // Is GameObject
            Collider collider = Prefab.GetComponentInChildren<Collider>();
            if (collider)
            {
                PrefabHalfSize.x = collider.bounds.size.x / 2;
                PrefabHalfSize.y = collider.bounds.size.z / 2;
            }
        }
    }

    /// <summary>
    /// Generate all objects as a field
    /// </summary>
    void GenerateField()
    {

        // Calculate middle point of field
        Vector3 initPos = new Vector3(
            transform.position.x - ((PrefabHalfSize.x + Padding.x) / 2) * (FieldSizeX - 1),
            transform.position.y,
            transform.position.z - ((PrefabHalfSize.y + Padding.y) / 2) * (FieldSizeY - 1));

        Vector3 spawnPosition = Vector3.zero;

        // Generate field of Objects
        for (int i = 0; i < FieldSizeY; i++)
        {
            for (int j = 0; j < FieldSizeX; j++)
            {
                spawnPosition = new Vector3(
                    initPos.x + (PrefabHalfSize.x + Padding.x) * j,
                    initPos.y,
                    initPos.z + (PrefabHalfSize.y + Padding.y) * i);

                // Add Objects to List to keep Track
                ObjectList.Add(Instantiate(Prefab, spawnPosition, Quaternion.identity, transform));
            }
        }
    }

    /// <summary>
    /// Clear all objects
    /// </summary>
    public void Clear()
    {

        foreach (GameObject go in ObjectList)
        {
            go.SetActive(false);

#if UNITY_EDITOR
            StartCoroutine(DestroyInEditor(go));
#else
            Destroy(go);
#endif
        }

        ObjectList.Clear();
    }

    /// <summary>
    /// Workaround to destroy objects in Editor without custom Unity UI
    /// </summary>
    IEnumerator DestroyInEditor(GameObject go)
    {
        yield return null;
        DestroyImmediate(go);
    }
}
