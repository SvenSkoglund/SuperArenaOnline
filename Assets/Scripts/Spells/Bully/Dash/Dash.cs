using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static Primeable;

public class Dash : Spell, Primeable
{
    private float calcualteCost;
    public override float range { get { return 0; } }
    public override float damage { get { return 10; } }
    public override float cooldown { get { return 10; } }

    public override string pathToIcon { get { return "ArtFiles/DashIcon"; } }

    public override float cost { get { return calcualteCost; } }

    [SerializeField] public LineRenderer dashEffect;

    public float effectFadeSpeed;
    public float effectAlpha;

    public SpellIcon dashIcon;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        calcualteCost = 10;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = player.GetComponent<ClickHandler>().getMousePosition();
        Vector2 playerPosition = player.transform.position;
        float distance = Vector2.Distance(mousePosition, playerPosition);
        calcualteCost = 10 + distance * 2;

    }


    public override bool Cast()
    {
        bool wasCast = false;
        effectAlpha = 1f;

        Vector2 mousePosition = player.GetComponent<ClickHandler>().getMousePosition();
        Vector2 playerPosition = player.transform.position;
        // foreach (RaycastHit2D hit in hits)
        // {

        //     if (hit != null && hit.collider != null)
        //     {
        //         if (hit.collider.gameObject.GetComponent<EnemyController>() != null)
        //         {
        //             // CombatText combatText = hit.collider.gameObject.AddComponent(typeof(CombatText)) as CombatText;
        //             // combatText.text= damage.ToString();
        //             CombatText.Create(hit.collider.gameObject.transform.position, 100.ToString(), false);
        //             StatusText.Create(hit.collider.gameObject.transform.position, "Stunned", false);
        //             hit.collider.gameObject.GetComponent<CombatTextFactory>().addCombatText("Hit Enemy with dash");
        //             Debug.Log("Hit Enemy with dash");

        //         }
        //     }
        // }


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.name.Contains("Floor"))
            {
                Vector3 newPosition = hit.point;
                newPosition.y = .2f;
                player.transform.position = newPosition;
                DashEffect.Create(playerPosition, newPosition);
                Debug.Log(hit.point);
            }
        }


        wasCast = true;

        Debug.Log("Cast Dash");
        return wasCast;
    }

    public bool Prime()
    {
        bool wasCast = false;
        return wasCast;
    }

}
