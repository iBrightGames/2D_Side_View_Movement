using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DemoSetupGenerator : MonoBehaviour
{
    [Range(1,10)] public int horizontalSpacing = 2;
    [Range(1,10)] public int vertcalSpacing = 2;
    public GameObject playerPrefab;
    public InputConfig[] InputConfigs;
    public ForceConfig[] ForceConfigs;

    [Header("Line Settings")]
    public Vector2 v_startPoint;
    public Vector2 v_endPoint;
    public Vector2 h_startPoint;
    public Vector2 h_endPoint;

    void Awake()
    {
        // Boş obje oluştur
        GameObject lineObj = new GameObject("LineCollider");
        lineObj.transform.parent = transform;
        Vector2 start = new(-horizontalSpacing / 2, -vertcalSpacing / 2);
        h_startPoint = start;
        v_startPoint = start;
        h_endPoint = start;
        h_endPoint.x += horizontalSpacing * InputConfigs.Count();
        v_endPoint = start;
        v_endPoint.y += vertcalSpacing * ForceConfigs.Count();

        // EdgeCollider2D ekle
        EdgeCollider2D h_edge = lineObj.AddComponent<EdgeCollider2D>();
        h_edge.points = new Vector2[] { h_startPoint,h_endPoint };
        // EdgeCollider2D ekle
        EdgeCollider2D v_edge = lineObj.AddComponent<EdgeCollider2D>();
        v_edge.points = new Vector2[] { v_startPoint, v_endPoint };
        
        foreach (var _ in InputConfigs)
        {
            GameObject line = new GameObject($"VLine_");
            line.transform.parent = transform;
            EdgeCollider2D v_edge_inner = line.AddComponent<EdgeCollider2D>();
            v_startPoint.x += horizontalSpacing;
            v_endPoint.x += horizontalSpacing;
            v_edge_inner.points = new Vector2[] { v_startPoint, v_endPoint };

        }
            
        foreach (var _ in ForceConfigs)
        {
            GameObject line = new GameObject($"HLine_");
            line.transform.parent = transform;
            EdgeCollider2D h_edge_inner = line.AddComponent<EdgeCollider2D>();
            h_startPoint.y += vertcalSpacing;
            h_endPoint.y +=  vertcalSpacing;
            h_edge_inner.points = new Vector2[] { h_startPoint, h_endPoint };
            
        }
       

        int i = 0;
        foreach (var input in InputConfigs)
        {
            int j = 0;
            foreach (var force in ForceConfigs)
            {

                InputMovementBridge bridge = new InputMovementBridge();
                bridge.inputConfig = input;
                bridge.movementConfig = force;
                GameObject p = Instantiate(playerPrefab, transform);
                Vector3 pos = p.transform.position;
                pos.x += i * horizontalSpacing;
                pos.y += j * vertcalSpacing;
                p.transform.position = pos;

                PlayerMovementController controller = p.GetComponent<PlayerMovementController>();
                controller.inputMovementBridges.Add(bridge);
                controller.Initialize();

                j += 1;
            }
            i += 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
