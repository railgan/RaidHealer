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
/*
    public void AddBuffSymbol(string buffName, int buffID, GameObject buffTarget)
    {
        GameObject buffSymbolPrefab = Resources.Load<GameObject>("Prefabs/UI/BuffSymbol");
        GameObject buffSymbol = Instantiate(buffSymbolPrefab, buffTarget.transform); // Attach to buffTarget

        // Get the Rect Transform component of the buff symbol
        RectTransform rectTransform = buffSymbol.GetComponent<RectTransform>();

        // Set the anchor point to the top left corner relative to the buff target
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);

        // Set the pivot point to the top left corner
        rectTransform.pivot = new Vector2(0.5f, 1f);

        // Set the text
        buffSymbol.GetComponentInChildren<TMPro.TMP_Text>().text = buffName[..1];

        // Adjust the offset to position the buff symbol slightly above the buff target
        Vector3 offset = new Vector3(0f, 0.85f, 0f); // Adjust offset as needed
        rectTransform.anchoredPosition = offset;

        buffSymbol.GetComponent<BuffSymbolData>().id = buffID;
        buffSymbols.Add(buffSymbol);
    }

    public void RemoveBuffSymbol(int buffId)
    {

        int index = buffSymbols.FindIndex(symbol => symbol.GetComponent<BuffSymbolData>().id == (buffId));
        if (index != -1)
        {

            GameObject buffSymbol = buffSymbols[index];
            buffSymbols.RemoveAt(index);
            Destroy(buffSymbol);
        }
    }
*/


    public int GetNextBuffId() // New method to provide next ID
    {
        nextBuffId++;
        return nextBuffId - 1; // Return the previously assigned ID
    }

}
