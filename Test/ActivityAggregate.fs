module Tests

open Domain.Aggregate
open Domain.Activity
open Messages.Activities
open NodaTime
open System
open Xunit


let createLoader (events: ActivityEvent seq) =
    fun (id: Id) -> events
    
let createEventVerifier (expected: ActivityEvent) =
    fun (id: Id * int) (actual: ActivityEvent) ->
        Assert.Equal(expected, actual)

let activityAggregateTestRunner load commit command =
    let aggregateExecutor = MakeAggregateExecutor activityAggregate load commit
    aggregateExecutor (Guid.NewGuid (), 10) command


[<Fact>]
let ``CreateActivity command emits correct event`` () =
    // Given
    let events = []
    let load = createLoader events
    
    // When
    let command = CreateActivity {
        Name = "New Activity"
        CreateAt = Instant.MinValue
        Goal = Duration.FromHours 10
    }
    
    // Then
    let expectedEvent = ActivityCreated {
            Name = "New Activity"
            CreatedAt = Instant.MinValue
            Goal = Duration.FromHours 10
        }

    let commit = createEventVerifier expectedEvent    
    activityAggregateTestRunner load commit command


