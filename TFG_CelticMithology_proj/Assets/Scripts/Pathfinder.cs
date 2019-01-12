using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //Map settings
    int map_width = 0;
    int map_height = 0;

    //Pathfind
    List<PathNode> open_list;
    List<PathNode> close_list;
    List<PathNode> path_list;

    public Tilemap walkability;
    public Tile walkable_tile;
    public Tile non_walkable_tile;

    // Use this for initialization
    public void Start()
    {
        //Pathfinding
        open_list = new List<PathNode>();
        close_list = new List<PathNode>();
    }

    void Update()
    {

    }

    public List<PathNode> CalculatePath(PathNode origin, PathNode destiny)
    {
        //Clean both lists
        open_list.Clear();
        close_list.Clear();

        //Add my current position to the open list of them
        open_list.Add(origin);

        while (open_list.Count > 0)
        {
            if (IsInCloseList(destiny) != null)
                break;

            //Get the nearest node
            PathNode current_tile = GetLowestScoreNode();
            close_list.Add(current_tile);

            //Erase it form the open list
            open_list.Remove(current_tile);

            //Get the adjacent walkable nodes of the lowest node
            List<PathNode> adjacent_nodes = GetWalkableAdjacents(current_tile);

            foreach (PathNode node in adjacent_nodes)
            {
                //If it is in the closed list we will ignore it
                if (IsInCloseList(node) != null)
                    continue;

                //If it isn't in the open list we will set it and push it to the list

                PathNode tmp_node = IsInOpenList(node);

                if (tmp_node == null)
                {
                    node.CalculateDistance(destiny);
                    node.CalculateCost();
                    node.SetParent(current_tile);

                    open_list.Add(node);
                }
                else
                {
                    //If it is in the open list we will check if with the new parent
                    //it's score is lower than before, and if it is update;
                    PathNode actual_parent = tmp_node.GetParent();
                    int old_score = tmp_node.GetScore();

                    tmp_node.SetParent(current_tile);
                    int new_score = tmp_node.GetScore();

                    if (old_score < new_score)
                        tmp_node.SetParent(actual_parent);
                }
            }


        }

        FillPathList();


        return path_list;

    }

    public PathNode GetLowestScoreNode()
    {

        if (open_list.Count == 0)
        {
            return null;
        }

        PathNode ret = open_list[0];

        foreach (PathNode node in open_list)
        {
            if (node.GetScore() < ret.GetScore())
                ret = node;
        }

        return ret;
    }

    private PathNode IsInCloseList(PathNode node)
    {
        foreach (PathNode node_in_list in close_list)
        {
            if (node_in_list.IsEqual(node))
                return node_in_list;
        }

        return null;
    }

    private PathNode IsInOpenList(PathNode node)
    {
        foreach (PathNode node_in_list in open_list)
        {
            if (node_in_list.IsEqual(node))
                return node_in_list;
        }

        return null;
    }

    private void FillPathList()
    {
        path_list = new List<PathNode>();

        PathNode new_node = close_list[close_list.Count - 1];

        path_list.Add(new_node);

        while (new_node.GetParent() != null)
        {
            new_node = new_node.GetParent();
            path_list.Add(new_node);
        }

        path_list.Reverse();
        path_list.Remove(path_list[0]);

    }

    public int GetMapWidth()
    {
        return map_width;
    }

    public int GetMapHeight()
    {
        return map_height;
    }

    public List<PathNode> GetWalkableAdjacents(PathNode node, uint range = 1)
    {
        List<PathNode> ret = new List<PathNode>();

        int x_pos = node.GetTileX();
        int y_pos = node.GetTileY();


        if (IsWalkableTile(x_pos + (int)range, y_pos))
        {
            ret.Add(new PathNode(x_pos + (int)range, y_pos));
        }

        if (IsWalkableTile(x_pos - (int)range, y_pos))
        {
            ret.Add(new PathNode(x_pos - (int)range, y_pos));
        }

        if (IsWalkableTile(x_pos, y_pos + (int)range))
        {
            ret.Add(new PathNode(x_pos, y_pos + (int)range));
        }

        if (IsWalkableTile(x_pos, y_pos - (int)range))
        {
            ret.Add(new PathNode(x_pos, y_pos - (int)range));
        }

        //right up diagonal
        if (IsWalkableTile(x_pos + (int)range, y_pos + (int)range))
        {
            ret.Add(new PathNode(x_pos + (int)range, y_pos + (int)range));
        }

        //left up diagonal
        if (IsWalkableTile(x_pos - (int)range, y_pos + (int)range))
        {
            ret.Add(new PathNode(x_pos - (int)range, y_pos + (int)range));
        }

        //right down diagonal
        if (IsWalkableTile(x_pos + (int)range, y_pos - (int)range))
        {
            ret.Add(new PathNode(x_pos + (int)range, y_pos - (int)range));
        }

        //left down diagonals
        if (IsWalkableTile(x_pos - (int)range, y_pos - (int)range))
        {
            ret.Add(new PathNode(x_pos - (int)range, y_pos - (int)range));
        }

        return ret;
    }

    public bool IsWalkableTile(int x_t, int y_t)
    {
        bool ret = false;

        Vector3Int position_tile = new Vector3Int(x_t, y_t, 0);
        TileBase tile_temp = walkability.GetTile(position_tile);
        if(tile_temp == walkable_tile)
        {
            ret = true;
        }
        else
        {
            ret = false;
        }

        return ret;
    }

}

