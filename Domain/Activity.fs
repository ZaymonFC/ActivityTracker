module Domain.Activity

open System
open System
open Library.OptionExtensions
open Messages.Activities
open NodaTime
open Domain.Aggregate

type ActivityState = {
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
let execActivityCreate (command: CreateActivity) =
    ActivityCreated {
         Name = command.Name
         Goal = command.Goal
         CreatedAt = command.CreateAt
    }
    
let applyActivityCreated (event: ActivityCreated) =
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
let execUpdateGoal (state: ActivityState) (command: UpdateActivityGoal) =
    ActivityGoalUpdated {
        Goal = command.Goal
        UpdatedAt = command.UpdateAt
    }
    
let applyActivityGoalUpdate (state: ActivityState) (event: ActivityGoalUpdated) =
    { state with
        Goal = event.Goal
    }

// Update Name
let execUpdateName (state: ActivityState) (command: UpdateActivityName) =
    ActivityNameUpdated {
        Name = command.Name
        UpdatedAt = command.UpdateAt
    }
    
let applyActivityNameUpdate (state: ActivityState) (event: ActivityNameUpdated) =
    { state with
        Name = event.Name
        UpdatedAt = Some event.UpdatedAt
    }

// Log Time
let execLogTime (state: ActivityState) (command: LogTime) =
    TimeLogged {
        Duration = command.Duration
        DateLogged = command.LogAt
    }

let applyTimeLogged (state: ActivityState) (event: TimeLogged) =
    match state.TotalTime with
    | None -> { state with TotalTime = Some event.Duration.Seconds }
    | Some t -> { state with TotalTime = Some (t + event.Duration.Seconds)}

// Start Time Logging
let execStartTimeLogging (state: ActivityState) (command: StartTimeLogging) =
    StartedLoggingTime {
        StartedAt = command.StartAt
    }
    
let applyStartedLoggingTime (state: ActivityState) (event: StartedLoggingTime) =
    { state with
        StartedLoggingAt = Some event.StartedAt
    }

// End Time Logging
let execEndTimeLogging (state: ActivityState) (command: EndTimeLogging) =
    match state.StartedLoggingAt with
    | None -> DomainRuleViolated {
            Details = "Cannot finalize time logging without a corresponding start logging event"
        }
    | Some s ->
        if command.EndAt.ToUnixTimeMilliseconds() < s.ToUnixTimeMilliseconds() then
            DomainRuleViolated {
                Details = sprintf "Time logging cannot finish at a time before logging started. Start time: %A %A End Time: %A"
                            s Environment.NewLine command.EndAt
            }
        else
            EndedLoggingTime {
                EndedAt = command.EndAt
            }

let applyEndedLoggingTime (state: ActivityState) (event: EndedLoggingTime) =
    let newTotal = optional {
        let! s = state.StartedLoggingAt
        let! t = state.TotalTime
        return t + (event.EndedAt - s).Seconds
    }
    { state with TotalTime = newTotal }
    
// Delete Activity
let execActivityDelete (state: ActivityState) (command: DeleteActivity) =
    ActivityDeleted {
        DeletedAt = command.DeleteAt
    }

let applyActivityDeleted (state: ActivityState) (event: ActivityDeleted) =
    { state with
        Deleted = true
        DeletedAt = Some event.DeletedAt
    }

let execErrorActivityDeleted (command: ActivityCommand) =
    ErrorInvalidCommand {
        Details = sprintf "Reason: %ACannot execute command on deleted activity. %ACommand: %A"
                      Environment.NewLine Environment.NewLine command
    }


let exec (state: ActivityState option) (command: ActivityCommand) =
    match state with
    | None ->
        match command with
        | CreateActivity c -> execActivityCreate c
        | _ -> ErrorInvalidCommand {
                Details = sprintf "Attempted to fire %A with non-existent activity state" command
            }
    | Some state ->
        match state.Deleted with
        | true -> execErrorActivityDeleted command
        | _ ->
            match command with
            | UpdateActivityGoal c -> execUpdateGoal state c
            | UpdateActivityName c -> execUpdateName state c
            | LogTime c -> execLogTime state c
            | StartTimeLogging c -> execStartTimeLogging state c
            | EndTimeLogging c -> execEndTimeLogging state c
            | DeleteActivity c -> execActivityDelete state c
            | _ -> ErrorInvalidCommand {
                    Details = sprintf "The command %A is not applicable in this context" command
                } 

let apply (state: ActivityState option) (event: ActivityEvent) =
    match state with
    | None ->
        match event with
        | ActivityCreated e -> applyActivityCreated e |> Some
        | _ -> None
    | Some state ->
        match event with
        | ActivityGoalUpdated e -> applyActivityGoalUpdate state e
        | ActivityNameUpdated e -> applyActivityNameUpdate state e
        | TimeLogged e -> applyTimeLogged state e
        | StartedLoggingTime e -> applyStartedLoggingTime state e
        | EndedLoggingTime e -> applyEndedLoggingTime state e
        | ActivityDeleted e -> applyActivityDeleted state e
        | _ -> state
        |> Some

let activityAggregate: Aggregate<_, _, _> = {
    Zero = None
    Apply = apply
    Exec = exec
}