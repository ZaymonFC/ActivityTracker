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
    { state with
        StartedLoggingAt = Some event.StartedAt
    }

// End Time Logging
let execEndTimeLogging (state: State) (command: EndTimeLogging) =
    raise <| NotImplementedException ()
    
let applyEndedLoggingTime (state: State) (event: EndedLoggingTime) =
    match state.TotalTime, state.StartedLoggingAt with
    | Some t, Some s -> { state with TotalTime = Some (t + (event.EndedAt - s).Seconds) }
    | None, Some s -> { state with TotalTime = Some (event.EndedAt - s).Seconds }
    | _ -> state

// Delete Activity
let execActivityDelete (state: State) (command: DeleteActivity) =
    raise <| NotImplementedException ()

let applyActivityDeleted (state: State) (event: ActivityDeleted) =
    { state with
        Deleted = true
        DeletedAt = Some event.DeletedAt
    }


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


