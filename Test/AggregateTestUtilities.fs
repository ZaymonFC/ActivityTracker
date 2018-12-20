module Test.AggregateTestUtilities

open Domain.Aggregate

let tee f v =
    f v |> ignore; v

let hydrate (a: Aggregate<_, _, _>) events =
    events |> Seq.fold a.Apply a.Zero
    
let run (a: Aggregate<_, _, _>) (state: 'TState) (command: 'TCommand) =
    a.Exec state command |> tee (a.Apply state)
