module Domain.Activity

open Messages.Activities
open Messages.Users
open NodaTime
open NodaTime
open NodaTime
open System

type State = {
    Name: string
    Goal: Duration
    TotalTime: int option
    CreatedAt: Instant
    Deleted: bool
    DeletedAt: Instant option
    UpdatedAt: Instant option
    CurrentlyLoggingTime: bool
    StartedLoggingAt: Instant option
}

// Create Activity
let execActivityCreate (state: State) (command: CreateActivity) =
    raise <| NotImplementedException ()
    
let applyActivityCreated (state: State) (event: ActivityCreated) =
    {
        Name = event.Name
        Goal = event.Goal
        TotalTime = Some 0
        CreatedAt = event.CreatedAt
        Deleted = false
        DeletedAt = None
        UpdatedAt = None
        CurrentlyLoggingTime = false
        StartedLoggingAt = None
    }

// Update Goal
let execUpdateGoal (state: State) (command: UpdateActivityGoal) =
    raise <| NotImplementedException ()
    
let applyActivityGoalUpdate (state: State) (event: ActivityGoalUpdated) =
    { state with
        Goal = event.Goal
    }

// Update Name
let execUpdateName (state: State) (command: UpdateActivityName) =
    raise <| NotImplementedException ()
    
let applyActivityNameUpdate (state: State) (event: ActivityNameUpdated) =
    { state with
        Name = event.Name
        UpdatedAt = Some event.UpdatedAt
    }

// Log Time
let execLogTime (state: State) (command: LogTime) =
    raise <| NotImplementedException ()

let applyTimeLogged (state: State) (event: TimeLogged) =
    match state.TotalTime with
    | None -> { state with TotalTime = Some event.Duration.Seconds }
    | Some t -> { state with TotalTime = Some (t + event.Duration.Seconds)}

// Start Time Logging
let execStartTimeLogging (state: State) (command: StartTimeLogging) =
    raise <| NotImplementedException ()
    
let applyStartedLoggingTime (state: State) (event: StartedLoggingTime) =
    raise <| NotImplementedException ()

// End Time Logging
let execEndTimeLogging (state: State) (command: EndTimeLogging) =
    raise <| NotImplementedException ()
    
let applyEndedLoggingTime (state: State) (event: EndedLoggingTime) =
    raise <| NotImplementedException ()

// Delete Activity
let execActivityDelete (state: State) (command: DeleteActivity) =
    raise <| NotImplementedException ()

let applyActivityDeleted (state: State) (event: ActivityDeleted) =
    raise <| NotImplementedException ()


let exec (state: State) (command: ActivityCommand) =
    match command with
    | CreateActivity c -> execActivityCreate state c
    | UpdateActivityGoal c -> execUpdateGoal state c
    | UpdateActivityName c -> execUpdateName state c
    | LogTime c -> execLogTime state c
    | StartTimeLogging c -> execStartTimeLogging state c
    | EndTimeLogging c -> execEndTimeLogging state c
    | DeleteActivity c -> execActivityDelete state c

let apply (state: State) (event: ActivityEvent) =
    match event with
    | ActivityCreated e -> applyActivityCreated state e
    | ActivityGoalUpdated e -> applyActivityGoalUpdate state e
    | ActivityNameUpdated e -> applyActivityNameUpdate state e
    | TimeLogged e -> applyTimeLogged state e
    | StartedLoggingTime e -> applyStartedLoggingTime state e
    | EndedLoggingTime e -> applyEndedLoggingTime state e
    | ActivityDeleted e -> applyActivityDeleted state e


