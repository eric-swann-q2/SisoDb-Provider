﻿using System.Collections;
using System.Linq;
using System.Text;
using SisoDb.NCore.Collections;

namespace SisoDb.Querying.Lambdas.Nodes
{
    public static class NodesExtensions
    {
        public static string AsString(this INode[] nodes)
        {
            var sb = new StringBuilder();
            foreach (var node in nodes)
            {
                sb.Append(node);

                if (node is OperatorNode)
                    sb.Append(" ");
            }
            return sb.ToString();
        }

        public static InSetMemberNode ToInSetNode(this MemberNode memberNode, IEnumerable values)
        {
            return new InSetMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, values.Yield().ToArray());
        }

        public static NotInSetMemberNode ToNotInSetNode(this MemberNode memberNode, IEnumerable values)
        {
            return new NotInSetMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, values.Yield().ToArray());
        }

        public static LikeMemberNode ToLikeNode(this MemberNode memberNode, string value)
        {
            return new LikeMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, value);
        }

        public static StringContainsMemberNode ToStringContainsNode(this MemberNode memberNode, string value)
        {
            return new StringContainsMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, value);
        }

        public static StringExactMemberNode ToStringExactNode(this MemberNode memberNode, string value)
        {
            return new StringExactMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, value);
        }

        public static StringEndsWithMemberNode ToStringEndsWithNode(this MemberNode memberNode, string value)
        {
            return new StringEndsWithMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, value);
        }

        public static StringStartsWithMemberNode ToStringStartsWithNode(this MemberNode memberNode, string value)
        {
            return new StringStartsWithMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode, value);
        }

        public static ToLowerMemberNode ToLowerNode(this MemberNode memberNode)
        {
            return new ToLowerMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode);
        }

        public static ToUpperMemberNode ToUpperNode(this MemberNode memberNode)
        {
            return new ToUpperMemberNode(memberNode.Path, memberNode.DataType, memberNode.DataTypeCode);
        }
    }
}