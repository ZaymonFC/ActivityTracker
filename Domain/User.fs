module Domain.User
open Domain.Aggregate
open System
open Messages.Users
open NodaTime

type UserState = {
    FirstName: string
    LastName: string
    Username: string
    Email: string
    CreatedAt: Instant
    Deleted: bool
}

let execCreateUser (command: CreateUser) =
    UserCreated {
        FirstName = command.FirstName
        LastName = command.LastName
        Username = command.Username
        Email = command.Email
        CreatedAt = command.CreateAt
    }

let applyUserCreated (event: UserCreated) =
    {
        FirstName = event.FirstName
        LastName = event.LastName
        Username = event.Username
        Email = event.Email
        CreatedAt = event.CreatedAt
        Deleted = false
    }

let execUpdateName (state: UserState) (command: UpdateName) =
    // TODO Invariate Enforcement ᕙ╏✖۝✖╏⊃-(===>
    NameUpdated {
        FirstName = command.FirstName
        LastName = command.LastName
        UpdatedAt = command.UpdateAt
    }

let applyNameUpdated (state: UserState) (event: NameUpdated) =
    { state with
        FirstName = event.FirstName
        LastName = event.LastName
    }

let execUpdateUsername (state: UserState) (command: UpdateUsername) =
    UsernameUpdated {
        Username = command.Username
        UpdatedAt = command.UpdateAt
    }

let applyUsernameUpdate (state: UserState) (event: UsernameUpdated) =
    { state with
        Username = event.Username
    }

let execUpdateEmail (state: UserState) (command: UpdateEmail) =
    EmailUpdated {
        Email = command.Email
        UpdatedAt = command.UpdateAt
    }

let applyEmailUpdated (state: UserState) (event: EmailUpdated) =
    { state with
        Email = event.Email
    }

let execDeleteUser (state: UserState) (command: DeleteUser) =
    UserDeleted {
        DeletedAt = command.DeleteAt
    }

let applyUserDeleted (state: UserState) (event: UserDeleted) =
    { state with
        Deleted = true
    }


let exec (state: UserState option) (command: UserCommand) =
    match state with
    | None ->
        match command with
        | CreateUser c -> execCreateUser c
        | _ -> ErrorInvalidCommand {
                Details = sprintf "Attempted to fire %A with non-existent activity state" command
            }
    | Some state ->
        match command with
        | UpdateName c -> execUpdateName state c
        | UpdateUsername c -> execUpdateUsername state c
        | UpdateEmail c -> execUpdateEmail state c
        | DeleteUser c -> execDeleteUser state c
        | _ -> ErrorInvalidCommand {
            Details = sprintf "The command %A is not applicable in this context" command
        }

let apply (state: UserState option) (event: UserEvent) =
    match state with
    | None ->
       match event with
       | UserCreated e -> applyUserCreated e |> Some
       | _ -> None
    | Some state ->
        match event with
        | NameUpdated e -> applyNameUpdated state e
        | UsernameUpdated e -> applyUsernameUpdate state e
        | EmailUpdated e -> applyEmailUpdated state e
        | UserDeleted e -> applyUserDeleted state e
        | _ -> state
        |> Some

let userAggregate: Aggregate<_, _, _> = {
    Zero = None
    Apply = apply
    Exec = exec
}
