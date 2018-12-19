module Domain.Aggregate
open System

type Aggregate<'TState, 'TCommand, 'TEvent> =
    {
        Zero: 'TState
        Apply: 'TState -> 'TEvent -> 'TState
        Exec: 'TState -> 'TCommand -> 'TEvent
    }

type Id = Guid

let MakeAggregateRunner (aggregate: Aggregate<'TState, 'TCommand, 'TEvent>) (load: Id -> 'TEvent seq) (commit: Id * int -> 'TEvent -> unit) =
    fun (id, version) command ->
        let state = load id |> Seq.fold aggregate.Apply aggregate.Zero
        // Todo Write handling for idempotency (Map all command id's from meta data into seq and then check command ID)
        let event = aggregate.Exec state command
        event |> commit (id, version)


