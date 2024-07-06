#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Daipan.Enemy.Scripts;
using Daipan.Option.Scripts;
using Daipan.Tutorial.Interfaces;
using UnityEngine;

namespace Daipan.Tutorial.Scripts
{
    public interface ISpeechEvent
    {
        int Id { get; }
        string Message { get; }
        SpeechEventEnum SpeechEventEnum { get; }
        (bool, ISpeechEvent) MoveNext();
        void SetNextEvent(params ISpeechEvent[] nextEvents);
    }

    public abstract record AbstractSpeechEvent : ISpeechEvent, IDisposable
    {
        public int Id { get; protected init; }
        public string Message { get; protected init; }
        public SpeechEventEnum SpeechEventEnum { get; protected init; }
        protected Func<bool> OnMoveAction { get; set; }
        protected readonly IList<IDisposable> Disposables = new List<IDisposable>();
        public void Dispose()
        {
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
        public abstract (bool, ISpeechEvent) MoveNext();
        public abstract void SetNextEvent(params ISpeechEvent[] nextEvents);
    }
    
    public enum SpeechEventEnum
    {
        None,
        Listening, // 聞くタイプのチュートリアル
        Practical, // 実践するタイプのチュートリアル
    }

    public sealed record SequentialEvent : AbstractSpeechEvent
    {
        ISpeechEvent? NextEvent { get; set; }

        public SequentialEvent(
            int id
            ,string message
            ,SpeechEventEnum speechEventEnum
            )
        {
            Id = id;
            Message = message;
            SpeechEventEnum = speechEventEnum;
            OnMoveAction = () => true;
        }
        
        public SequentialEvent(
            int id
            ,string message
            ,SpeechEventEnum speechEventEnum
            ,Func<bool> onMoveAction
            )
        {
            Id = id;
            Message = message;
            SpeechEventEnum = speechEventEnum;
            OnMoveAction = onMoveAction;
        }


        public override (bool, ISpeechEvent) MoveNext()
        {
            var result = OnMoveAction();
            if(!result) return  (false, this);
            if (NextEvent == null) return (false, this);
            return (true, NextEvent);
        }

        public override void SetNextEvent(params ISpeechEvent[] nextEvents)
        {
            if(nextEvents.Length != 1) throw new ArgumentException("NextEvent must be one");
            NextEvent = nextEvents[0];
        }
    }

    public sealed record ConditionalEvent : AbstractSpeechEvent
    {
        Func<bool> Condition { get; }
        ISpeechEvent? TrueEvent { get; set; }
        ISpeechEvent? FalseEvent { get; set; }
        public ConditionalEvent(
            int id
            ,string message
            ,SpeechEventEnum speechEventEnum
            ,Func<bool> onMoveAction
            ,Func<bool> condition
            )
        {
            Id = id;
            Message = message; 
            SpeechEventEnum = speechEventEnum;
            OnMoveAction = onMoveAction;
            Condition = condition;
        }
        
        public override (bool, ISpeechEvent) MoveNext()
        {
            var result = OnMoveAction();
            if(!result) return  (false, this);
            if (TrueEvent == null || FalseEvent == null) return (false, this);
            return (true, Condition() ? TrueEvent : FalseEvent);
        }
        
        public override void SetNextEvent(params ISpeechEvent[] nextEvents)
        {
            if(nextEvents.Length != 2) throw new ArgumentException("NextEvent must be two");
            TrueEvent = nextEvents[0];
            FalseEvent = nextEvents[1];
        }
            
    }
    public sealed record StartEvent : AbstractSpeechEvent
    {
        ISpeechEvent? NextEvent { get; set; }
        public StartEvent(SpeechEventEnum speechEventEnum)
        {
            SpeechEventEnum = speechEventEnum;
            OnMoveAction = () => true;
        }
        public StartEvent(SpeechEventEnum speechEventEnum, Func<bool> initialOnMoveAction)
        {
            SpeechEventEnum = speechEventEnum;
            OnMoveAction = initialOnMoveAction;
        }
        public override (bool, ISpeechEvent) MoveNext()
        {
            var result = OnMoveAction();
            if(!result) return  (false, this);
            if (NextEvent == null) return (false, this);
            return (true, NextEvent);
        }

        public override void SetNextEvent(params ISpeechEvent[] nextEvents)
        {
            if(nextEvents.Length != 1) throw new ArgumentException("NextEvent must be one");
            NextEvent = nextEvents[0];
        }
    }
    public sealed record EndEvent : AbstractSpeechEvent
    {
        public override (bool, ISpeechEvent) MoveNext()
        {
            return (false, this);
        }

        public override void SetNextEvent(params ISpeechEvent[] nextEvents)
        {
            throw new NotImplementedException();
        }
    }

    public static class SpeechEventBuilder
    {
        public static ISpeechEvent BuildUICatIntroduce()
        {
            List<ISpeechEvent> speechEvents =
                new List<ISpeechEvent>
                { 
                    new StartEvent(SpeechEventEnum.Listening),
                    new SequentialEvent(1, "やぁ、初めまして！僕はネコ！",SpeechEventEnum.Listening),
                    new SequentialEvent(2, "君の配信をサポートするよ！",SpeechEventEnum.Listening),
                    new SequentialEvent(3, "じゃあ、まずこのゲームの説明...！", SpeechEventEnum.Listening),
                    new EndEvent()
                };
            speechEvents[0].SetNextEvent(speechEvents[1]);
            speechEvents[1].SetNextEvent(speechEvents[2]);
            speechEvents[2].SetNextEvent(speechEvents[3]);
            speechEvents[3].SetNextEvent(speechEvents[4]);
            return speechEvents[0]; 
        }
        
        public static ISpeechEvent BuildRedEnemyTutorial(
            RedEnemyTutorial redEnemyTutorial
            , EnemySpawnerTutorial enemySpawnerTutorial
            )
        {
            List<ISpeechEvent> speechEvents =
                new List<ISpeechEvent>
                {
                    new StartEvent(SpeechEventEnum.Listening, ()=>
                    {
                        enemySpawnerTutorial.SpawnRedEnemy();
                        return true;
                    }),
                    new SequentialEvent(1, "赤い敵が来たね！", SpeechEventEnum.Listening),
                    new ConditionalEvent(2, "赤色のボタンを押そう！", SpeechEventEnum.Practical
                        , () =>
                        {
                             return true;
                        },() => redEnemyTutorial.IsSuccess == true),
                    new SequentialEvent(3, "そうそう！上手！", SpeechEventEnum.Listening),
                    new SequentialEvent(4, "それは違うボタンだよ！もう一回！", SpeechEventEnum.Listening
                        , () =>
                        {
                            return true;
                        }
                        ),
                    new EndEvent(),
                };
            
            speechEvents[0].SetNextEvent(speechEvents[1]);
            speechEvents[1].SetNextEvent(speechEvents[2]);
            speechEvents[2].SetNextEvent(speechEvents[3], speechEvents[4]);
            speechEvents[3].SetNextEvent(speechEvents[5]); // Success path
            speechEvents[4].SetNextEvent(speechEvents[1]); // Failure path, retry
            return speechEvents[0]; 
        }
    }
    

    public class SpeechEventManager
    {
        ISpeechEvent? _currentEvent;

        public ISpeechEvent CurrentEvent
        {
            get
            {
                if (_currentEvent == null)
                {
                    Debug.LogWarning("CurrentEvent is null");
                    return new EndEvent();
                }
                return _currentEvent;
            }
            private set => _currentEvent = value; 
        }   
        public void SetSpeechEvent(ISpeechEvent speechEvent)
        {
            CurrentEvent = speechEvent;
        }
        public SpeechEventEnum GetSpeechEventEnum()
        {
            return CurrentEvent.SpeechEventEnum;
        }
        
        public bool IsEnd()
        {
            return CurrentEvent is EndEvent;
        }

        public bool MoveNext()
        {
            var (result, nextEvent) = CurrentEvent.MoveNext(); 
            if(result) CurrentEvent = nextEvent;
            return result;
        }
    }
}

