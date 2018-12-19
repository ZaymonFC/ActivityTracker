module Tests

open Domain.Aggregate
open Domain.Activity
open Messages.Activities
open NodaTime
open NodaTime
open System
open System
open System.Diagnostics
open Xunit


let createLoader (events: ActivityEvent seq) =
    fun (id: Id) -> events
    
let createEventVerifier (expected: ActivityEvent) =
    fun (id: Id * int) (actual: ActivityEvent) ->
        Assert.Equal(expected, actual)

[<Fact>]
let ``CreateActivity command emits correct event`` () =
    let events = []
    let load = createLoader events
    
    let command = CreateActivity {
        Name = "New Activity"
        CreateAt = Instant.MinValue
        Goal = Duration.FromHours 10
    }
    
    let expectedEvent = ActivityCreated {
            Name = "New Activity"
            CreatedAt = Instant.MinValue
            Goal = Duration.FromHours 10
        }
    
    let commit = createEventVerifier expectedEvent
    
    let aggregateRunner = MakeAggregateExecutor activityAggregate load commit
    aggregateRunner (Guid.NewGuid (), 10) command