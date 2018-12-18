namespace Messages.Activities

open System
open NodaTime


type CreateActivity =
    { 
        Name: string
        CreateAt: Instant 
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
    | LogTime of LogTime
    | StartTimeLogging of StartTimeLogging
    | EndTimeLogging of EndTimeLogging
    | DeleteActivity of DeleteActivity


type ActivityCreated =
    { 
        CreatedAt: Instant
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
    | TimeLogged of TimeLogged
    | StartedLoggingTime of StartedLoggingTime
    | EndedLoggingTime of EndedLoggingTime
    | ActivityDeleted of ActivityDeleted
