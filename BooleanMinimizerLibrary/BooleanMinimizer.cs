using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BooleanMinimizerLibrary
{
    public class BooleanMinimizer
    {
        public static string MinimizeMKNF(string vector)
        {
            var maxterms = GetMaxterms(vector);
            var variables = GetVariables(vector.Length);
            var terms = maxterms.Select(m => BuildMaxterm(m, variables));
            return terms.Any() ? string.Join(" ∧ ", terms) : "1"; // 1 для тривиального случая
        }

        public static string MinimizeMDNF(string vector)
        {
            var minterms = GetMinterms(vector);
            var variables = GetVariables(vector.Length);
            var terms = minterms.Select(m => BuildMinterm(m, variables));
            return terms.Any() ? string.Join(" ∨ ", terms) : "0"; // 0 для тривиального случая
        }

        private static List<int> GetMinterms(string vector)
        {
            var indices = new List<int>();
            for (int i = 0; i < vector.Length; i++)
                if (vector[i] == '1') indices.Add(i);
            return indices;
        }

        private static List<int> GetMaxterms(string vector)
        {
            var indices = new List<int>();
            for (int i = 0; i < vector.Length; i++)
                if (vector[i] == '0') indices.Add(i);
            return indices;
        }

        private static List<string> GetVariables(int vectorLength)
        {
            int numVars = (int)Math.Log(vectorLength, 2);
            return Enumerable.Range(0, numVars)
                .Select(i => i switch { 0 => "w", 1 => "x", 2 => "y", 3 => "z" })
                .ToList();
        }

        private static string BuildMinterm(int index, List<string> variables)
        {
            string binary = Convert.ToString(index, 2).PadLeft(variables.Count, '0');
            var literals = binary.Select((bit, i) => bit == '1' ? variables[i] : $"¬{variables[i]}");
            return $"({string.Join(" ∧ ", literals)})";
        }

        private static string BuildMaxterm(int index, List<string> variables)
        {
            string binary = Convert.ToString(index, 2).PadLeft(variables.Count, '0');
            var literals = binary.Select((bit, i) => bit == '0' ? variables[i] : $"¬{variables[i]}");
            return $"({string.Join(" ∨ ", literals)})";
        }
    }
}