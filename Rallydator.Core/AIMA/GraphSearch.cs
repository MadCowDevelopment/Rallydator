using System.Collections.Generic;
using System.Linq;
using Rallydator.Core.Validation;

namespace Rallydator.Core.AIMA
{
    /// <summary>
    /// function GRAPH-SEARCH(problem) returns a solution, or failure
    ///   initialize the frontier using the initial state of problem
    ///   initialize the explored set to be empty
    ///   loop do
    ///     if the frontier is empty then return failure
    ///     choose a leaf node and remove it from the frontier
    ///    if the node contains a goal state then return the corresponding solution
    ///     add the node to the explored set
    ///     expand the chosen node, adding the resulting nodes to the frontier
    ///       only if not in the frontier or explored set
    /// </summary>
    public class GraphSearch<TAction> : IGraphSearch<RallyState, TAction, ValidationResult>
    {
        public ValidationResult Search(Problem<RallyState, TAction> problem)
        {
            var result = new ValidationResult();
            Queue<Node<RallyState, TAction>> frontier = new Queue<Node<RallyState, TAction>>();
            HashSet<Node<RallyState, TAction>> explored = new HashSet<Node<RallyState, TAction>>();

            var root = new Node<RallyState, TAction>(problem.InitalState);
            frontier.Enqueue(root);

            for (; ; )
            {
                if (frontier.Count == 0)
                {
                    result.AddError("No solution found.");
                    break;
                }

                var leaf = frontier.Dequeue();
                if (problem.GoalTest.IsGoal(leaf.State))
                {
                    result.Damage = leaf.State.Damage;
                    break;
                }

                explored.Add(leaf);

                var children = ExpandNode(leaf, problem);
                foreach (var child in children)
                {
                    Node<RallyState, TAction> child1 = child;
                    if (!frontier.Any(p => p.State.Equals(child1.State)) &&
                        !explored.Any(p => p.State.Equals(child1.State)))
                    {
                        frontier.Enqueue(child);
                    }
                }
            }

            return result;
        }

        public List<Node<RallyState, TAction>> ExpandNode(Node<RallyState, TAction> node, Problem<RallyState, TAction> problem)
        {
            var childNodes = new List<Node<RallyState, TAction>>();

            var actions = problem.ActionFunction.Actions(node.State);
            foreach (var action in actions)
            {
                var state = problem.ResultFunction.Result(node.State, action);
                var cost = problem.StepCost.Cost(node.State, action);
                var childNode = new Node<RallyState, TAction>(state, node, action, cost);
                childNodes.Add(childNode);
            }

            return childNodes;
        }
    }
}
