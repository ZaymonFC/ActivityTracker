module Messages.Users
open NodaTime

type CreateUser =
    {
        FirstName: string
        LastName: string
        Username: string
        Email: string
        CreateAt: Instant
    }

type UpdateName =
    {
        FirstName: string
        LastName: string
        UpdateAt: Instant
    }

type UpdateUsername =
    {
        Username: string
        UpdateAt: string
    }
    
type UpdateEmail =
    {
        Email: string
        UpdateAt: Instant
    }

type DeleteUser =
    {
        DeleteAt: Instant
    }

type UserCommand =
    | CreateUser of CreateUser
    | UpdateName of UpdateName
    | UpdateUsername of UpdateUsername
    | UpdateEmail of UpdateEmail
    | DeleteUser of DeleteUser


type UserCreated =
    {
        FirstName: string
        LastName: string
        Username: string
        Email: string
        CreatedAt: Instant
    }

type NameUpdated =
    {
        FirstName: string
        LastName: string
        UpdatedAt: Instant
    }
    
type UsernameUpdated =
    {
        Username: string
        UpdatedAt: Instant
    }

type EmailUpdated =
    {
        Email: string
        UpdatedAt: Instant
    }

type UserDeleted =
    {
        DeletedAt: Instant
    }

type UserEvent =
    | UserCreated of UserCreated
    | NameUpdated of NameUpdated
    | UsernameUpdate of UsernameUpdated
    | EmailUpdated of EmailUpdated
    | UserDelete of UserDeleted