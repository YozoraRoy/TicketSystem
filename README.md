## Ticket System Design

This is a Interview Question.

Please design a ticket tracking system. This system allows QA to report a bug and RD can mark a bug as resolved.
A. Phase I Requirement:
- There are two types of user: QA and RD.
- Only QA can create a bug, edit a bug and delete a bug.
- Only RD can resolve a bug.
- Summary field and Description filed are required of a bug when QA is creating a bug.

B. Phase II Requirement:
- Adding new field Severity and Priority to a ticket.
- Adding new type of user “PM” that can create new ticket type “Feature Request”. And only RD can mark it as resolved.
- Adding new ticket type “Test Case” that only QA can create and resolve. It’s read-only for other type of users.
- Adding new type of user “Administrator” user that can manage all the stuffs including adding new QA, RD and PM user.

Task 1 - Please write down all the use cases either in text or diagram you can think for Phase I and Phase II requirement separately.
Task 2 - Please implement the A. Phase I Requirement by .NET Core MVC/Java Spring MVC/PHP Laravel 8/ Python Django. For front-end, you can use any framework you like
Task 3 - Think of yourself as an architect. How will you design this system, please write down the design document at least to include data model, class diagram and UI mock up
Task 4 - If we are going to open the system for 3rd party to use, can you please design the Web API(Json format) and api document?

Note 1: You don’t have to finish all of the tasks, but please do your best to complete as much as you can.
Note 2: Requirements may not be very clear, but please do your best to finish them.
