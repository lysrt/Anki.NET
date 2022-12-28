CREATE TABLE IF NOT EXISTS [notes]
(
    [id]    integer primary key,
    [guid]  text    not null,
    [mid]   integer not null,
    [mod]   integer not null,
    [usn]   integer not null,
    [tags]  text    not null,
    [flds]  text    not null,
    [sfld]  integer not null,
    [csum]  integer not null,
    [flags] integer not null,
    [data]  text    not null
);