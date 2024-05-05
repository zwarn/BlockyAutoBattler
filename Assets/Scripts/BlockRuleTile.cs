using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockRuleTile : RuleTile
{
    public BlockType type;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is BlockRuleTile otherRuleTile)
        {
            switch (neighbor)
            {
                case TilingRuleOutput.Neighbor.This: return otherRuleTile.type == this.type;
                case TilingRuleOutput.Neighbor.NotThis: return otherRuleTile.type != this.type;
            }
        }

        return base.RuleMatch(neighbor, other);
    }

    public enum BlockType
    {
        Red
    }
}