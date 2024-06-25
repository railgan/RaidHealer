using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public Canvas canvas; // Assign your Canvas here
    private List<GameObject> buffSymbols = new List<GameObject>();
    public int nextBuffId = 0; // Keeps track of the next available ID


    private static BuffManager _instance;

    public static BuffManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BuffManager>();
                if (_instance == null)
                {
                    Debug.LogError("BuffManager not found in scene.");
                }
            }
            return _instance;
        }
    }

    public void AddBuffSymbol(string buffName, int buffID, GameObject buffTarget)
    {
        GameObject buffSymbolPrefab = Resources.Load<GameObject>("Prefabs/UI/BuffSymbol");
        GameObject buffSymbol = Instantiate(buffSymbolPrefab, canvas.transform);
        buffSymbol.GetComponentInChildren<TMPro.TMP_Text>().text = buffName;  // Use TMP_Text

        // Calculate position slightly above the buff target
        Vector3 targetPosition = buffTarget.transform.position;
        Vector3 offset = new Vector3(0f, 0.5f, 0f); // Adjust offset for desired placement above target
        buffSymbol.transform.position = targetPosition + offset;

        buffSymbol.GetComponent<BuffSymbolData>().id = buffID;
        buffSymbols.Add(buffSymbol);
        UpdateBuffSymbols();
    }

    public void RemoveBuffSymbol(int buffId)
    {
        Debug.Log("Trying to remove a buff: " + buffId);
        int index = buffSymbols.FindIndex(symbol => symbol.GetComponent<BuffSymbolData>().id == (buffId));
        if (index != -1)
        {
            Debug.Log("Removing a buff");
            GameObject buffSymbol = buffSymbols[index];
            buffSymbols.RemoveAt(index);
            Destroy(buffSymbol);
        }
        UpdateBuffSymbols();
    }

    private void UpdateBuffSymbols()
    {
        for (int i = 0; i < buffSymbols.Count; i++)
        {
            buffSymbols[i].transform.localPosition = new Vector3(i * 2, 0, 0); // Adjust spacing and position as needed
        }
    }

    public int GetNextBuffId() // New method to provide next ID
    {
        nextBuffId++;
        return nextBuffId - 1; // Return the previously assigned ID
    }

}
