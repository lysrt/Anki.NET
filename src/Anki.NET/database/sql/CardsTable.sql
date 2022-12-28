CREATE TABLE IF NOT EXISTS [cards]
(
    [id]     integer primary key,
    [nid]    integer not null,
    [did]    integer not null,
    [ord]    integer not null,
    [mod]    integer not null,
    [usn]    integer not null,
    [type]   integer not null,
    [queue]  integer not null,
    [due]    integer not null,
    [ivl]    integer not null,
    [factor] integer not null,
    [reps]   integer not null,
    [lapses] integer not null,
    [left]   integer not null,
    [odue]   integer not null,
    [odid]   integer not null,
    [flags]  integer not null,
    [data]   text    not null
);