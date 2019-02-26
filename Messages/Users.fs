namespace Messages.Users
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
        UpdateAt: Instant
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

type ErrorInvalidCommand =
    {
        Details: string
    }

type DomainRuleViolated =
    {
        Details: string
    }

type UserEvent =
    | UserCreated of UserCreated
    | NameUpdated of NameUpdated
    | UsernameUpdated of UsernameUpdated
    | EmailUpdated of EmailUpdated
    | UserDeleted of UserDeleted
    | ErrorInvalidCommand of ErrorInvalidCommand
    | DomainRuleViolated of DomainRuleViolated
