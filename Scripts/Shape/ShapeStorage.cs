using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShapeStorage : MonoBehaviour
{
    public List<ShapeData> shapeData;
    public List<Shape> shapeList;

    private void OnEnable()
    {
        GameEvent.RequestNewShape += RequestNewShape;
    }
    private void OnDisable()
    {
        GameEvent.RequestNewShape -= RequestNewShape;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.CreateShape(shapeData[shapeIndex]);
        }
    }

    public Shape GetCurrentSelectedShape()
    {
        foreach(var shape in shapeList)
        {
            if(shape.IsOnStartPosition() == false && shape.IsAnyOfShapeSquareActive())
            {
                return shape;
            }
        }
        Debug.LogError("No shape is selected");
        return null;
    }

    private void RequestNewShape()
    {
        foreach(var shape in shapeList)
        {
            var shapeIndex = UnityEngine.Random.Range(0, shapeData.Count);
            shape.RequestNewShape(shapeData[shapeIndex]);
        }
    }

}
