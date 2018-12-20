module Tests

open Messages.Activities
open NodaTime
open Xunit
open Test.AggregateTestUtilities
open Domain.Activity


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


