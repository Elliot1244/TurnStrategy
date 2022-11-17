using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private int _width;
    private int _height;
    private float _cellSize;
    private GridSystem<PathNode> _gridSystem;

    [SerializeField] Transform _gridDebugObjectPrefab;
    [SerializeField] bool _showDebug;
    [SerializeField] Transform _root;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private void Awake()
    {
        //Vérifie qu'il n'y ait qu'une instance sinon destruction
        if (Instance != null)
        {
            Debug.LogError("More than one PathFinding" + transform + " - " + Instance);
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;


    }

    public void SetUp(int width, int height, float cellSize)
    {
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellSize, 
            (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition), _root);
        if (_showDebug)
        {
            _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
        }

        //On défini les nodes qui ne sont pas marchable en émettant un raycast qui va toucher les obstacles uniquement
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float offsetRaycastDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * offsetRaycastDistance, Vector3.up, offsetRaycastDistance * 2, _obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        }
    }

    //Récupère une liste de deux gridPosition avec la position de début et celle de fin
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        //Liste des nodes dispo pour être cherchés
        List<PathNode> openList = new List<PathNode>();

        //Liste des nodes que l'on a déjà recherché
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        for (int x = 0; x < _gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < _gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFcost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFcost();

        //Tant que l'on a des nodes à chercher
        while (openList.Count > 0)
        {
            //On prend le node actuel et on le met en premier dans la liste des nodes que l'on peut rechercher
            PathNode currentNode = GetLowestFCostPathNode(openList);


            if (currentNode == endNode)
            {
                //On a fini la recherche des nodes
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourgList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                //Si le node n'est pas walkable, on le met dans la liste des nodes déjà recherchés
                if (!neighbourNode.IsWakable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFcost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //Pas de chemin valable trouvé
        return null;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remainingDistance = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remainingDistance;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            //Si le node que l'on vérifie a un coût F inférieur au node qui a le coût F le moins élevé
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                //Alors le node que l'on vérifie devient celui avec le coût en F le plus bas
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return _gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourgList(PathNode currentNode)
    {
        List<PathNode> neighbourgList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            //Node de gauche
            neighbourgList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0)
            {
                //Node de gauche bas
                neighbourgList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                //Node de gauche haut
                neighbourgList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }

        }

        if (gridPosition.x + 1 < _gridSystem.GetWidth())
        {
            //Node de droite
            neighbourgList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0));

            if (gridPosition.z - 1 >= 0)
            {
                //Node de droite bas
                neighbourgList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }

            if (gridPosition.z + 1 < _gridSystem.GetHeight())
            {
                //Node de droite haut
                neighbourgList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }

        if (gridPosition.z - 1 >= 0)
        {

            //Node d'en bas
            neighbourgList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1));
        }

        if (gridPosition.z + 1 < _gridSystem.GetHeight())
        {
            //Node d'en haut
            neighbourgList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1));
        }

        return neighbourgList;
    }

    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }


    //Renvoi une casse sur la grille qui est walkable
    public bool IsWalkableGridPosition(GridPosition gridPosition)
    {
        return _gridSystem.GetGridObject(gridPosition).IsWakable();
    }

}
