using System.Collections.Generic;
using System.Linq;
using ParserLib.Parsing;
using ParserLib.Parsing.Rules;

namespace ParserLib
{
	/// <summary>
	/// Extensions for pretty formatting
	/// </summary>
	public static class Extensions
    {
		/// <summary>
		/// Makes a nice tree view of the children rules.
		/// </summary>
		/// <param name="rule">The rule.</param>
		/// <returns>System.String.</returns>
		public static string PrettyFormat(this Rule rule) => Util.PrettyFormat(rule, r => r.GetChildren());

		/// <summary>
		/// Makes a nice tree view of the node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <returns>System.String.</returns>
		public static string PrettyFormat(this Node node) => Util.PrettyFormat(node, n => n.ChildLeafs);

		/// <summary>
		/// Makes a nice tree view of the nodes.
		/// </summary>
		/// <param name="nodes">The nodes.</param>
		/// <returns>IEnumerable&lt;System.String&gt;.</returns>
		public static IEnumerable<string> PrettyFormat(this IEnumerable<Node> nodes) => nodes.Select(PrettyFormat);
    }
}