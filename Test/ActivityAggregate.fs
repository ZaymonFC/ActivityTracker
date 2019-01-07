module Tests

open Messages.Activities
open NodaTime
open Xunit
open Test.AggregateTestUtilities
open Domain.Activity

let generateBaseEvents =
    let create = ActivityCreated {
        Name = "Activity"
        Goal = Duration.MaxValue
        CreatedAt = Instant.MinValue
    }
    [create]

[<Fact>]
let ``CreateActivity command emits correct event`` () =
    // Given
    let events = []
    let state = hydrate activityAggregate events
    
    // When
    let command = CreateActivity {
        Name = "New Activity"
        CreateAt = Instant.MinValue
        Goal = Duration.FromHours 10
    }
    
    let event = run activityAggregate state command
    
    // Then
    let expectedEvent = ActivityCreated {
            Name = "New Activity"
            CreatedAt = Instant.MinValue
            Goal = Duration.FromHours 10
        }

    Assert.Equal(expectedEvent, event)


[<Fact>]
let ``UpdateGoal command emits correct event`` () =
    // Given
    let events = generateBaseEvents
    let state = hydrate activityAggregate events
    
    // When
    let command = UpdateActivityGoal {
        Goal = Duration.Zero
        UpdateAt = Instant.MaxValue
    }
    
    let event = run activityAggregate state command
    
    // Then
    let expectedEvent = ActivityGoalUpdated {
        Goal = Duration.Zero
        UpdatedAt = Instant.MaxValue
    }
    
    Assert.Equal(expectedEvent, event)
    
[<Fact>]
let ``UpdateName command emits correct event`` () =
    // Given
    let events = generateBaseEvents
    let state = hydrate activityAggregate events
    
    // When
    let command = UpdateActivityName {
        Name = "Something Else"
        UpdateAt = Instant.MaxValue
    }
    
    let event = run activityAggregate state command
    
    // Then
    let expectedEvent = ActivityNameUpdated {
        Name = "Something Else"
        UpdatedAt = Instant.MaxValue
    }
    
    Assert.Equal(expectedEvent, event)
    
    
    
[<Fact>]
let ``ErrorInvalidCommand when trying to issue a command and state has not been initialised`` () =
    // Given
    let events = []
    let state = hydrate activityAggregate events
    
    Assert.Equal(None, state)
    
    // When
    let command = UpdateActivityName {
        Name = "Something Else"
        UpdateAt = Instant.MaxValue
    }
    let event = run activityAggregate state command
    
    // Then 
    match event with
    | ErrorInvalidCommand _ -> ()
    | _ -> failwithf "Event was not the correct type. Actual Event: %A" event



[<Fact>]
let ``ErrorInvalidCommand when trying to issue a command to a deleted activity`` () =
    // Given
    let events = generateBaseEvents
    let deletedEvent = ActivityDeleted {
        DeletedAt = Instant.MaxValue
    }
    let events = events @ [deletedEvent]
    
    let state = hydrate activityAggregate events
    
    // When
    let command = UpdateActivityGoal {
        Goal = Duration.MaxValue
        UpdateAt = Instant.MaxValue
    }
    let event = run activityAggregate state command
    
    // Then
    match event with
    | ErrorInvalidCommand _ -> ()
    | _ -> failwithf "Event was not correct type. Actual Event: %A" event
    
    
[<Fact>]
let ``Domain Rule Violated when trying to stop time logging without a start event`` () =
    // Given
    let events = generateBaseEvents
    let state = hydrate activityAggregate events
    
    // When
    let command = EndTimeLogging {
        EndAt = Instant.MaxValue
    }
    let event = run activityAggregate state command
    
    // Then
    match event with
    | DomainRuleViolated e ->
        Assert.Contains("Cannot finalize", e.Details)
    | _ -> failwithf "Event was not correct type. Actual Event: %A" event


[<Fact>]
let ``Domain Rule Violated when end time logging is before`` () =
    // Given
    let baseEvents = generateBaseEvents
    
    let events = baseEvents @ [StartedLoggingTime {
        StartedAt = Instant.FromUnixTimeSeconds 10000L
    }]
    
    let state = hydrate activityAggregate events
    
    // When
    let command = EndTimeLogging {
        EndAt = Instant.FromUnixTimeSeconds 100L
    }
    let event = run activityAggregate state command
    
    // Then
    match event with
    | DomainRuleViolated e ->
        Assert.Contains("before logging started", e.Details)
    | _ -> failwithf "Event was not correct type. Actual Event: %A" event

[<Fact>] 
let ``End time logging results in correct event with precursor start time logging event`` () = 
    // Given
    let baseEvents = generateBaseEvents

    let events = baseEvents @[StartedLoggingTime { 
        StartedAt = Instant.FromUnixTimeSeconds 0L
    }]
    let state = hydrate activityAggregate events

    // When
    let command = EndTimeLogging {
        EndAt = Instant.FromUnixTimeSeconds 1000L
    }
    let event = run activityAggregate state command

    // Then
    match event with
    | EndedLoggingTime _ -> ()
    | _ -> failwithf "Event was not correct type. Actual Event: %A" event
