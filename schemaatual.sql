create table public."AccountMembers" (
  "Id" uuid not null,
  "AccountId" uuid not null,
  "UserId" uuid not null,
  "Role" text not null,
  "JoinedAt" timestamp with time zone not null,
  constraint PK_AccountMembers primary key ("Id"),
  constraint FK_AccountMembers_Accounts_AccountId foreign KEY ("AccountId") references "Accounts" ("Id") on delete CASCADE,
  constraint FK_AccountMembers_Users_UserId foreign KEY ("UserId") references "Users" ("Id") on delete CASCADE
) TABLESPACE pg_default;

create unique INDEX IF not exists "IX_AccountMembers_AccountId_UserId" on public."AccountMembers" using btree ("AccountId", "UserId") TABLESPACE pg_default;

create index IF not exists "IX_AccountMembers_UserId" on public."AccountMembers" using btree ("UserId") TABLESPACE pg_default;


create table public."Accounts" (
  "Id" uuid not null,
  "Name" character varying(255) not null,
  "Type" text not null,
  "OwnerId" uuid not null,
  "CreatedAt" timestamp with time zone not null,
  constraint PK_Accounts primary key ("Id"),
  constraint FK_Accounts_Users_OwnerId foreign KEY ("OwnerId") references "Users" ("Id") on delete RESTRICT
) TABLESPACE pg_default;

create index IF not exists "IX_Accounts_OwnerId" on public."Accounts" using btree ("OwnerId") TABLESPACE pg_default;

create table public."Categories" (
  "Id" uuid not null,
  "AccountId" uuid not null,
  "Name" character varying(100) not null,
  "Color" character varying(7) not null,
  "Icon" character varying(50) not null,
  "Type" text not null,
  "CreatedAt" timestamp with time zone not null,
  constraint PK_Categories primary key ("Id"),
  constraint FK_Categories_Accounts_AccountId foreign KEY ("AccountId") references "Accounts" ("Id") on delete CASCADE
) TABLESPACE pg_default;

create index IF not exists "IX_Categories_AccountId" on public."Categories" using btree ("AccountId") TABLESPACE pg_default;

create table public."Transactions" (
  "Id" uuid not null,
  "AccountId" uuid not null,
  "UserId" uuid not null,
  "CategoryId" uuid not null,
  "Amount" numeric(18, 2) not null,
  "Description" character varying(500) not null,
  "Date" timestamp with time zone not null,
  "Type" text not null,
  "CreatedAt" timestamp with time zone not null,
  "UpdatedAt" timestamp with time zone not null,
  constraint PK_Transactions primary key ("Id"),
  constraint FK_Transactions_Accounts_AccountId foreign KEY ("AccountId") references "Accounts" ("Id") on delete CASCADE,
  constraint FK_Transactions_Categories_CategoryId foreign KEY ("CategoryId") references "Categories" ("Id") on delete RESTRICT,
  constraint FK_Transactions_Users_UserId foreign KEY ("UserId") references "Users" ("Id") on delete RESTRICT
) TABLESPACE pg_default;

create index IF not exists "IX_Transactions_AccountId" on public."Transactions" using btree ("AccountId") TABLESPACE pg_default;

create index IF not exists "IX_Transactions_CategoryId" on public."Transactions" using btree ("CategoryId") TABLESPACE pg_default;

create index IF not exists "IX_Transactions_Date" on public."Transactions" using btree ("Date") TABLESPACE pg_default;

create index IF not exists "IX_Transactions_UserId" on public."Transactions" using btree ("UserId") TABLESPACE pg_default;

create table public."Users" (
  "Id" uuid not null,
  "Email" character varying(255) not null,
  "PasswordHash" text not null,
  "Name" character varying(255) not null,
  "CreatedAt" timestamp with time zone not null,
  "UpdatedAt" timestamp with time zone not null,
  constraint PK_Users primary key ("Id")
) TABLESPACE pg_default;

create unique INDEX IF not exists "IX_Users_Email" on public."Users" using btree ("Email") TABLESPACE pg_default;