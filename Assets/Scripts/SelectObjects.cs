using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SelectObjects : MonoBehaviour
{

    public static List<GameObject> unit;
    public static List<GameObject> unitSelected;

    private Rect rect;
    private bool draw;
    private Vector2 startPos;
    private Vector2 endPos;

    void Awake()
    {
        unit = new List<GameObject>();
        unitSelected = new List<GameObject>();
    }

    bool CheckUnit(GameObject unit)
    {
        bool result = false;
        foreach (GameObject u in unitSelected)
        {
            if (u == unit) result = true;
        }
        return result;
    }

    void Select()
    {
        if (unitSelected.Count > 0)
        {
            for (int j = 0; j < unitSelected.Count; j++)
            {
                if (unitSelected[j].GetComponent<Planet>().planetOwner == ObjectOwner.ControlledByPlayer)
                {
                    unitSelected[j].GetComponent<Planet>().selectedPlanet.SetActive(true);
                    PlayerController.instance.homePlanet.Add(unitSelected[j].GetComponent<Planet>());
                    unitSelected[j].GetComponent<Planet>().selected = true;
                }

            }
        }
        PlayerController.instance.homePlanet = PlayerController.instance.homePlanet.Distinct().ToList();
    }

    void Deselect()
    {
        if (unitSelected.Count > 0)
        {
            for (int j = 0; j < unitSelected.Count; j++)
            {
                if (unitSelected[j].GetComponent<Planet>().planetOwner == ObjectOwner.ControlledByPlayer)
                {
                    unitSelected[j].GetComponent<Planet>().selectedPlanet.SetActive(false);
                    unitSelected[j].GetComponent<Planet>().selected = false;
                    PlayerController.instance.homePlanet.Clear();
                }

            }
        }
    }

    void OnGUI()
    {
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0))
        {
            Deselect();
            startPos = Input.mousePosition;
            draw = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            draw = false;
            Select();
        }

        if (draw)
        {
            unitSelected.Clear();
            endPos = Input.mousePosition;
            if (startPos == endPos) return;

            rect = new Rect(Mathf.Min(endPos.x, startPos.x),
                            Screen.height - Mathf.Max(endPos.y, startPos.y),
                            Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
                            Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
                            );

            GUI.Box(rect, "");

            for (int j = 0; j < unit.Count; j++)
            {

                Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(unit[j].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(unit[j].transform.position).y);

                if (rect.Contains(tmp))
                {
                    if (unitSelected.Count == 0)
                    {
                        unitSelected.Add(unit[j]);
                    }
                    else if (!CheckUnit(unit[j]))
                    {
                        unitSelected.Add(unit[j]);
                    }
                }
            }
        }
    }
}