using System.Text.RegularExpressions;

namespace SolrNet {
	public class SolrQueryByField<T> : AbstractSolrQuery<T> where T : ISolrDocument {
		private string q;

		public SolrQueryByField(string fieldName, string fieldValue) {
			if (fieldName == null || fieldValue == null)
				q = null;
			else
				q = string.Format("{0}:{1}", fieldName, quote(fieldValue));
		}

		public string quote(string value) {
			string r = Regex.Replace(value, "(\\+|\\-|\\&\\&|\\|\\||\\!|\\{|\\}|\\[|\\]|\\^|\\(|\\)|\\\"|\\~|\\:|\\\\)", "\\$1");
			if (r.IndexOf(' ') != -1)
				r = string.Format("\"{0}\"", r);
			return r;
		}

		/// <summary>
		/// query string
		/// </summary>
		public override string Query {
			get { return q; }
		}
	}
}