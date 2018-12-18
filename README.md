# Activity Tracker Prototype
This is an attempt to learn to develop a functional paradigm application using event sourcing and CQRS.

# Modelling the domain

### Find the aggregates
- **Activity**
- **User**

### Find the behaviours
**User** *creates* **Activities**

- Weekly time goal
- Activity name

**User** *logs time against an* **Activity**

- Log distinct time amount *retroactively*
- Start and stop event for logging time

**User** *removes* an **Activity**
**User** *views activity progress* towards **Activity** time goal
**User** *updates* **Activity**
    
- Change goal or change name
**User** *updates* **User**

### Create the contracts
**Turn behaviours into commands and events (code)**

### Determine the business rules
