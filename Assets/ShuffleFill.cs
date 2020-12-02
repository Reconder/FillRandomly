using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShuffleFill : MonoBehaviour
{

    //METHOD BEGINS HERE --------------------------------------------------------------------------------------------------------

    //Shuffling method. OPTIONAL. Instead you could use LINQ, as shown in FillRandomly()
    public static List<T> Shuffle<T>(List<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var randomIndex = UnityEngine.Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = tmp;
        }
        return list;
    }

    //Required method for the task
    public void FillRandomly(GameObject[] array1, GameObject[] array2)
    {
        // Make a list of non null GameObjects of array2
        var array2Clear = new List<GameObject>(array2);
        array2Clear.RemoveAll(item => item == null);

        //Get list of indexes of elements of array1 that are null
        var nullIndex = array1.Select((item, index) => new { item, index }).Where(pair => pair.item == null).Select(pair => pair.index).ToList();

        //Shuffle the list of indexes
        //var nullIndecies = nullIndex.OrderBy(item => UnityEngine.Random.value).ToList(); // Quick to write, but slow to perform   
        var nullIndecies = Shuffle(nullIndex);        // Requires a shuffle method to be written, but is faster to perform than LINQ way

        //Insert the references to the elements of array2 in empty elements of array1
        for (int i = 0; i < nullIndecies.Count; i++)
        {
            if (i >= array2Clear.Count) { return; }
            int index = nullIndecies[i];
            array1[index] = array2Clear[i];
        }
    }
    // METHOD ENDS HERE ---------------------------------------------------------------------------------------------------------


    // TESTING STARTS HERE ------------------------------------------------------------------------------------------------------

    //Prefab for instantiation
    public GameObject prefab;

    //To see arrays in inspector
    public GameObject[] array1check = new GameObject[50];
    public GameObject[] array1 = new GameObject[50];
    public GameObject[] array2 = new GameObject[50];


    void Start()
    {

        GenerateArray(array1, 1, 0.5f);
        array1check = (GameObject[])array1.Clone();
        GenerateArray(array2, 2, 0.5f);
        FillRandomly(array1, array2);
        ShowArray(array1);
    }

    //Show array in Debug console
    private void ShowArray(GameObject[] array1)
    {
        string output = "";
        int count = 0; ;
        foreach (var item in array1)
        {
            if (item != null)
            {
                output += $", {item.name}";
                count++;
            }
            else
            {
                output += ", null";
            }
        }
        Debug.Log(output);
        Debug.Log(count);
    }

    //Generate array with random nulls and objects. arrayNum is used for indexing which array object originaly belongs to (for FillRandomly()), 
    //randomThreshold is for probability of object to be null
    private void GenerateArray(GameObject[] array1, int arrayNum, float randomThreshold)
    {
        int notNullCount = 0;
        for (int i = 0; i < array1.Length; i++)
        {
            var randomNumber = UnityEngine.Random.value;
            if (randomNumber > randomThreshold)
            {
                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y, transform.position.z);
                array1[i] = Instantiate(prefab, pos, transform.rotation) as GameObject;
                array1[i].name = $"Cube arr{arrayNum} #{i}";
                notNullCount++;
            }
            else
            {
                array1[i] = null;
                
            }
        }
        Debug.Log(notNullCount);
    }


    // TESTING ENDS HERE --------------------------------------------------------------------------------------------------------
}
