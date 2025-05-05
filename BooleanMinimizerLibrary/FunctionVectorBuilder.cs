using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BooleanMinimizerLibrary
{
    public class FunctionVectorBuilder
    {
        public string BuildVector(Node root)
        {
            if (root.Type == NodeType.Vector)
            {
                int n = (int)Math.Log(root.Value.Length, 2);
                var variables = new List<string>();
                for (int i = 0; i < n; i++)
                {
                    // Используем порядок w, x, y, z
                    variables.Add(i switch
                    {
                        0 => "w",
                        1 => "x",
                        2 => "y",
                        3 => "z",
                        _ => throw new Exception("Максимум 4 переменные")
                    });
                }
                root.Variables = variables;
                return root.Value;
            }

            var variablesList = GetVariables(root).OrderBy(v => v).ToList();
            int vectorLength = 1 << variablesList.Count;
            var resultVector = new char[vectorLength];

            for (int mask = 0; mask < vectorLength; mask++)
            {
                var variableValues = new Dictionary<string, bool>();
                for (int i = 0; i < variablesList.Count; i++)
                {
                    variableValues[variablesList[i]] = ((mask >> (variablesList.Count - i - 1)) & 1) == 1;
                }

                bool value = Evaluate(root, variableValues);
                resultVector[mask] = value ? '1' : '0';
            }

            return new string(resultVector);
        }

        public List<Dictionary<string, bool>> BuildTruthTable(Node root)
        {
            List<string> variables;

            if (root.Type == NodeType.Vector)
            {
                variables = root.Variables; // Используем сохраненные переменные
            }
            else
            {
                variables = GetVariables(root).OrderBy(v => v).ToList();
            }

            int n = variables.Count;
            int vectorLength = 1 << n;
            var truthTable = new List<Dictionary<string, bool>>();

            for (int mask = 0; mask < vectorLength; mask++)
            {
                var variableValues = new Dictionary<string, bool>();
                for (int i = 0; i < n; i++)
                {
                    variableValues[variables[i]] = ((mask >> (n - i - 1)) & 1) == 1;
                }

                bool value;
                if (root.Type == NodeType.Vector)
                {
                    // Вычисляем индекс для вектора
                    int index = 0;
                    for (int i = 0; i < variables.Count; i++)
                    {
                        if (variableValues[variables[i]])
                            index |= 1 << (variables.Count - i - 1);
                    }
                    value = root.Value[index] == '1';
                }
                else
                {
                    value = Evaluate(root, variableValues);
                }

                variableValues["Result"] = value;
                truthTable.Add(new Dictionary<string, bool>(variableValues));
            }

            foreach (var row in truthTable)
            {
                // Заменяем "Result" на имя функции
                row["F"] = row["Result"];
                row.Remove("Result");
            }

            return truthTable;

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

        private bool EvaluateVector(Node node, Dictionary<string, bool> variables)
        {
            // Проверяем, что переменные существуют
            if (node.Variables == null || node.Variables.Count == 0)
                throw new Exception("Переменные для вектора не определены");

            int index = 0;
            for (int i = 0; i < node.Variables.Count; i++)
            {
                if (variables[node.Variables[i]])
                    index |= 1 << (node.Variables.Count - 1 - i);
            }

            // Проверяем, что индекс находится в диапазоне длины вектора
            if (index >= node.Value.Length)
                throw new Exception("Индекс выходит за границы вектора");

            return node.Value[index] == '1';
        }

        private bool Evaluate(Node node, Dictionary<string, bool> variables)
        {
            if (node == null)
                throw new Exception("Пустой узел при вычислении");

            return node.Type switch
            {
                NodeType.Variable => variables[node.Value],
                NodeType.Constant => node.Value == "1",
                NodeType.Not => !Evaluate(node.Right, variables),
                NodeType.And => Evaluate(node.Left, variables) && Evaluate(node.Right, variables),
                NodeType.Or => Evaluate(node.Left, variables) || Evaluate(node.Right, variables),
                NodeType.Xor => Evaluate(node.Left, variables) ^ Evaluate(node.Right, variables),
                NodeType.Implies => !Evaluate(node.Left, variables) || Evaluate(node.Right, variables),
                NodeType.Equivalent => Evaluate(node.Left, variables) == Evaluate(node.Right, variables),
                NodeType.Vector => EvaluateVector(node, variables),
                _ => throw new Exception("Неизвестный тип узла")
            };
        }
    }
}
