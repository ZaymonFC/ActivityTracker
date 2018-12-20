namespace Messages.Activities
open System
open NodaTime

type CreateActivity =
    { 
        Name: string
        CreateAt: Instant
        Goal: Duration
    }

type UpdateActivityGoal =
    {
        Goal: Duration
        UpdateAt: Instant
    }

type UpdateActivityName =
    {
        Name: string
        UpdateAt: Instant
    }

type LogTime =
    { 
        Duration: Duration
        LogAt: Instant 
    }

type StartTimeLogging =
    {
        StartAt: Instant
    }

type EndTimeLogging =
    {
        EndAt: Instant
    }

type DeleteActivity =
    {
        DeleteAt: Instant
    }

type ActivityCommand =
    | CreateActivity of CreateActivity
    | UpdateActivityGoal of UpdateActivityGoal
    | UpdateActivityName of UpdateActivityName
    | LogTime of LogTime
    | StartTimeLogging of StartTimeLogging
    | EndTimeLogging of EndTimeLogging
    | DeleteActivity of DeleteActivity


type ActivityCreated =
    {
        Name: string
        Goal: Duration
        CreatedAt: Instant
    }
    
type ActivityGoalUpdated =
    {
        Goal: Duration
        UpdatedAt: Instant
    }

type ActivityNameUpdated =
    {
        Name: string
        UpdatedAt: Instant
    }

type TimeLogged =
    { 
        Duration: Duration 
        DateLogged: Instant
    }

type StartedLoggingTime =
    {
        StartedAt: Instant
    }

type EndedLoggingTime =
    { 
        EndedAt: Instant
    }

type ActivityDeleted =
    {
        DeletedAt: Instant
    }

type ActivityEvent =
    | ActivityCreated of ActivityCreated
    | ActivityGoalUpdated of ActivityGoalUpdated
    | ActivityNameUpdated of ActivityNameUpdated
    | TimeLogged of TimeLogged
    | StartedLoggingTime of StartedLoggingTime
    | EndedLoggingTime of EndedLoggingTime
    | ActivityDeleted of ActivityDeleted
