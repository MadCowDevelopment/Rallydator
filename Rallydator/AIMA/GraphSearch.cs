﻿using System.Collections.Generic;
using System.Linq;
using Rallydator.Validation;

namespace Rallydator.AIMA
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
    public class GraphSearch<TState, TAction> : IGraphSearch<TState, TAction, ValidationResult>
    {
        public ValidationResult Search(Problem<TState, TAction> problem)
        {
            var result = new ValidationResult();
            Queue<Node<TState, TAction>> frontier = new Queue<Node<TState, TAction>>();
            HashSet<Node<TState, TAction>> explored = new HashSet<Node<TState, TAction>>();

            var root = new Node<TState, TAction>(problem.InitalState);
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
                    break;
                }

                explored.Add(leaf);

                var children = ExpandNode(leaf, problem);
                foreach (var child in children)
                {
                    Node<TState, TAction> child1 = child;
                    if (!frontier.Any(p => p.State.Equals(child1.State)) &&
                        !explored.Any(p => p.State.Equals(child1.State)))
                    {
                        frontier.Enqueue(child);
                    }
                }
            }

            return result;
        }

        public List<Node<TState, TAction>> ExpandNode(Node<TState, TAction> node, Problem<TState, TAction> problem)
        {
            var childNodes = new List<Node<TState, TAction>>();

            var actions = problem.ActionFunction.Actions(node.State);
            foreach (var action in actions)
            {
                var state = problem.ResultFunction.Result(node.State, action);
                var cost = problem.StepCost.Cost(node.State, action);
                var childNode = new Node<TState, TAction>(state, node, action, cost);
                childNodes.Add(childNode);
            }

            return childNodes;
        }
    }
}
