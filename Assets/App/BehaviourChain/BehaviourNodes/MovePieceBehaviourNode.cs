using System.Collections.Generic;
using App.Services.BoardService;
using ppl.PBehaviourChain.Core;
using ppl.PBehaviourChain.Core.Behaviours;
using ppl.Services.Core;
using UnityEngine;

namespace App.BehaviourChain.BehaviourNodes
{
    public class MovePieceBehaviourNode : ppl.PBehaviourChain.Core.Behaviours.BehaviourNode
    {

        //O DesignDoc pedia para conter apenas um campo informando a direção que o Pawn deve andar
        //Se quiser, esse comportamento pode ser substituido por PathDirection[],
        //caso queiram movimentos mais complexos.
        //Tal qual como funciona o movimento normal do pawn, ao selecionar um caminho
        [SerializeField] private PathDirection _movementLayout;
        
        private bool _isInterpolating;
        private bool _hasFailed;
        
        protected override void OnStart(Dictionary<string, object> args = null)
        {
            ServiceContainer.UseService<IBoardService>(boardService =>
            {
                if (null != args && args.TryGetValue("pawnId", out object pawnId))
                {
                    _isInterpolating = true;
                    string id = (string)pawnId;
                    Coordinate from = boardService.GetPawnCoordinate(id);
                    Path evaluatedPath = boardService.EvaluatePathFromDirections(id, from, new PathDirection[] { _movementLayout }, inclusive: false);
                    
                    //Checando se pode ou não andar pelo caminho configurado.
                    if (evaluatedPath.Coordinates.Length == 0)
                    {
                        _hasFailed = true;
                        return;
                    }
                    
                    boardService.WatchPawn(id);
                    boardService.InterpolateWatchedPawnTo(evaluatedPath, () => { _isInterpolating = false; });
                }
            });
        }

        protected override State OnUpdate()
        {
            if(_hasFailed)
                return State.Failure;
            
            return _isInterpolating ? State.Running : State.Success;
        }

        protected override void OnStop()
        {
            Debug.Log("Stop Move Piece Behaviour for " + name);
        }

        public override void ResetState()
        {
            base.ResetState();
            _isInterpolating = false;
            _hasFailed = false;
        }
        
        public override Node Instantiate()
        {
            MovePieceBehaviourNode node = ScriptableObject.CreateInstance<MovePieceBehaviourNode>();
            node.GUID = GUID;
            node.Started = Started;
            node.Child = Child;
            node.name = name;
            node.NodeState = node.NodeState;
            node.Child = Child?.Instantiate() as BehaviourNode;
            node._movementLayout = _movementLayout;
            return node;
        }
    }
}