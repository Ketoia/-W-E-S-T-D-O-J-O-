using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorV2 : MonoBehaviour
{
    public int seed;
    public bool random;
    
    // Start is called before the first frame update
    
    GeneratorCalculationV2 genCalc;
    List<Vector2> clearRooms = new List<Vector2>();
    void Start()
    {
        if (random)
            seed = Random.Range(1000000, 9999999);

        genCalc = new GeneratorCalculationV2();
        List<Vector2> positions = new List<Vector2>();
        positions.Add(new Vector2(0, 0));
        for (int i = 0; i < 1; i++)
        {
            List<Vector2> positionsOfTheSameType = new List<Vector2>();
            for (int x = 0; x < 1; x++)
            {
                Vector2 actualPos = genCalc.RandomPosition(seed, 4, 2, 1, positions, positionsOfTheSameType);
                positions.Add(actualPos);
                positionsOfTheSameType.Add(actualPos);
                
            }
        }
        List<Vector2> grid = genCalc.GridCalc(positions);
        for (int i = 0; i < positions.Count; i++)
        {
            List<Vector2> testList = genCalc.FindCorrectPath(Vector2.zero, positions[i], grid, positions);
            for (int x = 0; x < testList.Count; x++)
            {
                Debug.Log(testList[x]);

            }
        }

    }


}
struct RooomSpecX
{

}
