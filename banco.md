| table_name                     | column_name            | data_type                | is_nullable | column_default |
| ------------------------------ | ---------------------- | ------------------------ | ----------- | -------------- |
| AccountMembers                 | Id                     | uuid                     | NO          | null           |
| AccountMembers                 | AccountId              | uuid                     | NO          | null           |
| AccountMembers                 | UserId                 | uuid                     | NO          | null           |
| AccountMembers                 | Role                   | text                     | NO          | null           |
| AccountMembers                 | JoinedAt               | timestamp with time zone | NO          | null           |
| Accounts                       | Id                     | uuid                     | NO          | null           |
| Accounts                       | Name                   | character varying        | NO          | null           |
| Accounts                       | Type                   | text                     | NO          | null           |
| Accounts                       | OwnerId                | uuid                     | NO          | null           |
| Accounts                       | CreatedAt              | timestamp with time zone | NO          | null           |
| Alerts                         | Id                     | uuid                     | NO          | null           |
| Alerts                         | AccountId              | uuid                     | NO          | null           |
| Alerts                         | UserId                 | uuid                     | NO          | null           |
| Alerts                         | Type                   | text                     | NO          | null           |
| Alerts                         | Threshold              | numeric                  | NO          | null           |
| Alerts                         | CategoryId             | uuid                     | YES         | null           |
| Alerts                         | IsActive               | boolean                  | NO          | null           |
| Alerts                         | CreatedAt              | timestamp with time zone | NO          | null           |
| Alerts                         | UpdatedAt              | timestamp with time zone | NO          | null           |
| BankAccounts                   | Id                     | uuid                     | NO          | null           |
| BankAccounts                   | BankConnectionId       | uuid                     | NO          | null           |
| BankAccounts                   | PluggyAccountId        | character varying        | NO          | null           |
| BankAccounts                   | Name                   | character varying        | NO          | null           |
| BankAccounts                   | Type                   | character varying        | NO          | null           |
| BankAccounts                   | Balance                | numeric                  | NO          | null           |
| BankAccounts                   | Currency               | character varying        | NO          | null           |
| BankAccounts                   | LastSyncAt             | timestamp with time zone | YES         | null           |
| BankAccounts                   | CreatedAt              | timestamp with time zone | NO          | null           |
| BankAccounts                   | UpdatedAt              | timestamp with time zone | NO          | null           |
| BankConnections                | Id                     | uuid                     | NO          | null           |
| BankConnections                | AccountId              | uuid                     | NO          | null           |
| BankConnections                | UserId                 | uuid                     | NO          | null           |
| BankConnections                | PluggyItemId           | character varying        | NO          | null           |
| BankConnections                | InstitutionName        | character varying        | NO          | null           |
| BankConnections                | Status                 | text                     | NO          | null           |
| BankConnections                | LastSyncAt             | timestamp with time zone | YES         | null           |
| BankConnections                | CreatedAt              | timestamp with time zone | NO          | null           |
| BankConnections                | UpdatedAt              | timestamp with time zone | NO          | null           |
| BankTransactions               | Id                     | uuid                     | NO          | null           |
| BankTransactions               | BankAccountId          | uuid                     | NO          | null           |
| BankTransactions               | PluggyTransactionId    | character varying        | NO          | null           |
| BankTransactions               | Description            | character varying        | NO          | null           |
| BankTransactions               | Amount                 | numeric                  | NO          | null           |
| BankTransactions               | Date                   | timestamp with time zone | NO          | null           |
| BankTransactions               | Type                   | text                     | NO          | null           |
| BankTransactions               | Category               | character varying        | YES         | null           |
| BankTransactions               | IsImported             | boolean                  | NO          | null           |
| BankTransactions               | TransactionId          | uuid                     | YES         | null           |
| BankTransactions               | CreatedAt              | timestamp with time zone | NO          | null           |
| Budgets                        | Id                     | uuid                     | NO          | null           |
| Budgets                        | AccountId              | uuid                     | NO          | null           |
| Budgets                        | CategoryId             | uuid                     | NO          | null           |
| Budgets                        | Amount                 | numeric                  | NO          | null           |
| Budgets                        | Period                 | text                     | NO          | null           |
| Budgets                        | StartDate              | timestamp with time zone | NO          | null           |
| Budgets                        | EndDate                | timestamp with time zone | NO          | null           |
| Budgets                        | IsActive               | boolean                  | NO          | null           |
| Budgets                        | CreatedAt              | timestamp with time zone | NO          | null           |
| Budgets                        | UpdatedAt              | timestamp with time zone | NO          | null           |
| Categories                     | Id                     | uuid                     | NO          | null           |
| Categories                     | AccountId              | uuid                     | NO          | null           |
| Categories                     | Name                   | character varying        | NO          | null           |
| Categories                     | Color                  | character varying        | NO          | null           |
| Categories                     | Icon                   | character varying        | NO          | null           |
| Categories                     | Type                   | text                     | NO          | null           |
| Categories                     | CreatedAt              | timestamp with time zone | NO          | null           |
| Goals                          | Id                     | uuid                     | NO          | null           |
| Goals                          | AccountId              | uuid                     | NO          | null           |
| Goals                          | UserId                 | uuid                     | NO          | null           |
| Goals                          | Name                   | character varying        | NO          | null           |
| Goals                          | Description            | text                     | YES         | null           |
| Goals                          | TargetAmount           | numeric                  | NO          | null           |
| Goals                          | CurrentAmount          | numeric                  | NO          | null           |
| Goals                          | TargetDate             | timestamp with time zone | NO          | null           |
| Goals                          | Status                 | text                     | NO          | null           |
| Goals                          | CreatedAt              | timestamp with time zone | NO          | null           |
| Goals                          | UpdatedAt              | timestamp with time zone | NO          | null           |
| Invitations                    | Id                     | uuid                     | NO          | null           |
| Invitations                    | AccountId              | uuid                     | NO          | null           |
| Invitations                    | InviterId              | uuid                     | NO          | null           |
| Invitations                    | Email                  | character varying        | NO          | null           |
| Invitations                    | Role                   | text                     | NO          | null           |
| Invitations                    | Token                  | character varying        | NO          | null           |
| Invitations                    | Status                 | text                     | NO          | null           |
| Invitations                    | ExpiresAt              | timestamp with time zone | NO          | null           |
| Invitations                    | CreatedAt              | timestamp with time zone | NO          | null           |
| Invitations                    | AcceptedAt             | timestamp with time zone | YES         | null           |
| Notifications                  | Id                     | uuid                     | NO          | null           |
| Notifications                  | UserId                 | uuid                     | NO          | null           |
| Notifications                  | Type                   | text                     | NO          | null           |
| Notifications                  | Title                  | character varying        | NO          | null           |
| Notifications                  | Message                | text                     | NO          | null           |
| Notifications                  | IsRead                 | boolean                  | NO          | null           |
| Notifications                  | CreatedAt              | timestamp with time zone | NO          | null           |
| Notifications                  | ReadAt                 | timestamp with time zone | YES         | null           |
| RecurringTransactionExecutions | Id                     | uuid                     | NO          | null           |
| RecurringTransactionExecutions | RecurringTransactionId | uuid                     | NO          | null           |
| RecurringTransactionExecutions | TransactionId          | uuid                     | YES         | null           |
| RecurringTransactionExecutions | ScheduledDate          | timestamp with time zone | NO          | null           |
| RecurringTransactionExecutions | ExecutedDate           | timestamp with time zone | YES         | null           |