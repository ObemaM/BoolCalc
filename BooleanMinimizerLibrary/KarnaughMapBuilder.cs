using System;
using System.Collections.Generic;
using System.Linq;

namespace BooleanMinimizerLibrary
{
    public class KarnaughMapBuilder
    {
        public List<List<string>> Build(Node root, List<string> variables = null)
        {
            if (variables == null)
            {
                variables = GetVariables(root).OrderBy(v => v).ToList();
            }

            int varCount = variables.Count;
            if (varCount < 2 || varCount > 4)
            {
                throw new Exception("Карта Карно поддерживает от 2 до 4 переменных");
            }

            string vector = new FunctionVectorBuilder().BuildVector(root);
            var map = new List<List<string>>();

            if (varCount == 2)
            {
                return BuildForTwoVariables(vector, variables);
            }
            else if (varCount == 3)
            {
                return BuildForThreeVariables(vector, variables);
            }
            else // 4 variables
            {
                return BuildForFourVariables(vector, variables);
            }
        }

        private List<List<string>> BuildForTwoVariables(string vector, List<string> variables)
        {
            var map = new List<List<string>>
            {
                new List<string> { variables[0] + "\\" + variables[1], "0", "1" },
                new List<string> { "0", vector[0].ToString(), vector[1].ToString() },
                new List<string> { "1", vector[2].ToString(), vector[3].ToString() }
            };
            return map;
        }

        private List<List<string>> BuildForThreeVariables(string vector, List<string> variables)
        {
            var map = new List<List<string>>
            {
                // Исправлен заголовок - первая переменная отдельно, остальные объединены
                new List<string> { $"{variables[0]} \\ {variables[1]}{variables[2]}", "00", "01", "11", "10" }
            };

            // Остальной код оставляем без изменений
            map.Add(new List<string> { "0",
                vector[0].ToString(),
                vector[1].ToString(),
                vector[3].ToString(),
                vector[2].ToString() });

            map.Add(new List<string> { "1",
                vector[4].ToString(),
                vector[5].ToString(),
                vector[7].ToString(),
                vector[6].ToString() });

            return map;
        }

        private List<List<string>> BuildForFourVariables(string vector, List<string> variables)
        {
            var map = new List<List<string>>
            {
                new List<string> { 
                    variables[0] + variables[1] + "\\" + variables[2] + variables[3], 
                    "00", "01", "11", "10" 
                }
            };

            map.Add(new List<string> { "00", 
                vector[0].ToString(), 
                vector[1].ToString(), 
                vector[3].ToString(), 
                vector[2].ToString() });
            
            map.Add(new List<string> { "01", 
                vector[4].ToString(), 
                vector[5].ToString(), 
                vector[7].ToString(), 
                vector[6].ToString() });
            
            map.Add(new List<string> { "11", 
                vector[12].ToString(), 
                vector[13].ToString(), 
                vector[15].ToString(), 
                vector[14].ToString() });
            
            map.Add(new List<string> { "10", 
                vector[8].ToString(), 
                vector[9].ToString(), 
                vector[11].ToString(), 
                vector[10].ToString() });

            return map;
        }

        private HashSet<string> GetVariables(Node node)
        {
            var variables = new HashSet<string>();
            if (node == null) return variables;
            if (node.Type == NodeType.Variable)
                variables.Add(node.Value);
            variables.UnionWith(GetVariables(node.Left));
            variables.UnionWith(GetVariables(node.Right));
            return variables;
        }
    }
}