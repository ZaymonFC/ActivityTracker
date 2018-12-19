module Domain.User
open System
open Messages.Users
open NodaTime

type State = {
    FirstName: string
    LastName: string
    Username: string
    Email: string
    CreatedAt: Instant
}

let execCreateUser (state: State) (command: CreateUser) =
    raise <| NotImplementedException ()

let applyUserCreated (state: State) (command: UserCreated) =
    raise <| NotImplementedException ()

let execUpdateName (state: State) (command: UpdateName) =
    raise <| NotImplementedException ()
   
let applyNameUpdated (state: State) (command: NameUpdated) =
    raise <| NotImplementedException ()
    
let execUpdateUsername (state: State) (command: UpdateUsername) =
    raise <| NotImplementedException ()

let applyUsernameUpdate (state: State) (command: UsernameUpdated) =
    raise <| NotImplementedException ()

let execUpdateEmail (state: State) (command: UpdateEmail) =
    raise <| NotImplementedException ()

let applyEmailUpdated (state: State) (command: EmailUpdated) =
    raise <| NotImplementedException ()

let execDeleteUser (state: State) (command: DeleteUser) =
    raise <| NotImplementedException ()

let applyUserDelete (state: State) (command: UserDeleted) =
    raise <| NotImplementedException ()


let apply (state: State) (event: UserEvent) =
    match event with
    | UserCreated e -> applyUserCreated state e
    | NameUpdated e -> applyNameUpdated state e
    | UsernameUpdated e -> applyUsernameUpdate state e
    | EmailUpdated e -> applyEmailUpdated state e
    | UserDeleted e -> applyUserDelete state e


let exec (state: State) (command: UserCommand) =
    match command with
    | CreateUser c -> execCreateUser state c
    | UpdateName c -> execUpdateName state c
    | UpdateUsername c -> execUpdateUsername state c
    | UpdateEmail c -> execUpdateEmail state c
    | DeleteUser c -> execDeleteUser state c
